using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ATAP.WebGet
{
    public static class HttpResponseExtensions
    {
        public static T AsType<T>(this HttpResponseMessage response)
        {
        //ToDo, these embedded Async method calls need to be wrapped in a policy (think chunked responses, or new stuff)
        // pattern based on https://stackoverflow.com/questions/28203179/should-i-await-readasstringasync-if-i-awaited-the-response-that-im-performing
        // using the anti-pattern, this extension makes the method call synchronous. It runs the risk of deadlock
            var data = response.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ?
                            default(T) :
                            JsonConvert.DeserializeObject<T>(data);
        }
        public static string AsJson(this HttpResponseMessage response)
        {
            //ToDo, these embedded Async method calls need to be wrapped in a policy (think chunked responses, or new stuff)
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }
        public static string AsString(this HttpResponseMessage response)
        {
            //ToDo, these embedded Async method calls need to be wrapped in a policy (think chunked responses, or new stuff)
            var data = response.Content.ReadAsStringAsync().Result;
            return data.ToString();
        }

    }
    public static class HttpRequestHeadersExtensions
    {
        public static void Set(this HttpRequestHeaders headers, string name, string value)
        {
            if (headers.Contains(name)) headers.Remove(name);
            headers.Add(name, value);
        }
    }
}
