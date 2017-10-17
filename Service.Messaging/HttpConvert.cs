using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Todo.Model;

namespace Service.Messaging
{
    public class HttpConvert
    {
        public static ApiRequest ToRequest(HttpRequest request)
        {

            var message = new ApiRequest();
            message.RequestUri = new Uri(request.GetEncodedUrl());
            message.Method = new HttpMethod(request.Method);
            message.Content = new StreamContent(request.Body).ReadAsStringAsync().Result;
            message.ContentType = request.ContentType;
            message.ContentLength = request.ContentLength;
          
           
            message.Cookies = request.Cookies.ToDictionary(t => t.Key, y => y.Value);

            foreach (var keyValue in request.Headers)
            {
                message.Headers.Add(keyValue.Key, keyValue.Value.ToArray());
            }
            return message;
        }

        public static ApiResponse ToResponse(HttpResponseMessage response)
        {
            var message = new ApiResponse();
            message.Content = response.Content.ReadAsStringAsync().Result;
            message.ContentType = response.Content.Headers.ContentType;
            message.IsSuccessStatusCode = response.IsSuccessStatusCode;
            message.ReasonPhrase = response.ReasonPhrase;
            message.StatusCode = response.StatusCode;
           
           
            if (message.ContentType != null && message.ContentType.MediaType == "application/json")
                message.Object = response.Content.ReadAsAsync<dynamic>().Result;
                    
            foreach (var keyValue in response.Headers)
            {
                message.Headers.Add(keyValue.Key, keyValue.Value.ToArray());
            }

            return message;
        }


        public static IActionResult ToResult(ApiResponse response)
        {
            var hasContent = response.ContentType != null;
            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var location = response.Headers.ContainsKey("location") ? response.Headers["location"].First() : string.Empty;

                
                var createdResult = new CreatedResult(location, response.Object);
                createdResult.ContentTypes.Add(response.ContentType.MediaType);
                
                return createdResult;
            }
            
            
            
            return hasContent ? new ContentResult {
                Content = response.Content,
                ContentType = response.ContentType.MediaType,
                StatusCode = (int)response.StatusCode
                

            } as IActionResult :  new StatusCodeResult((int)response.StatusCode);
        }
    }
}
