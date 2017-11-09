using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Polly;
using Polly.Bulkhead;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;
namespace ATAP.WebGet {
    // Within an application, there should only be one static instance of a HTPClient. This class provides that, and a set of static async tasks to interact with it.
    // see this article for "why" https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
    // This article was used as a basis for ensuring that there is only one instance of WebGet in an app: http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class WebGet {
        static readonly Lazy<WebGet> lazy = new Lazy<WebGet>(() => new WebGet());
        HttpClient httpClient;
        List<HttpStatusCode> httpStatusCodesWorthRetrying;
        PolicyRegistry policyRegistry;
        WebGetRegistry webGetRegistry;
        
        WebGet()
        {
            httpClient = new HttpClient();
            policyRegistry = new PolicyRegistry();
            httpStatusCodesWorthRetrying = new List<HttpStatusCode> {
                HttpStatusCode.RequestTimeout, // 408
                 HttpStatusCode.InternalServerError, // 500
                 HttpStatusCode.BadGateway, // 502
                 HttpStatusCode.ServiceUnavailable, // 503
                 HttpStatusCode.GatewayTimeout // 504
                 };

            TimeoutPolicy policyTimeout30Seconds = Policy.TimeoutAsync(new TimeSpan(0, 0, 30), TimeoutStrategy.Optimistic);
            // ToDo, add a BulkheadPolicy that pairs with an action to run if the bulkhead rejects and the queue is full
            BulkheadPolicy policyBulkhead50Q150 = Policy.BulkheadAsync(50, 150);
            policyRegistry.Add("policyBulkhead50Q150", policyBulkhead50Q150);
            policyRegistry.Add("policyTimeout30Seconds", policyTimeout30Seconds);
            policyRegistry.Add("policyWaitAndRetry3TimesOnResponseContainsHttpStatusCodesWorthRetrying", Policy.HandleResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode)).WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));
            policyRegistry.Add("policyWaitAndRetry3TimesOnRequestException",  Policy.Handle<HttpRequestException>().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));

            // PolicyWrap policyFullyResilient = Policy.Wrap(fallback, cache, retry, breaker, bulkhead, timeout);

            // 
            webGetRegistry = new WebGetRegistry();

        }
        public async Task<T> ASyncWebGetFast<T>(WebGetRegistryKey reqID)
        {
            WebGetRegistryValue registryValue = webGetRegistry[reqID];
            Policy p = registryValue.Pol;
            httpClient = new HttpClient();
            //Type typeToReturn = registryValue.Rsp.

            return await p.ExecuteAsync<T>(async () => {
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(registryValue.Req);
                var x= httpResponseMessage.AsType<T>();
                return x;
            });

        }
        public async Task<string> ASyncWebGetFast(WebGetRegistryKey reqID)
        {
            WebGetRegistryValue registryValue = webGetRegistry[reqID];
            Policy p = registryValue.Pol;
            return await p.ExecuteAsync(async () =>
            {
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(registryValue.Req);
                HttpContent httpContent = httpResponseMessage.Content;
                var str = httpContent.ReadAsStringAsync().Result;
                return str;
            });
        }
        public Task<T> AsyncWebGetSafe<T>(WebGetRegistryKey reqID)
        {
            throw new NotImplementedException("'AsyncWebGetSafe<T> not yet implemented");
        // ToDo Add validation tests
        // ToDo does the dictionary contain this key
        //return ASyncWebGetFast<T>(reqID);
        }
        public List<HttpStatusCode> HttpStatusCodesWorthRetrying { get => httpStatusCodesWorthRetrying; set => httpStatusCodesWorthRetrying = value; }
        public static WebGet Instance { get { return lazy.Value; } }
        public PolicyRegistry PolicyRegistry { get => policyRegistry; set => policyRegistry = value; }
        public WebGetRegistry WebGetRegistry { get => webGetRegistry; set => webGetRegistry = value; }
    }
    public class HttpRequestMessageBuilder : IHttpRequestMessageBuilder {
        string acceptHeader;
        string bearerToken;
        HttpContent content;
        HttpMethod method;
        Uri requestUri;
        public HttpRequestMessageBuilder()
        {
        }
        public HttpRequestMessageBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }
        public HttpRequestMessageBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }
        public HttpRequestMessageBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }
        public HttpRequestMessageBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }
        public HttpRequestMessageBuilder AddRequestUri(Uri requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }
        public HttpRequestMessage Build()
        {
            HttpRequestMessage hrm = new HttpRequestMessage(method, requestUri);
            if(content != default(HttpContent)) { hrm.Content = content; };
            if(bearerToken != default(string)) { hrm.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken); };
            //ToDo: further research, is .Headers.Accept.Clear() needed on a newly created HttpRequestMessage?
            hrm.Headers.Accept.Clear();
            if(acceptHeader != default(string)) { hrm.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader)); };
            return hrm;
        }
        public static HttpRequestMessageBuilder CreateNew()
        {
            return new HttpRequestMessageBuilder();
        }
    }
}
