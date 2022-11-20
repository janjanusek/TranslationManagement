using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TranslationManagement.Api.Exceptions;

namespace TranslationManagement.Api.Filters;

public class ExceptionsActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context is not { Exception: null } and { ExceptionHandled: false })
        {
            context.Result = context.Exception switch
            {
                NotFoundException notFound => new JsonResult(null)
                {
                    StatusCode = (int?)HttpStatusCode.NotFound,
                    Value = notFound.GetDescriptiveValue()
                },
                Exception internalServerError => new JsonResult(null)
                {
                    StatusCode = (int?)HttpStatusCode.InternalServerError,
                    Value = internalServerError 
                }
            };
            context.ExceptionHandled = true;
        }
    }
}