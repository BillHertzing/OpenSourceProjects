using System;
using ATAP.CryptoCurrency;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ATAP.CryptoCurrency.CryptoCoinAndBuilderUnitTests
{
    public class CryptoCoinBuilderUnitTests
    {
        [Fact]
        public void ReturnsObjectOfCryptoCoinTypeHavingDefaultCoinsEValue() 
        {
            var b = new CryptoCoinBuilder();
            var c = b.Build();
            Assert.Equal(c.Coin, default(CoinsE));
        }
        [Fact]
        public void WithDefaultCoinEReturnsObjectOfCryptoCoinTypeHavingDefaultCoinsEValue()
        {
            var b = new CryptoCoinBuilder();
            var c = b.AddCoin(default(CoinsE)).Build();
            Assert.Equal(c.Coin, default(CoinsE));
        }
        [Fact]
        public void WithBTCCoinEReturnsObjectOfCryptoCoinTypeHavingBTCCoinValue()
        {
            var b = new CryptoCoinBuilder();
            var c = b.AddCoin(CoinsE.BTC).Build();
            Assert.Equal(c.Coin, CoinsE.BTC);
        }
        [Fact]
        public void WithRandomCoinEReturnsObjectOfCryptoCoinTypeHavingSameCoinValue()
        {
            var lastCoinValue = Enum.GetValues(typeof(CoinsE)).Cast<CoinsE>().Max();
            var firstCoinValue = Enum.GetValues(typeof(CoinsE)).Cast<CoinsE>().Min();
            var randomCoin = (CoinsE) new Random().Next((int)firstCoinValue, (int)lastCoinValue+1);
            var b = new CryptoCoinBuilder();
            var c = b.AddCoin(randomCoin).Build();
            Assert.Equal(c.Coin, randomCoin);
        }
        public void WithNetworkHashRateReturnsObjectOfCryptoCoinTypeHavingSettableGettableNetworkHashRate()
        {
            var b = new CryptoCoinBuilder();
            
            var c = b.AddCoin(CoinsE.BTC).AddHashRate(new HashRate(10.0D, 1, new TimeSpan(0, 0, 1))).Build();
            //Assert.Equal(c.n, randomCoin);
        }
    }
    public class CryptoCoinAndBuilderUnitTests
        {
            // Test for method that returns the average number of coins generated based on coin, miner hashrate, and TimeSpan
            //[Fact]
            //public void AvgNumCoinsCreatedPerWeekIsReasonable()
            //{
            //    decimal _avgNumCCoinsCreated = 0m;
            //    Coin c = new Coin(CoinsE.XMR);
            //    CoinStats cs = new CoinStats(CoinsE.XMR);
            //    //CryptoNoteCoinStats cncs = new CryptoNoteCoinStats(Coins.XMR)
            //    Assert.NotNull(_avgNumCCoinsCreated);
            //}

        }
    }
