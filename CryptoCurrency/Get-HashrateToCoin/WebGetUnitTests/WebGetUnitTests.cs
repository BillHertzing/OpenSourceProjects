using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ATAP.WebGet;
using static ATAP.DateTimeHelpers.DateTimeHelpers;
using Polly;
using Polly.Retry;
using Xunit;
using System.Threading.Tasks;

namespace ATAP.WebGetUnitTests {
    public class WebGetPrimitives {
        public WebGet.WebGet webGet;
        public WebGetRegistryKey jsonTest001RegistryKey;
        public string jsonTest001PolicyKey;
        public Policy jsonTest001Policy;
        public WebGetRegistryValue jsonTest001Value;
        public HttpRequestMessage jsonTest001HttpRequestMessage;
        public HttpRequestMessage jsonTest001HttpRequestMessage2;
        public HttpRequestMessage jsonTest001HttpRequestMessage3;
        public WebGetHttpResponseCharacteristics jsonTest001HttpResponseHandler;
        
        ResponseTo_date_jsonTest_com jsonTest001ResponseType = new ResponseTo_date_jsonTest_com();

        public WebGetRegistry webGetRegistry;

        public WebGetPrimitives()
        {
            //ToDo refactor the primitives and tests so that individual validation occurs before using the built-in policies
            webGet = WebGet.WebGet.Instance;
            jsonTest001RegistryKey = new WebGetRegistryKey("jsonTest001");
            jsonTest001PolicyKey = "policyTimeout30Seconds";
            jsonTest001Policy = Policy.NoOpAsync();
            //jsonTest001Policy = webGet.PolicyRegistry.Get< IAsyncPolicy < HttpResponseMessage >>(jsonTest001PolicyKey);
            jsonTest001HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://date.jsontest.com/");
            jsonTest001HttpRequestMessage2 = new HttpRequestMessage(HttpMethod.Get, "http://date.jsontest.com/");
            jsonTest001HttpRequestMessage3 = new HttpRequestMessage(HttpMethod.Get, "http://date.jsontest.com/");
            jsonTest001HttpResponseHandler = new WebGetHttpResponseCharacteristics();
            jsonTest001HttpResponseHandler.TypeName = typeof(ResponseTo_date_jsonTest_com);
            jsonTest001Value = new WebGetRegistryValue(jsonTest001Policy, jsonTest001HttpRequestMessage , jsonTest001HttpResponseHandler);
            webGetRegistry = new WebGetRegistry() { { jsonTest001RegistryKey, jsonTest001Value } };

            webGet = WebGet.WebGet.Instance;
            webGet.WebGetRegistry = webGetRegistry;

        }
    }
    public class WebGetUnitTests : IClassFixture<WebGetPrimitives> {
        WebGetPrimitives webGetPrimitives;
        public WebGetUnitTests(WebGetPrimitives webGetPrimitives)
        {
            this.webGetPrimitives = webGetPrimitives;
        }

