using System;
using DaGetV2.Service;
using DaGetV2.Service.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DaGetV2.Api.Filters
{
    public class DaGetExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;

        public DaGetExceptionFilter(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public override void OnException(ExceptionContext context)
        {
           if (context.Exception is DaGetUnauthorizedException)
            {
                _loggerFactory.CreateLogger<DaGetUnauthorizedException>().LogError(context.Exception, context.Exception.Message);
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = 401;
                }
            }
            else
            {
                _loggerFactory.CreateLogger<Exception>().LogError(context.Exception, context.Exception.Message);
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = 500;
                }
            }

            context.Result = new JsonResult(new ApiErrorResultDto()
            {
                Message = context.Exception.Message,
                Details = _hostingEnvironment.IsDevelopment() ? context.Exception.ToString() : null
            });
        }
    }
}
