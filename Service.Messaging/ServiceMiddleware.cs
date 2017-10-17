using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Todo.Model;

namespace Service.Messaging
{
    public class ServiceMiddleware 
    {
        private readonly RequestDelegate _next;
        private MessageQueue messageQueue;


        public ServiceMiddleware(RequestDelegate next, MessageQueue messageQueue)
        {
            _next = next;
            this.messageQueue = messageQueue;
        }

        public void MakeRequest(HttpContext context)
        {
            var message = HttpConvert.ToRequest(context.Request);
         
            var response =  messageQueue.Direct<ApiRequest, ApiResponse>(message).Result;
           

            context.Items["message queued"] = true;
            context.Items["Result"] = response;
            
        }

        public Task Invoke(HttpContext context)
        {
            try
            {

                if (!context.Request.Headers.ContainsKey("noqueue"))
                {
                    MakeRequest(context);
                }
            }
            catch (Exception e)
            {
                throw;
            }


            

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }

}

