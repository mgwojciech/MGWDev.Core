using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Utilities
{
    public class HttpClientHttpHelper : IHttpHelper
    {
        HttpClient Client { get; set; }
        public ISerializer Serializer { get; set; } = new JSONSerilizer();
        public event EventHandler<WebRequest> OnExecuting;
        public HttpClientHttpHelper(HttpClient client)
        {
            Client = client;
        }
        public T CallAPI<T, U>(string url, string method, U data = null) where U : class
        {
            HttpResponseMessage response = null;
            if(!Client.DefaultRequestHeaders.Contains("Accept"))
            {
                Client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
            HttpContent content = CreateRequestData<U>(data);
            switch (method)
            {
                case "GET":
                    {
                        response = Client.GetAsync(url).Result;
                        break;
                    }
                case "POST":
                    response = Client.PostAsync(url, content).Result;
                    break;
            }
            return Serializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected virtual HttpContent CreateRequestData<U>(U data) where U : class
        {
            return new StringContent(Serializer.Serialize(data));
        }
    }
}
