using System;
using ATAP.CryptoCurrency;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CryptoCoinsUnitTests
{
    public class GetterHTPPClientUnitTest1
    {

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

        // Property Tests

        // Method tests
        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void GetDifficultyFromAPIThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "");
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

     
        //Constructor tests
        [Fact]
        public void ConstructorReturnsSortedList()
        {
            //IGetterArgsHTTPClient a = new HTTPClientGetterArgs(Getters.HTTPClient, "GET", "http://moneroblocks.info/api/get_stats/");
            //SortableHTTPClientGetterArgs sa = new SortableHTTPClientGetterArgs(1, a);
            //SortedListHTTPClientGetterArgs c = new SortedListHTTPClientGetterArgs(sa);
            Assert.Equal(1, 0);
        }
        [Fact(DisplayName = "GetterBuilder_CreatesAnObject_OfTypeGetter")]
        public void GetterBuilder_CreatesAnObject_OfTypeGetter()
        {
            var getterbuilder = new GetterBuilder();
            var getter = getterbuilder.Build(Getters.HTTPClient);
            Assert.IsType(typeof(Getter), getter);
            getter = getterbuilder.Build(Getters.File);
            Assert.IsType(typeof(Getter), getter);
        }
        [Fact(DisplayName = "GetterBuilder_HTTPClient")]
        public void GetterBuilder_HTTPClient()
        {
            var ga = new GetterArgsHTTPClient("GET", "http://moneroblocks.info/api/get_stats/");
            var getterbuilder = new GetterBuilder();
            var getter = getterbuilder
                .Build(Getters.HTTPClient)
                ;//.GetSetGetterArgs(ga);
            Assert.IsType(typeof(Getter), getter);
        }

        [Fact]
        public void GetterHTTPClientConstructorForXMRMonero()
        {

        }


    }
}
