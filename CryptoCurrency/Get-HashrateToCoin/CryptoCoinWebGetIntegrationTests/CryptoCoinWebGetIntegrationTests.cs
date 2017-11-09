using ATAP.Cryptocurrency.WebGetClasses;
using ATAP.CryptoCurrency;
using ATAP.WebGet;
using System;
using Xunit;

namespace ATAP.CryptoCoinWebGetIntegrationTests
{
    public class CryptoCoinWebGetIntegrationTests
    {
        [Fact]
        public void GetNetworkHashRate()
        {
            // the WebOps Registry entry
            // there are more than one  WEB APIs that return this information for each coin
            // select one of the web apis to use for this (sorted based on ?? cache availability??
            // for this test, create the WebGetRegistryEntry

            WebGetRegistry wgr = new WebGetRegistry() { { new WebGetRegistryKey ("BTCHAshRate"), WebGetRegistryValueBuilder.CreateNew().Build() } };

            
        }
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
}
