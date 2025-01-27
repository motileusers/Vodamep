using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Vodamep.Api.CmdQry;

namespace Vodamep.Api
{
    public static class VodamepExtensions
    {
        public static IApplicationBuilder UseVodamep(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            IApplicationBuilder result = null;

            try
            {
                var handler = app.ApplicationServices.GetService<VodamepHandler>();

                var defaultHandler = new RequestDelegate(handler.HandleDefault);
                var vodamepHandler = new RequestDelegate(handler.HandlePut);

                result = app.UseRouter(r =>
                {
                    r.MapPut("{year:int}/{month:int}", vodamepHandler);
                    r.MapPost("{year:int}/{month:int}", vodamepHandler); // auch Post akzeptieren

                    r.MapPut("{report}/{year:int}/{month:int}", vodamepHandler);
                    r.MapPost("{report}/{year:int}/{month:int}", vodamepHandler); // auch Post akzeptieren

                    r.MapGet("{report}/{year:int}/{month:int}", defaultHandler);
                    r.MapGet("", defaultHandler);
                    r.DefaultHandler = new RouteHandler(defaultHandler);
                });
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }



            // engine korrekt konfiguriert? 
            try
            {
                var engineFactory = app.ApplicationServices.GetService<Func<IEngine>>();
                engineFactory().Execute(new TestCommand());

            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }

            return result;

        }
    }
}
