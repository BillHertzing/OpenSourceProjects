using System;

using Polly;
namespace ATAP.WebGet {
    public class WebRequestDataT : IWebRequestDataT
    {
        public WebRequestDataT(Policy p, string verb, string uri, string responseType, object returnType)
        {
            this.p = p;
            this.responseType = responseType;
            this.returnType = returnType;
            this.uri = uri;
            this.verb = verb;
        }
        Policy p;
        string responseType;
        object returnType;
        string uri;
        string verb;
         public Policy P { get => p; set => p = value; }
        public string ResponseType { get => responseType; set => responseType = value; }
        public object ReturnType { get => returnType; set => returnType = value; }
        public string Uri { get => uri; set => uri = value; }
        public string Verb { get => verb; set => verb = value; }
    }
}
