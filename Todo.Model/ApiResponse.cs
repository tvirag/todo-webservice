using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Model
{
    public class ApiResponse
    {
        public string Content { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public IDictionary<string,IEnumerable<string>> Headers { get; set; }
        public MediaTypeHeaderValue ContentType { get; set; }
        public dynamic Object { get; set; }

        public ApiResponse()
        {
            Headers = new Dictionary<string, IEnumerable<string>>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
