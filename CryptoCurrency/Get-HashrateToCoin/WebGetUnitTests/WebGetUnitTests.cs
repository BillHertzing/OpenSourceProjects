using System;
using System.Net.Http;
using System.Threading.Tasks;
using ATAP.DateTimeUtilities;
using ATAP.WebGet;
using Polly;
using Xunit;
namespace ATAP.WebGetUnitTests {
    public class WebGetPrimitives {
        public HttpRequestMessage jsonTest001HttpRequestMessage;
        public HttpRequestMessage jsonTest001HttpRequestMessage2;
        public WebGetHttpResponseHowToHandle jsonTest001HttpResponseCharacteristics;
        public Policy jsonTest001Policy;
        public string jsonTest001PolicyKey;
        public WebGetRegistryKey jsonTest001RegistryKey;
        public WebGetRegistryValue jsonTest001Value;
        public WebGet.WebGet webGet;
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
            jsonTest001HttpResponseCharacteristics = new WebGetHttpResponseHowToHandle();
            jsonTest001HttpResponseCharacteristics.TypeName = typeof(ResponseTo_date_jsonTest_com);
            jsonTest001Value = new WebGetRegistryValue(jsonTest001Policy, jsonTest001HttpRequestMessage, jsonTest001HttpResponseCharacteristics);
            webGetRegistry = new WebGetRegistry() { { jsonTest001RegistryKey, jsonTest001Value } };


