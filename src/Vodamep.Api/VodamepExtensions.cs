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
            try
            {
                var handler = app.ApplicationServices.GetService<VodamepHandler>();

                // engine korrekt konfiguriert? 
                var engineFactory = app.ApplicationServices.GetService<Func<IEngine>>();
                engineFactory().Execute(new TestCommand());

                var defaultHandler = new RequestDelegate(handler.HandleDefault);
                var vodamepHandler = new RequestDelegate(handler.HandlePut);

                return app.UseRouter(r =>
                {
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

            return null;

        }
    }
}
