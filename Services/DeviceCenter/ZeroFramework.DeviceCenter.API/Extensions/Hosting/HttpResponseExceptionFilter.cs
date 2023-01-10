using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZeroFramework.DeviceCenter.API.Extensions.Hosting
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is InvalidOperationException ex && ex.Message == "DisalbeModifiedDeleted")
            {
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Title = ex.Message,
                    Detail = "The demo system does not support edit and delete operations."
                });

                context.ExceptionHandled = true;
            }
        }
    }
}
