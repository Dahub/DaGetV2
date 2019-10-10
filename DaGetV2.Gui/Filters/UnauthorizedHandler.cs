namespace DaGetV2.Gui
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class UnauthorizedHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                if (!context.HttpContext.Response.HasStarted)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
        }
    }
}
