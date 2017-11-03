using System;
using System.Collections.Generic;
using System.ComponentModel;
using static ATAP.CryptoCurrency.ExtensionHelpers;
namespace ATAP.CryptoCurrency {
    public enum Proofs {
        //[LocalizedDescription("Work", typeof(Resource))]
        //[LocalizedDescription("Work", typeof(Resource))]
        //[LocalizedDescription("Work", typeof(Resource))]
        //[LocalizedDescription("Work", typeof(Resource))]
        [Description("Work")]
        Work,
        [Description("Stake")]
        Stake,
        [Description("Burn")]
        Burn,
        [Description("Activity")]
        Activity,
        [Description("Capacity")]
        Capacity
    }
    // ToDo: automate the creation of the Algorithm enumeration and its attributes based on ???
    // ToDo: it will require the DLL version created to be part of versioning
    // RoDo: it require any changes be integrated into version control.
    public enum Algorithms {
        [Description("Casper")]
        Casper,
        [Description("CryptoNote")]
        CryptoNote,
        [Description("Hashcash")]
        Hashcash,
        [Description("Lyra2RE")]
        Lyra2RE,
        [Description("ZeroCoin")]
        ZeroCoin
    }
    // ToDo: automate the creation of the enumeration and its attributes based on the data stored in the GIT project https://github.com/crypti/cryptocurrencies.git. Also look at https://stackoverflow.com/questions/725043/dynamic-enum-in-c-sharp for making the list dynamic
    // ToDo: it will require the DLL version created to be part of versioning
    // RoDo: it require any changes be integrated into version control.
    // There are over 1500+ different documented coins so far
    public enum CoinsE {
        [Symbol("BCN")]
        [Description("Bytecoin")]
        [Proof("Work")]
        [Algorithm("CryptoNote")]
        BCN,
        [Proof("Work")]
        [Algorithm("Hashcash")]
        [Symbol("BTC")]
        [Description("BitCoin")]
        BTC,
        [Symbol("ETH")]
        [Description("Ethereum")]
        [Proof("Stake")]
        [Algorithm("Casper")]
        ETH,
        [Symbol("DSH")]
        [Description("Dashcoin ")]
        [Proof("Work")]
        [Algorithm("CryptoNote")]
        DSH,
        [Symbol("XMR")]
        [Description("Monero")]
        [Proof("Work")]
        [Algorithm("CryptoNote")]
        XMR,
        [Algorithm("ZeroCoin")]
        [Symbol("ZEC")]
        [Description("ZCoin")]
        ZEC
    }
    public class DTSandSpan {
        TimeSpan dateTimeSpan;
        DateTime dts;
        public DTSandSpan(DateTime dts, TimeSpan dateTimeSpan)
        {
            this.dts = dts;
            this.dateTimeSpan = dateTimeSpan;
        }
        public TimeSpan DateTimeSpan { get => dateTimeSpan; set => dateTimeSpan = value; }
        public DateTime Dts { get => dts; set => dts = value; }
    }
    public class Fees {
        double feeAsAPercent;
        public Fees(double feeAsAPercent)
        {
            this.feeAsAPercent = feeAsAPercent;
        }
        public double FeeAsAPercent { get => feeAsAPercent; set => feeAsAPercent = value; }
    }
    public interface IFees {
        double FeeAsAPercent { get; set; }
    }
    public class HashRate {
        double hashRatePerTimeSpan;
        TimeSpan hashRateSpan;
        int hashRateUOM;
        public HashRate(double hashRatePerTimeSpan, int uom, TimeSpan hashRateSpan)
        {
            this.hashRatePerTimeSpan = hashRatePerTimeSpan;
            hashRateUOM = uom;
            this.hashRateSpan = hashRateSpan;
        }
        public double HashRatePerTimeSpan { get { return hashRatePerTimeSpan; } set { hashRatePerTimeSpan = value; } }
        public TimeSpan HashRateSpan { get { return hashRateSpan; } set { hashRateSpan = value; } }
        public int HashRateUOM { get => hashRateUOM; set => hashRateUOM = value; }
    }
    public class BlockReward {
        double blockRewardPerBlock;
        public BlockReward(double blockRewardPerBlock)
        {
            this.blockRewardPerBlock = blockRewardPerBlock;
        }
        public double BlockRewardPerBlock { get { return blockRewardPerBlock; } set { blockRewardPerBlock = value; } }
    }
    public interface IBlockReward {
        double BlockRewardPerBlock { get; set; }
    }
    public interface IHashRates {
        Dictionary<CoinsE, List<HashRate>> HashRates { get; set; }
    }
    public interface ICryptoCoinE {
        CoinsE Coin { get; set; }
    }
    public interface IDTSandSpan {
        DTSandSpan DTSandSpan { get; set; }
    }
    public interface IHashRate {
        HashRate HashRate { get; set; }
    }
    public interface IAvgBlockTime {
        TimeSpan AvgBlockTime { get; set; }
    }
    public interface ICryptoCoin : ICryptoCoinE, IDTSandSpan, IHashRate, IAvgBlockTime {
        //CoinsE Coin { get; set; }
        //DTSandSpan DTSandSpan { get; set; }
        //HashRate HashRate { get; set; }
        }
    public class CryptoCoin : ICryptoCoinE, IDTSandSpan, IHashRate, IAvgBlockTime, IBlockReward {
        TimeSpan avgBlockTime;
        double blockRewardPerBlock;
        CoinsE coin;
        DTSandSpan dTSandSpan;
        HashRate hashRate;
        public CryptoCoin(CoinsE coin)
        {
            this.coin = coin;
        }
        public CryptoCoin(CoinsE coin, DTSandSpan dtss, HashRate hashRate, TimeSpan avgBlockTime, double blockRewardPerBlock)
        {
            this.coin = coin;
            dTSandSpan = dtss;
            this.hashRate = hashRate;
            this.avgBlockTime = avgBlockTime;
            this.blockRewardPerBlock = blockRewardPerBlock;
        }
        public TimeSpan AvgBlockTime { get => avgBlockTime; set => avgBlockTime = value; }
        public double BlockRewardPerBlock { get { return blockRewardPerBlock; } set { blockRewardPerBlock = value; } }
        public CoinsE Coin { get { return coin; }
            set { coin = value; } }
        public DTSandSpan DTSandSpan { get => dTSandSpan; set => dTSandSpan = value; }
        public HashRate HashRate { get => hashRate; set => hashRate = value; }
    }
    public class CryptoCoinBuilder {
        TimeSpan avgBlockTime;
        double blockRewardPerBlock;
        CoinsE coin;
        DTSandSpan dTSandSpan;
        HashRate hashRate;
        public CryptoCoinBuilder() { }
        public CryptoCoinBuilder AddAvgBlockTime(TimeSpan avgBlockTime)
        {
            this.avgBlockTime = avgBlockTime;
            return this;
        }
        public CryptoCoinBuilder AddBlockReward(double blockRewardPerBlock)
        {
            this.blockRewardPerBlock = blockRewardPerBlock;
            return this;
        }
        public CryptoCoinBuilder AddCoin(CoinsE coin)
        {
            this.coin = coin;
            return this;
        }
        public CryptoCoinBuilder AddDTSAndSpan(DTSandSpan dtss)
        {
            dTSandSpan = dtss;
            return this;
        }
        public CryptoCoinBuilder AddHashRate(HashRate hr)
        {
            hashRate = hr;
            return this;
        }
        public CryptoCoin Build()
        {
            return new CryptoCoin(coin, dTSandSpan, hashRate, avgBlockTime, blockRewardPerBlock);
        }
    }
//public class CryptoCoins
//{
//
//            //Coinname = coinname ?? throw new ArgumentNullException(nameof(coinname));
//            //Avgblocktime = avgblocktime == TimeSpan.Zero ? throw new ArgumentOutOfRangeException(nameof(avgblocktime)) : avgblocktime;
//            //Networkhashrate = networkhashrate == Decimal.Zero ? throw new ArgumentOutOfRangeException(nameof(networkhashrate)) : networkhashrate;
//            //Blockreward = blockreward == Decimal.Zero ? throw new ArgumentOutOfRangeException(nameof(blockreward)) : blockreward;
//            //Hashrate = hashrate == Decimal.Zero ? throw new ArgumentOutOfRangeException(nameof(hashrate)) : hashrate;
//        // provides an estimate of the probability
//        public decimal AvgNumCoinForHashrateOverTimeSpan(decimal hashrate, TimeSpan t)
//        {
//            decimal HRasAPercentOfTotal = hashrate / Networkhashrate;
//            decimal NetworkCoinsPerSecond = Blockreward / Convert.ToDecimal(Avgblocktime.TotalSeconds);
//            decimal AvgCoinsOverSpan = NetworkCoinsPerSecond;
//            return AvgCoinsOverSpan;
//        }
//    public class CoinStatsXMR : CoinStatsCryptoNote
//    {
//        public CoinStatsXMR(CoinsE coin, DateTime dts, TimeSpan dateTimeSpan, TimeSpan avgblocktime, decimal networkhashrate, decimal blockreward, decimal difficulty) : base(coin, dts, dateTimeSpan, avgblocktime, networkhashrate, blockreward, difficulty) { }
//        //public CoinStatsXMR(IHTTPClientGetter getter, IGetterArgsHTTPClient getterargs)
//        //{
//        //    CoinStatsCryptoNote cs = getter.get(getterargs);
//        //}
//    }
//public class CryptoCoinDifficulty
//{
//    string coinname;
//    string aPIfull;
//    public string Coinname { get => coinname; set => coinname = value; }
//    public string APIfull { get => aPIfull; set => aPIfull = value; }
//    public CryptoCoinDifficulty(string coinname, string aPIfull)
//    {
//        Coinname = coinname ?? throw new ArgumentNullException(nameof(coinname));
//        APIfull = aPIfull ?? throw new ArgumentNullException(nameof(aPIfull));
//    }
//    public static async Task<int> GetDifficultyFromAPI(string requestUri)
//    {
//        if (string.IsNullOrWhiteSpace(requestUri))
//        {
//            throw new ArgumentException("message", nameof(requestUri));
//        }
//        // TODO: add validation tests on the requestUri string to ensure it passes basic URI rules
//        var response = await HttpRequestFactory.Get(requestUri);
//        // TODO: throw appropriate exception if a bad response is received
//        // ToDo: parse response based on collection of requestUri rules
//        // This is for XMR stats
//        var rstr = response.ContentAsJson();
//        // parse the JSON
//        XMRMoneroblocksCoinStats stats = JsonConvert.DeserializeObject<XMRMoneroblocksCoinStats>(response.ContentAsJson());
//        int dif = stats.difficulty;
//        return dif;
//    }
//}
}
