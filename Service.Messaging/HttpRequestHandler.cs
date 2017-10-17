using System;
using System.Collections.Generic;
using System.Text;
using Todo.Model;

namespace Service.Messaging
{
    public interface IMessageHandler<T1, T2>
    {
        T2 Handle(T1 msg);
    }
    public class HttpRequestHandler : IMessageHandler<ApiRequest,ApiResponse>
    {
        public ApiResponse Handle(ApiRequest msg)
        {
            return new ApiResponse();
        }
    }
}
