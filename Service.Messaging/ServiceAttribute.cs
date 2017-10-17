using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Formatting;
using System.Text;
using Todo.Model;

namespace Service.Messaging
{
    public class MyFilterAttribute : ActionFilterAttribute
    {
        
        
        public override void OnActionExecuting (ActionExecutingContext context)
        {
            if (context.HttpContext.Items["message queued"] != null && context.HttpContext.Items["message queued"].Equals(true))
                context.Result = new OkObjectResult("message queued");

            if (context.HttpContext.Items["Result"] != null)
            {
               
                var response = (ApiResponse)context.HttpContext.Items["Result"];
                
                context.Result = HttpConvert.ToResult(response);
            }

            base.OnActionExecuting(context);
        }

    }
}