            webGet = WebGet.WebGet.Instance;
            webGet.WebGetRegistry = webGetRegistry;

        }
    }
    public class WebGetUnitTestsForBuildersAndBasics : IClassFixture<WebGetPrimitives> {
        WebGetPrimitives webGetPrimitives;
        public WebGetUnitTestsForBuildersAndBasics(WebGetPrimitives webGetPrimitives)
        {
            this.webGetPrimitives = webGetPrimitives;
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
        [Fact]
        void WebGetRegistryConstructorReturnsProperDictionary()
        {
            WebGetRegistry webGetRegistry = new WebGetRegistry();
            Assert.IsType<WebGetRegistry>(webGetRegistry);
        }
        [Fact(Skip = "ToDo")]
        void WebGetRegistryImplementsAddMethod()
        {
        }
        [Fact(Skip = "ToDo")]
        void WebGetRegistryImplementsIEnumerable()
        {
        }
        [Fact(Skip = "ToDo")]
        void WebGetRegistryImplementsInitializer()
        {
        }

        [Fact(Skip = "ToDo")]
        void WebGetRegistryIsSortableByKey()
        {
        }
        //[Fact]
        //void WebGetRequestIDSetAndGet()
        //{
        //    string s = @"WebRequestIDStringForXMRMonero";
        //    var a = new WebGetRegistryKey(s);
        //    Assert.Equal(s, a.WebRequestIDStr);
        //}
    }
    public class WebGetUnitTestsForWebData : IClassFixture<WebGetPrimitives> {
        WebGetPrimitives webGetPrimitives;
        public WebGetUnitTestsForWebData(WebGetPrimitives webGetPrimitives)
        {
            this.webGetPrimitives = webGetPrimitives;
        }

        [Fact]
        void GetDateFromJSONTESTDotCom()
        {
            long lowerbound = DateTimeHelpers.ToUnixTime(DateTime.Now - new TimeSpan(0, 1, 0), 1);
            long upperbound = DateTimeHelpers.ToUnixTime(DateTime.Now + new TimeSpan(0, 1, 0), 1);
            var x = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y = x.Result;
            Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact]
        void GetDateFromJSONTESTDotComThriceInSerial()
        {
            TimeSpan lowerbound = TimeSpan.FromSeconds(0);
            TimeSpan upperbound = TimeSpan.FromSeconds(60);
            var x1 = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var x2 = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var x3 = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            var y2 = x2.Result;
            var y3 = x3.Result;
            TimeSpan diff = TimeSpan.FromTicks(y3.milliseconds_since_epoch - y1.milliseconds_since_epoch);
            Assert.InRange<TimeSpan>(diff, lowerbound, upperbound);
        }
        [Fact]
        void GetDateFromJSONTESTDotComTwiceInSerial()
        {
            TimeSpan lowerbound = TimeSpan.FromSeconds(0);
            TimeSpan upperbound = TimeSpan.FromSeconds(60);
            var x1 = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var x2 = webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            var y2 = x2.Result;
            TimeSpan diff = TimeSpan.FromTicks(y2.milliseconds_since_epoch - y1.milliseconds_since_epoch);
            Assert.InRange<TimeSpan>(diff, lowerbound, upperbound);
        }
    }
    public class WebGetUnitTestsForParallelism : IClassFixture<WebGetPrimitives> {
        WebGetPrimitives webGetPrimitives;
        public WebGetUnitTestsForParallelism(WebGetPrimitives webGetPrimitives)
        {
            this.webGetPrimitives = webGetPrimitives;
        }
        //[Fact]
        //void  GetDateFromJSONTESTDotComThreeTimesUsingParallelFor()
        //{
        //    Dictionary<int, long> serialresults = new Dictionary<int, long>();
        //    Dictionary<int, long> parallelresults = new Dictionary<int, long>();
        //    var stopwatch = Stopwatch.StartNew();
        //    // serial
        //    for (var i = 0;i<3;i++)
        //    {
        //        serialresults.Add(i, webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey).Result.milliseconds_since_epoch);
        //    }
        //    var sCompleteTime = stopwatch.Elapsed;
        //    // parallel
        //    Task<long>[] tasks = new Task<long>[3];
        //    var results = new SortedDictionary<int, long>();
        //    for (var j = 0; j < 3; j++)
        //    {
        //        tasks.Add .Add(webGetPrimitives.webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey));
        //    };
        //    while (tasks.Count > 0)
        //    {
        //        var task = Task.WhenAny(tasks);
        //        tasks.Remove(task);
        //        switch () { }
        //        results.Add(i, task.Result.milliseconds_since_epoch);
        //    }
        //    stopwatch.Stop();
        //    //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        //}
    }
    public class WebGetUnitTestsAdHocTests : IClassFixture<WebGetPrimitives> {
        WebGetPrimitives webGetPrimitives;
        public WebGetUnitTestsAdHocTests(WebGetPrimitives webGetPrimitives)
        {
            this.webGetPrimitives = webGetPrimitives;
        }

        // ToDo: need to find a site that always returns 503 error, and use it to validate that httpClient returns a HttpContent type that contains the value "{System.Net.Http.NoWriteNoSeekStreamContent}"

        [Fact(Skip = "AdHoc")]
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

                return await p.ExecuteAsync<T>(async() => { HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(registryValue.Req);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local fileStream and an async ReadAsByte call
                    //ToDo read response data async into an asych byteStream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType; });

            }
            var x = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y = x.Result;
            var z = y;
        //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact(Skip = "AdHoc")]
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

                return await p.ExecuteAsync<T>(async() => { HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageOuter);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local filestream and an async ReadAsByte call
                    //ToDo read response data async into an asych bytestream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType; });

            }
            var x1 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            var x2 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(webGetPrimitives.jsonTest001RegistryKey);
            var y2 = x2.Result;
            var z = y1;
        //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
        [Fact(Skip = "AdHoc")]
        void AdHoc3()
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage httpRequestMessageOuter1 = webGetPrimitives.jsonTest001HttpRequestMessage2;
            WebGetRegistryValue registryValueOuter = webGetPrimitives.jsonTest001Value;
            WebGetRegistry WebGetRegistryOuter = webGetPrimitives.webGetRegistry;
            WebGetRegistryKey WebGetRegistryKeyOuter = webGetPrimitives.jsonTest001RegistryKey;
            //Assert.Equal(registryValueOuter.Req, httpRequestMessageOuter);
            async Task<T> ASyncWebGetFastLocal<T>(HttpRequestMessage httpRequestMessage, WebGetRegistryKey webGetRegistryKey)
            {
                WebGetRegistryValue registryValue = WebGetRegistryOuter[webGetRegistryKey];
                Policy p = webGetPrimitives.jsonTest001Policy;
                HttpRequestMessage httpRequestMessageInner = HttpRequestMessageBuilder.CreateNew()
                                                                 .AddMethod(registryValue.Req.Method)
                                                                 .AddRequestUri(registryValue.Req.RequestUri)
                                                                 // .AddHeaders(registryValue.Req.Headers);
                                                                 .Build();
                //httpRequestMessageInner.Method(registryValue.Req.Method); httpRequestMessage;

                //Type typeToReturn = registryValue.Rsp.

                return await p.ExecuteAsync<T>(async() => { HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageInner);
                    //Calling the .AsType extension could cause an exception, so need to execute it wrapped in a retry policy
                    var httpResponseMessageAsType = httpResponseMessage.AsType<T>();
                    var httpResponseMessageAsJson = httpResponseMessage.AsJson();
                    var httpResponseMessageAsJ = httpResponseMessage.AsString();
                    // ToDo implement consuming a ftp file put/get using a local filestream and an async ReadAsByte call
                    //ToDo read response data async into an asych bytestream to allow processing the chunks as they arrive.
                    return httpResponseMessageAsType; });

            }
            var x1 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(httpRequestMessageOuter1, webGetPrimitives.jsonTest001RegistryKey);
            var y1 = x1.Result;
            //var x2 = ASyncWebGetFastLocal<ResponseTo_date_jsonTest_com>(httpRequestMessageOuter2, webGetPrimitives.jsonTest001RegistryKey);
            //var y2 = x2.Result;
            var z = y1;
        //Assert.InRange<long>(y.milliseconds_since_epoch, lowerbound, upperbound);
        }
    }
}