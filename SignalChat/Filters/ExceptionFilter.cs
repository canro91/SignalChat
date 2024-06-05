using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace SignalChat.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        var value = new
        {
            Error = exception.Message,
            exception.StackTrace
        };

        var result = new JsonResult(value)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
        context.Result = result;
    }
}