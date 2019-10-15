namespace DaGetV2.Api.Filters
{
    using System;
    using System.Net;
    using ApplicationCore.DTO;
    using ApplicationCore.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

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
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else if (context.Exception is DaGetNotFoundException)
            {
                _loggerFactory.CreateLogger<DaGetNotFoundException>().LogError(context.Exception, context.Exception.Message);
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            else if (context.Exception is DaGetServiceException)
            {
                _loggerFactory.CreateLogger<DaGetServiceException>().LogError(context.Exception, context.Exception.Message);
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                _loggerFactory.CreateLogger<Exception>().LogError(context.Exception, context.Exception.Message);
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