        // ToDo: need to find a site that always returns 503 error, and use it to validate that httpClient returns a HttpContent type that contains the value "{System.Net.Http.NoWriteNoSeekStreamContent}"
        [Fact]
        void GetDateFromJSONTESTDotCom()
        {
            long lowerbound = ATAP.DateTimeHelpers.DateTimeHelpers.ToUnixTime(DateTime.Now - new TimeSpan(0, 1, 0));
            long upperbound = ATAP.DateTimeHelpers.DateTimeHelpers.ToUnixTime(DateTime.Now + new TimeSpan(0, 1, 0));
            //var t = webGetPrimitives.webGet.WebGetRegistry.Registry[webGetPrimitives.jsonTest001RegistryKey].Rsp.TypeName;
             var x = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y = x.Result;
            Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact]
        void AdHoc1()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessageOuter = webGetPrimitives.jsonTest001HttpRequestMessage;
            WebGetRegistryValue registryValueOuter = webGetPrimitives.jsonTest001Value;
            WebGetRegistry WebGetRegistryOuter = webGetPrimitives.webGetRegistry;
            WebGetRegistryKey WebGetRegistryKeyOuter = webGetPrimitives.jsonTest001RegistryKey;
            Assert.Equal(registryValueOuter.Req, httpRequestMessageOuter);
            async Task<T> ASyncWebGetFastLocal<T>(WebGetRegistryKey webGetRegistryKey)
            {
                WebGetRegistryValue registryValue = WebGetRegistryOuter[webGetRegistryKey];
                Policy p = webGetPrimitives.jsonTest001Policy;
                //Type typeToReturn = registryValue.Rsp.

                return await p.ExecuteAsync<T>(async () => {
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(registryValue.Req);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local filestream and an async ReadAsByte call
                    //ToDo read response data async into an asych bytestream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType;
                });

            }
            var x = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y = x.Result;
            var z = y;
            //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact]
        void AdHoc2()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessageOuter = webGetPrimitives.jsonTest001HttpRequestMessage;
            WebGetRegistryValue registryValueOuter = webGetPrimitives.jsonTest001Value;
            WebGetRegistry WebGetRegistryOuter = webGetPrimitives.webGetRegistry;
            WebGetRegistryKey WebGetRegistryKeyOuter = webGetPrimitives.jsonTest001RegistryKey;
            Assert.Equal(registryValueOuter.Req, httpRequestMessageOuter);
            async Task<T> ASyncWebGetFastLocal<T>(WebGetRegistryKey webGetRegistryKey)
            {
                WebGetRegistryValue registryValue = WebGetRegistryOuter[webGetRegistryKey];
                Policy p = webGetPrimitives.jsonTest001Policy;
                //Type typeToReturn = registryValue.Rsp.

                return await p.ExecuteAsync<T>(async () => {
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageOuter);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local filestream and an async ReadAsByte call
                    //ToDo read response data async into an asych bytestream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType;
                });

            }
            var x1 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            var x2 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y2 = x2.Result;
            var z = y1;
            //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact]
        void AdHoc3()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessageOuter1 = webGetPrimitives.jsonTest001HttpRequestMessage2;
            HttpRequestMessage httpRequestMessageOuter2 = webGetPrimitives.jsonTest001HttpRequestMessage3;
            WebGetRegistryValue registryValueOuter = webGetPrimitives.jsonTest001Value;
            WebGetRegistry WebGetRegistryOuter = webGetPrimitives.webGetRegistry;
            WebGetRegistryKey WebGetRegistryKeyOuter = webGetPrimitives.jsonTest001RegistryKey;
            //Assert.Equal(registryValueOuter.Req, httpRequestMessageOuter);
            async Task<T> ASyncWebGetFastLocal<T>(HttpRequestMessage httpRequestMessage, WebGetRegistryKey webGetRegistryKey)
            {
                WebGetRegistryValue registryValue = WebGetRegistryOuter[webGetRegistryKey];
                Policy p = webGetPrimitives.jsonTest001Policy;
                HttpRequestMessage httpRequestMessageInner = httpRequestMessage;
                //Type typeToReturn = registryValue.Rsp.

                return await p.ExecuteAsync<T>(async () => {
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageInner);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local filestream and an async ReadAsByte call
                    //ToDo read response data async into an asych bytestream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType;
                });

            }
            var x1 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(httpRequestMessageOuter1, webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            var x2 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(httpRequestMessageOuter2, webGetPrimitives.jsonTest001RegistryKey);
            var y2 = x2.Result;
            var z = y1;
            //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact(Skip = "needtofigureouhowtovalidateasingleton")]
        void WebGet001ValidateSingleton()
        {
        //    var a = new WebGet();
        //    var b = WebGet.CreateNew();
        //    Assert.Equal(a.GetType(), b.GetType());
        //}
        //[Fact(Skip = "Need to figure out how to compare types")]
        //void WebGet002StaticConstructorsReturnsWebGetType()
        //{
        //    var a = WebGet.CreateNew();
        //    Assert.IsType(a.GetType(), typeof(WebGet.WebGet));
        //}
        //[Fact(Skip = "Need to figure out how to compare types")]
        //void WebGetBuilder003BuildMethodReturnsWebGetType()
        //{
        //    var a = WebGetBuilder.CreateNew()
        //                .Build();
        //    var b = Type.GetType("ATAP.WebGet.WebGet");
        //    Assert.IsType(a.GetType(), b);
        }
        //[Fact]
        //void WebGetRequestIDSetAndGet()
        //{
        //    string s = @"WebRequestIDStringForXMRMonero";
        //    var a = new WebGetRegistryKey(s);
        //    Assert.Equal(s, a.WebRequestIDStr);
        //}
    }

}
namespace ATAP.DateTimeHelpers
{
    public static class DateTimeHelpers {
    public static long ToUnixTime(this DateTime date)
    {
        return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }
}
}