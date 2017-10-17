using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Todo.WebApi.Utility
{
    public class NotFoundFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;

            var notFound = new NotFoundObjectResult("Resource could not be found");
            
            if (result != null && result.Value == null)
            {
                context.Result = notFound;
                
            }

        }
    }
}
