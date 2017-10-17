using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Service.Messaging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Todo.Model;

namespace TestConsole
{
    class Program
    {
        static async Task<ApiResponse> Run(ApiRequest msg)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = msg.RequestUri,
                    Content = new StringContent(msg.Content, System.Text.Encoding.UTF8, msg.ContentType),
                    Method = msg.Method
                };
                foreach (var keyValue in msg.Headers)
                {
                    try
                    {
                        request.Headers.Add(keyValue.Key, keyValue.Value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                }
                request.Headers.Add("noqueue", new string[] { "true" });

                var response = await client.SendAsync(request);



                var message = HttpConvert.ToResponse(response);

                Console.WriteLine(message);
                return message;
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new MessageQueue().Respond<ApiRequest, ApiResponse>(Run);
        }
    }
}
