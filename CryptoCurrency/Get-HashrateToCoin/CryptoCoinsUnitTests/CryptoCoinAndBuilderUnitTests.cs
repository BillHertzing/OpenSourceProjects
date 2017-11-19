using System;
using ATAP.CryptoCurrency;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ATAP.CryptoCurrency.CryptoCoinAndBuilderUnitTests
{
    public class Fixture
    {
        public TimeSpan hashRateTimeSpan;
        public double hashRatePerTimeSpan;
        public HashRate hashRate;
        public HashRate hashRate2X;
        public HashRate hashRate2T;
        public HashRate hashRate1000X;
        public PowerConsumption powerConsumption;
        public double feeAsAPercent;
        public List<CoinsE> coinList;

        public HashRate HashRate { get => hashRate; set => hashRate = value; }
        public double HashRatePerTimeSpan { get => hashRatePerTimeSpan; set => hashRatePerTimeSpan = value; }


        public Fixture()
        {
            hashRateTimeSpan = new TimeSpan(0, 0, 1);
            hashRatePerTimeSpan = 10000;
            
            hashRate = new HashRate(hashRatePerTimeSpan,  hashRateTimeSpan);
            hashRate2X = new HashRate(hashRatePerTimeSpan * 2,  hashRateTimeSpan);
            hashRate2T = new HashRate(hashRatePerTimeSpan,  hashRateTimeSpan + hashRateTimeSpan);
            hashRate1000X = new HashRate(hashRatePerTimeSpan * 1000,  hashRateTimeSpan);
            feeAsAPercent = 1.0;
            coinList = new List<CoinsE> { CoinsE.ETH, CoinsE.BTC };
            
        }
    }
        public class CryptoCoinHashRateUnitTests : IClassFixture<Fixture>
    {
        Fixture fixture;
        public CryptoCoinHashRateUnitTests(Fixture cryptoCoinPrimitives)
        {
            this.fixture = cryptoCoinPrimitives;
        }
        [Fact]
        public void HashRateConstructorTest()
        {
            var h = new HashRate(fixture.HashRatePerTimeSpan, fixture.hashRateTimeSpan);
            Assert.Equal(h.HashRatePerTimeSpan, fixture.HashRatePerTimeSpan);
            Assert.Equal(h.HashRateTimeSpan, fixture.hashRateTimeSpan);
        }
    }
        public class CryptoCoinBuilderUnitTests : IClassFixture<Fixture>
    {
        Fixture fixture;
        public CryptoCoinBuilderUnitTests(Fixture cryptoCoinPrimitives)
        {
            this.fixture = cryptoCoinPrimitives;
        }
        [Fact]
        public void ReturnsObjectOfCryptoCoinTypeHavingDefaultCoinsEValue() 
        {
            var b = new CryptoCoinBuilder();
            var c = b.Build();
            Assert.Equal(default(CoinsE), c.Coin);
        }
        [Fact]
        public void WithDefaultCoinEReturnsObjectOfCryptoCoinTypeHavingDefaultCoinsEValue()
        {
            var b = new CryptoCoinBuilder();
            var c = b.AddCoin(default(CoinsE)).Build();
            Assert.Equal(default(CoinsE), c.Coin);
        }
        [Fact]
        public void WithBTCCoinEReturnsObjectOfCryptoCoinTypeHavingBTCCoinValue()
        {
            var b = new CryptoCoinBuilder();
            var c = b.AddCoin(CoinsE.BTC).Build();
            Assert.Equal(CoinsE.BTC, c.Coin);
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
        [Fact]
        public void WithNetworkHashRateReturnsObjectOfCryptoCoinTypeHavingSettableGettableNetworkHashRate()
        {
            var a = CryptoCoinBuilder.CreateNew()
                        .AddCoin(CoinsE.BTC)
                        .AddHashRate(fixture.HashRate)
                        .Build();
            Assert.Equal(a.HashRate.HashRatePerTimeSpan, fixture.HashRate.HashRatePerTimeSpan);
            Assert.Equal(a.HashRate.HashRatePerTimeSpan, fixture.HashRate.HashRatePerTimeSpan);
        }
        [Fact]
        void NumCoinsCreated()
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

        //[Fact]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void GetDifficultyFromAPIThrowsArgumentExceptionOnIsNullOrWhiteSpace()
        //{
        //    CryptoCoinDifficulty D = new CryptoCoinDifficulty("XMR", "");
        //}
    }

}
