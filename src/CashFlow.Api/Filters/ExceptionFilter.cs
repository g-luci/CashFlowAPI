using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is CashFlowException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknownError(context);
            }

        }

        private void HandleProjectException(ExceptionContext context)
        {
            if (context.Exception is ErrorOnValidationException errorOnValidationException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                context.Result = new BadRequestObjectResult(new ResponseErrorJson(errorOnValidationException.Errors));
            }
            else if (context.Exception is NotFoundException notFoundException) 
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                context.Result = new NotFoundObjectResult(new ResponseErrorJson(notFoundException.Message));
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                context.Result = new BadRequestObjectResult(new ResponseErrorJson(context.Exception.Message));
            }

        }

        private void ThrowUnknownError(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
 
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR));
        }
    }
}
