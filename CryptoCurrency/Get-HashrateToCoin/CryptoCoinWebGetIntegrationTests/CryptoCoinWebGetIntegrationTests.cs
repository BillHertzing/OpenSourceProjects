using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ATAP.WebGet;
using Polly;
using Xunit;
using ATAP.Cryptocurrency.WebGetClasses;
using ATAP.CryptoCurrency;

namespace ATAP.CryptoCoinWebGetIntegrationTests {
    public class FixturePrimitives
    {
        public WebGetRegistryKey webGetRegistryKeyForBTCHashRate;
        public WebGetHttpResponseHowToHandle httpResponseCharacteristicsForBTCHashRate;
        public WebGetRegistryValue webGetRegistryValueForBTCHashRate;
        public WebGet.WebGet webGet;
        public FixturePrimitives()
        {
            webGet = WebGet.WebGet.Instance;
            webGetRegistryKeyForBTCHashRate = new WebGetRegistryKey("BTCHashRate");
            httpResponseCharacteristicsForBTCHashRate = new WebGetHttpResponseHowToHandle();
            httpResponseCharacteristicsForBTCHashRate.TypeName = typeof(chain_so_api_v2_get_info);
            webGetRegistryValueForBTCHashRate = WebGetRegistryValueBuilder.CreateNew()
            .AddPolicy(Policy.NoOpAsync())
            .AddRequest(new HttpRequestMessage(HttpMethod.Get, "https://chain.so//api/v2/get_info/BTC"))
            .AddResponse(httpResponseCharacteristicsForBTCHashRate)
            .Build();
            webGet = WebGet.WebGet.Instance;
            webGet.WebGetRegistry = new WebGetRegistry() { { webGetRegistryKeyForBTCHashRate, webGetRegistryValueForBTCHashRate } }; 
        }
    }
    public class CryptoCoinWebGetIntegrationTests : IClassFixture<FixturePrimitives>
    {
        FixturePrimitives fixturePrimitives;
        public CryptoCoinWebGetIntegrationTests(FixturePrimitives fixturePrimitives)
        {
            this.fixturePrimitives = fixturePrimitives;
        }
        [Fact]
        public void GetNetworkHashRateForBTCHashRate()
        {
            // the WebOps Registry entry
            // there are more than one  WEB APIs that return this information for each coin
            // select one of the web apis to use for this (sorted based on ?? cache availability??
            // for this test, create the WebGetRegistryEntry
            fixturePrimitives.webGet.WebGetRegistry = new WebGetRegistry() { { fixturePrimitives.webGetRegistryKeyForBTCHashRate, fixturePrimitives.webGetRegistryValueForBTCHashRate } };
            var task = fixturePrimitives.webGet.ASyncWebGetFast<chain_so_api_v2_get_info>(fixturePrimitives.webGetRegistryKeyForBTCHashRate);
            var result = task.Result;
            ///ToDo: Use TryParse to better handle exceptions??
            double hashratevalue = Double.Parse(result.data.hashrate);
            Assert.InRange(hashratevalue, 1000000000000000000.0, 20000000000000000000.0);
        }
        [Theory]
        [InlineData(CoinsE.ETH, 1000000000000000000.0, 20000000000000000000.0)]
        [InlineData (CoinsE.BTC, 100000000000000000.0, 20000000000000000000.0)]
        public void GetNetworkHashRateForCoins(CoinsE coin, double lowerbound, double upperbound)
        {
            WebGetRegistryKey webGetRegistryKey = new WebGetRegistryKey(coin.ToString());
            fixturePrimitives.webGet.WebGetRegistry = new WebGetRegistry() {
                { webGetRegistryKey,
                  WebGetRegistryValueBuilder.CreateNew()
                    .AddPolicy(Policy.NoOpAsync())
                    .AddRequest(new HttpRequestMessage(HttpMethod.Get, "https://chain.so//api/v2/get_info/BTC"))
                    .AddResponse(WebGetHttpResponseHowToHandleBuilder.CreateNew()
                        .AddTypeName(typeof(chain_so_api_v2_get_info))
                        .Build())
                    .Build()
            }};
            var task = fixturePrimitives.webGet.ASyncWebGetFast<chain_so_api_v2_get_info>(webGetRegistryKey);
            var result = task.Result;
            ///ToDo: Use TryParse to better handle exceptions??
            double hashratevalue = Double.Parse(result.data.hashrate);
            Assert.InRange<double>(hashratevalue, lowerbound, upperbound);
        }
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}
        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");
        //}
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //public void ConstructorThrowsExceptionOnZeroNetworkHashRate()
        //{
        //    HashrateToCoin x = new HashrateToCoin("dummy", new System.TimeSpan(1, 0, 0, 0), 0.0m, 12.0m, 2.78m);
        //}

        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void ConstructorThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR","");
        //}

