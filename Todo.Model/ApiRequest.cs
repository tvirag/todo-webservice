using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Todo.Model
{
    public class ApiRequest 
    {
      
        public Uri RequestUri
        {
            get;set;

        }

        public  HttpMethod Method
        {
            get;set;
        }
        public  IDictionary<string, IEnumerable<string>> Headers
        {
            get;set;
        }
        public  string Content
        {
            get;set;
        }

        public IDictionary<string,string> Cookies
        {
            get;set;
        }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }

        public ApiRequest()
        {
            this.Headers = new Dictionary<string, IEnumerable<string>>();
        }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented );
        }
    }
}
