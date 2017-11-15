using System;
using ATAP.CryptoCurrency;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ATAP.CryptoCurrency.CryptoCoinAndBuilderUnitTests
{
    public class CryptoCoinPrimitives
    {
        TimeSpan timeSpan;
        double hashRatePerTimeSpan;
        int hashRateUOM;
        HashRate hashRate;
        HashRate hashRate2X;
        HashRate hashRate2T;
        HashRate hashRate1000X;
        PowerConsumption powerConsumption;
        double feeAsAPercent;
        List<CoinsE> coinlist;


        public TimeSpan TimeSpan { get => timeSpan; set => timeSpan = value; }
        public HashRate HashRate { get => hashRate; set => hashRate = value; }
        public HashRate HashRate2X { get => hashRate2X; set => hashRate2X = value; }
        public HashRate HashRate2T { get => hashRate2T; set => hashRate2T = value; }
        public double HashRatePerTimeSpan { get => hashRatePerTimeSpan; set => hashRatePerTimeSpan = value; }
        public int HashRateUOM { get => hashRateUOM; set => hashRateUOM = value; }
        public PowerConsumption PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
        public double FeeAsAPercent { get => feeAsAPercent; set => feeAsAPercent = value; }
        public List<CoinsE> CoinList { get => coinlist; set => coinlist = value; }

        public CryptoCoinPrimitives()
        {
            timeSpan = new TimeSpan(0, 0, 1);
            hashRatePerTimeSpan = 10000;
            hashRateUOM = 1;
            hashRate = new HashRate(hashRatePerTimeSpan, hashRateUOM, timeSpan);
            hashRate2X = new HashRate(hashRatePerTimeSpan * 2, hashRateUOM, timeSpan);
            hashRate2T = new HashRate(hashRatePerTimeSpan, hashRateUOM, timeSpan + timeSpan);
            hashRate1000X = new HashRate(hashRatePerTimeSpan, hashRateUOM * 1000, timeSpan);
            feeAsAPercent = 1.0;
            coinlist = new List<CoinsE> { CoinsE.ETH, CoinsE.BTC };
            
        }
    }
        public class CryptoCoinUnitTests : IClassFixture<CryptoCoinPrimitives>
    {
        CryptoCoinPrimitives cryptoCoinPrimitives;
        public CryptoCoinUnitTests(CryptoCoinPrimitives cryptoCoinPrimitives)
        {
            this.cryptoCoinPrimitives = cryptoCoinPrimitives;
        }
        [Fact]
        public void HashRateTest()
        {
            var h = new HashRate(cryptoCoinPrimitives.HashRatePerTimeSpan, cryptoCoinPrimitives.HashRateUOM, cryptoCoinPrimitives.TimeSpan);
            Assert.Equal(h.HashRatePerTimeSpan, cryptoCoinPrimitives.HashRatePerTimeSpan);
        }
    }
        public class CryptoCoinBuilderUnitTests : IClassFixture<CryptoCoinPrimitives>
    {
        CryptoCoinPrimitives cryptoCoinPrimitives;
        public CryptoCoinBuilderUnitTests(CryptoCoinPrimitives cryptoCoinPrimitives)
        {
            this.cryptoCoinPrimitives = cryptoCoinPrimitives;
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
                        .AddHashRate(cryptoCoinPrimitives.HashRate)
                        .Build();
            Assert.Equal(a.HashRate.HashRatePerTimeSpan, cryptoCoinPrimitives.HashRate.HashRatePerTimeSpan);
            Assert.Equal(a.HashRate.HashRatePerTimeSpan, cryptoCoinPrimitives.HashRate.HashRatePerTimeSpan);
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