        //[Fact]
        //public async Task GetDifficultyFromAPIMonero()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");

        //}

        // Tests for getters
        //[Fact]
        //public async Task<XMRMoneroblocksCoinStats> Getter(HTTPClient,"http://moneroblocks.info/api/get_stats/")
        //{
        //    XMRMoneroblocksCoinStats cs= new XMRMoneroblocksCoinStats()
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "http://moneroblocks.info/api/get_stats/");
        //    int diff = await CryptoCoinDifficulty.GetDifficultyFromAPI("http://moneroblocks.info/api/get_stats/");
        //    Assert.IsNotNull(diff, "difficulty returned null");

        //}
        [Fact]
        public void NumCoinsGenerated()
        {
        // specify the coin
        //CoinsE coinname =CoinsE.BTC;
        // get the networkhashrate
        //api_coinbase_com_v2_prices_BTC_USD_spot bTCUSDSpot;
        //string uRIStr = "https://api.coinbase.com/v2/prices/BTC-USD/spot";
        //"JSON"
        // Policy p = 
        }
    }
    public class CryptoCoinWebGetAdHocTests {
        class ResponseTo_date_jsonTest_com
        {
            public string time { get; set; }
            public long milliseconds_since_epoch { get; set; }
            public string date { get; set; }
        }
        [Fact]
        public void AdHoc()
        {

        //HttpRequestMessage jsonTest001HttpRequestMessage;
        //    HttpRequestMessage jsonTest001HttpRequestMessage2;
        //    WebGetHttpResponseCharacteristics jsonTest001HttpResponseCharacteristics;
        //    Policy jsonTest001Policy;
        //    WebGetRegistryKey jsonTest001RegistryKey;
        //    WebGetRegistryValue jsonTest001Value;
        //    WebGet.WebGet webGet;
        //    WebGetRegistry webGetRegistry;
        //    webGet = WebGet.WebGet.Instance;
        //    jsonTest001RegistryKey = new WebGetRegistryKey("jsonTest001");
        //    jsonTest001Policy = Policy.NoOpAsync();

        //    jsonTest001HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://date.jsontest.com/");
        //    jsonTest001HttpRequestMessage2 = new HttpRequestMessage(HttpMethod.Get, "http://date.jsontest.com/");
        //    jsonTest001HttpResponseCharacteristics = new WebGetHttpResponseCharacteristics();
        //    jsonTest001HttpResponseCharacteristics.TypeName = typeof(ResponseTo_date_jsonTest_com);
        //    jsonTest001Value = new WebGetRegistryValue(jsonTest001Policy, jsonTest001HttpRequestMessage, jsonTest001HttpResponseCharacteristics);
        //    webGetRegistry = new WebGetRegistry() { { jsonTest001RegistryKey, jsonTest001Value } };

        //    var MSDownloadBlock = new TransformBlock<string,string>(()=>
        //    {

        //    }
        //    Task ProduceMSAsync(ITargetBlock<long> target)  {
        //        var x1 = webGet.ASyncWebGetFast<ResponseTo_date_jsonTest_com>(jsonTest001RegistryKey);
        //        target.Post(x1.Result.milliseconds_since_epoch);
        //        return;
        //    };
        //    async Task<long> ConsumeMSAsync(ISourceBlock<long> source) { long l = 0; while(await source.OutputAvailableAsync()) { long innerl = source.Receive(); l = innerl; } return l; }
        //    var lbb = new BufferBlock<long>();
        //    var consumer = ConsumeMSAsync(lbb);
        //    ProduceMSAsync.Wait();
        //    consumer.Wait();
        //    Tuple<int, long, string> tu3 = new Tuple<int, long , string>(1, consumer.Result, "a");
        //    T3 t3 = new T3();
        //    // spawn a task to populate each property of t3, and await them all
        //    Task getT3int = .Run;
        //    Task getT3long = .Run;
        }
        class T3 {
            public int i;
            public long l;
            public string s;
            public T3()
            {
            }
            public T3(int i, long l, string s)
            {
                this.i = i;
                this.l = l;
                this.s = s;
            }
        }
    }
}
