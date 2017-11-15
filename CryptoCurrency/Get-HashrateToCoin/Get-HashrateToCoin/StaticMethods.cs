using System;
using System.Collections;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ATAP.CryptoCurrency
{
    public interface IAverageShareOfBlockRewardDT
    {
        TimeSpan AverageBlockCreationSpan { get; set; }
        TimeSpan Duration { get; set; }
        HashRate MinerHashRate { get; set; }
        HashRate NetworkHashRate { get; set; }
                double BlockRewardPerBlock { get; set; }
    }

    public interface IROAverageShareOfBlockRewardDT
    {
        TimeSpan AverageBlockCreationSpan { get; }
        TimeSpan Duration { get; }
        HashRate MinerHashRate { get; }
        HashRate NetworkHashRate { get; }
                double BlockRewardPerBlock { get; }
    }

    /* the minimum data fields needed to calculate one miners average share of total coins mined in a time period */
    public class AverageShareOfBlockRewardDT : IAverageShareOfBlockRewardDT, IROAverageShareOfBlockRewardDT
    {
        TimeSpan averageBlockCreationSpan;
        TimeSpan duration;
        HashRate minerHashRate;
        HashRate networkHashRate;
        
        double blockRewardPerBlock;

        public AverageShareOfBlockRewardDT(TimeSpan averageBlockCreationSpan, TimeSpan duration, HashRate minerHashRate, HashRate networkHashRate,  double blockRewardPerBlock)
        {
            this.averageBlockCreationSpan = averageBlockCreationSpan;
            this.duration = duration;
            this.minerHashRate = minerHashRate;
            this.networkHashRate = networkHashRate;
            
            this.blockRewardPerBlock = blockRewardPerBlock;
        }

        public TimeSpan AverageBlockCreationSpan
        {
            get { return averageBlockCreationSpan; }
            set { averageBlockCreationSpan = value; }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public HashRate MinerHashRate
        {
            get { return minerHashRate; }
            set { minerHashRate = value; }
        }
        public HashRate NetworkHashRate
        {
            get { return networkHashRate; }
            set { networkHashRate = value; }
        }
        public double BlockRewardPerBlock
        {
            get { return blockRewardPerBlock; }
            set { blockRewardPerBlock = value; }
        }
    }

    /** Read-only thread safe Lazy loaded Singleton Instance class with the minimum data fields needed to calculate one miners average share of total coins mined in a time period */
    public sealed class NTSP_AverageShareOfBlockRewardDT : AverageShareOfBlockRewardDT
    {
        //static readonly Lazy<NTSP_NormalizedAverageShareOfEmittedCoins> lazy =
        //new Lazy<NTSP_NormalizedAverageShareOfEmittedCoins>(() => new NTSP_NormalizedAverageShareOfEmittedCoins());

        public NTSP_AverageShareOfBlockRewardDT(TimeSpan averageBlockCreationSpan, TimeSpan duration, HashRate minerHashRate, HashRate networkHashRate,  double blockRewardPerBlock) :
          base(averageBlockCreationSpan, duration, minerHashRate, networkHashRate,  blockRewardPerBlock)
        {
        }

        //public static NTSP_NormalizedAverageShareOfEmittedCoins Instance { get { return lazy.Value; } }
    }


    public sealed class InterestingCoins
    {
        static readonly Lazy<InterestingCoins> lazy =
    new Lazy<InterestingCoins>(() => new InterestingCoins());

        public InterestingCoins()
        {
        }

        public static InterestingCoins Instance { get { return lazy.Value; } }
    }
    // NormalizedAverageShareOfEmittedCoins is a set of properties/fields, aka a DS.
    // NormalizedAverageShareOfEmittedCoinsDS_FromHTTPClient
    // 

    public partial class CryptoCoin
    {
        public static HashRate NormalizeHashRateSafe(HashRate fromHashRate, HashRate toHashRate) {
            // ToDo: Add tests to ensure the UOM is neither null nor zero
            // ToDo: Add tests to ensure the Span is neither null nor zero
            return NormalizeHashRateFast(fromHashRate, toHashRate);
           }
        public static HashRate NormalizeHashRateFast(HashRate fromHashRate, HashRate toHashRate)
        {
            // no parameter checking
            double normalizedUOM = fromHashRate.HashRateUOM / toHashRate.HashRateUOM;
            double normalizedTimeSpan = (fromHashRate.HashRateSpan).TotalMilliseconds / (toHashRate.HashRateSpan).TotalMilliseconds;
            return new HashRate((fromHashRate.HashRatePerTimeSpan * normalizedUOM / normalizedTimeSpan), toHashRate.HashRateUOM, toHashRate.HashRateSpan);
        }
        public static double AverageShareOfBlockRewardPerNetworkHashRateSpanSafe(AverageShareOfBlockRewardDT data)
        {
            // ToDo: Add parameter checking
                return AverageShareOfBlockRewardPerNetworkHashRateSpanFast(data);
        }
            public static double AverageShareOfBlockRewardPerNetworkHashRateSpanFast(AverageShareOfBlockRewardDT data)
        {
            // normalize into normalizedMinerHashRate the MinerHashRate to the same uom and span as the NetworkHashRate
            HashRate normalizedMinerHashRate = NormalizeHashRateFast(data.MinerHashRate, data.NetworkHashRate);
            double HashRateAsAPercentOfTotal = normalizedMinerHashRate.HashRatePerTimeSpan / data.NetworkHashRate.HashRatePerTimeSpan;
            // normalize the BlockRewardPerSpan to the same span the network HashRate span
            double normalizedBlockCreationSpan = data.AverageBlockCreationSpan.TotalMilliseconds / data.NetworkHashRate.HashRateSpan.TotalMilliseconds;
            double normalizedBlockRewardPerSpan = data.BlockRewardPerBlock / (data.AverageBlockCreationSpan.TotalMilliseconds * normalizedBlockCreationSpan);
            // The number of block rewards found, on average, within a given TimeSpan, is number of blocks in the span, times the fraction of the NetworkHashRate contributed by the miner
            return  normalizedBlockRewardPerSpan *(normalizedMinerHashRate.HashRatePerTimeSpan/data.NetworkHashRate.HashRatePerTimeSpan) ;
            
        }
    }
    //public class FromJSON1
    //{

    //    public FromJSON1(string r)
    //    {
    //        async Task<FromJSON1> GetAsync(string requestUri)
    //        {

    //            //ToDO: Better validation on getter
    //            if (string.IsNullOrWhiteSpace(requestUri))
    //            {
    //                //ToDo: Better error handling on the throw
    //                throw new ArgumentException("message", nameof(requestUri));
    //            }
    //            // TODO: add validation tests on the requestUri string to ensure it passes basic URI rules
    //            var response = await HttpRequestFactory.Get(requestUri);
    //            // TODO: throw appropriate exception if a bad response is received
    //            // ToDo: parse response based on collection of requestUri rules - generalize the conversion based on the enumerations
    //            // This is for XMR stats
    //            var rstr = response.ContentAsJson();
    //            // parse the JSON
    //            //ToDo: encapsulate the specific type to which the JSON returned by that specific URI can be Converted by the JSONConvert<specificType> into the method parameter
    //            FromJSON1 stats = JsonConvert.DeserializeObject<FromJSON1>(response.ContentAsJson());

    //            return stats;

    //        }
    //        //FromJSON1 t = await GetAsync("http://moneroblocks.info/api/get_stats/");
    //        Difficulty = 1;
    //        Height = 2;
    //        HashRate = 3.0m;
    //        Current_emission = 5;
    //        Last_reward = 2;
    //        Last_timestamp = 2;
    //        ;
    //    }



    //public CryptoCoins.CoinStatsCryptoNote ConvertToCryptoNoteCoinStats()
    //{
    //        CryptoCoins.CoinStatsCryptoNote t = new CryptoCoins.CoinStatsCryptoNote(CoinsE.XMR, DateTime.Now, TimeSpan.Zero, TimeSpan.FromMinutes(1.00), HashRate, Last_reward, Difficulty);
    //        return t;
    //    }



}