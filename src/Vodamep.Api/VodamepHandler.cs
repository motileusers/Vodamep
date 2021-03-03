using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Connexia.Service.Client;
using Vodamep.Api.CmdQry;
using Vodamep.ReportBase;

namespace Vodamep.Api
{
    public class VodamepHandler
    {
        private readonly IValidationClient _validationClient;
        private readonly Func<IEngine> _engineFactory;
        private readonly ILogger<VodamepHandler> _logger;
        private readonly bool _useAuthentication;

        public VodamepHandler(Func<IEngine> engineFactory, bool useAuthentication, ILogger<VodamepHandler> logger, IValidationClient validationClient)
        {
            _validationClient = validationClient;
            _engineFactory = engineFactory;
            _logger = logger;
            _useAuthentication = useAuthentication;
        }
        private async Task RespondError(HttpContext context, string message)
        {
            _logger?.LogWarning(message);

            var result = new SendResult() { IsValid = false, ErrorMessage = message };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteJson(result);
        }

        private async Task RespondSuccess(HttpContext context, string message)
        {
            var result = new SendResult() { IsValid = true, Message = message };

            context.Response.StatusCode = StatusCodes.Status200OK;

            await context.Response.WriteJson(result);
        }


        public async Task HandleDefault(HttpContext context)
        {
            _logger?.LogInformation("Handle Default");

            if (_useAuthentication)
            {
                await EnsureIsAuthenticated(context);

                if (!IsAuthenticated(context))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }

            var assembly = this.GetType().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"Vodamep.Api.swagger.yaml");
            using (var reader = new StreamReader(resourceStream))
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync(reader.ReadToEnd());
            }
        }

        public async Task HandlePut(HttpContext context)
        {
            _logger?.LogInformation("Handle Put");

            if (context.Request.Method != HttpMethods.Put && context.Request.Method != HttpMethods.Post)
            {
                await HandleDefault(context);
                return;
            }

            if (_useAuthentication)
            {
                await EnsureIsAuthenticated(context);

                if (!IsAuthenticated(context))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    _logger?.LogInformation(String.Format("User {0} unauthorized.", context.User.Identity?.Name));

                    return;
                }

                _logger?.LogInformation(String.Format("Authentication for user {0} successfull.", context.User.Identity?.Name));
            }

            int.TryParse((string)context.GetRouteValue("year"), out int year);
            int.TryParse((string)context.GetRouteValue("month"), out int month);
            var reportTypeAsString = (string) context.GetRouteValue("report");

            _logger?.LogInformation($"Report type from route: {reportTypeAsString}");

            //todo legacy code
            if (string.IsNullOrWhiteSpace(reportTypeAsString))
            {
                reportTypeAsString = ReportType.Hkpv.ToString();
            }

            if (year < 2000 || year > DateTime.Today.Year)
            {
                await RespondError(context, $"Ungültiges Jahr '{context.GetRouteValue("year")}'");
                return;
            }

            if (month < 1 || month > 12)
            {
                await RespondError(context, $"Ungültiger Monat '{context.GetRouteValue("month")}'");
                return;
            }

            _logger?.LogInformation("Reading data.");

            IReportBase report;

            try
            {
                var reportType = (ReportType) Enum.Parse(typeof(ReportType), reportTypeAsString, true);
                report = new ReportFactory().Create(reportType, context.Request.Body);
            }
            catch
            {
                _logger?.LogError("Deserialize failed.");
                report = null;
            }

            if (report == null)
            {
                await RespondError(context, $"Die Daten können nicht gelesen werden.");
                return;
            }

            var date = new DateTime(year, month, 1);
            if (report.FromD != date)
            {
                await RespondError(context, $"Ungültiger Zeitraum: {year}-{month}, entspricht nicht {report.From}.");
                return;
            }

            try
            {
                var test = await _validationClient.ValidateByUserAndInstitutionAsync(context.User.Identity?.Name, report.Institution.Id);
            }
            catch (Exception exception)
            {
                _logger?.LogError(exception, "Benutzer darf keine Daten für diese Einrichtung senden");
                await RespondError(context, "Benutzer darf keine Daten für diese Einrichtung senden");
                return;
            }

            var validationResult = await new VodamepValidator().Validate(report);
            if (validationResult == null)
            {
                await RespondError(context, "Validierung konnte nicht durchgeführt werden.");
            }

            if (!validationResult.Item1)
            {
                await RespondError(context, validationResult.Item2);
                return;
            }

            var saveCmd = new ReportSaveCommand() { Report = report };
            var engine = _engineFactory();

            if (_useAuthentication)
            {
                engine.Login(context.User);
            }
            else
            {
                var identity = new ClaimsIdentity("anonymous");
                identity.AddClaim(new Claim(ClaimTypes.Name, "anonymous"));

                engine.Login(new ClaimsPrincipal(identity));
            }
            engine.Execute(saveCmd);

            await RespondSuccess(context, validationResult.Item2);

            _logger?.LogInformation("Report received.");
        }

        private bool IsAuthenticated(HttpContext context) => context.User != null && !string.IsNullOrEmpty(context.User.Identity?.Name);
        private async Task EnsureIsAuthenticated(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync(BasicAuthenticationDefaults.AuthenticationScheme);

            if (authResult.None)
                await context.ChallengeAsync(BasicAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
