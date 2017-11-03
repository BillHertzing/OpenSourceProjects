using System;
using System.Threading.Tasks;
using Fiver.Api.HttpClient;
using Newtonsoft.Json;

namespace ATAP.CryptoCurrency
{
    public interface INormalizedAverageShareOfEmittedCoins
    {
        double AverageBlockCreationSpan { get; set; }
        TimeSpan Duration { get; set; }
        double MinerHashRate { get; set; }
        double NetworkHashRate { get; set; }
        double NormalizationFactor { get; set; }
        double NumCoinsEmittedPerBlock { get; set; }
    }

    public interface IRONormalizedAverageShareOfEmittedCoins
    {
        double AverageBlockCreationSpan { get;  }
        TimeSpan Duration { get;  }
        double MinerHashRate { get;  }
        double NetworkHashRate { get;  }
        double NormalizationFactor { get;  }
        double NumCoinsEmittedPerBlock { get;  }
    }

    /* the minimum data fields needed to calculate one miners average share of total coins mined in a time period */
    public class NormalizedAverageShareOfEmittedCoins : INormalizedAverageShareOfEmittedCoins, IRONormalizedAverageShareOfEmittedCoins
    {
        double averageBlockCreationSpan;
        TimeSpan duration;
        double minerHashRate;
        double networkHashRate;
        double normalizationFactor;
        double numCoinsEmittedPerBlock;

        public NormalizedAverageShareOfEmittedCoins(double averageBlockCreationSpan, TimeSpan duration, double minerHashRate, double networkHashRate, double normalizationFactor, double numCoinsEmittedPerBlock)
        {
            this.averageBlockCreationSpan = averageBlockCreationSpan;
            this.duration = duration;
            this.minerHashRate = minerHashRate;
            this.networkHashRate = networkHashRate;
            this.normalizationFactor = normalizationFactor;
            this.numCoinsEmittedPerBlock = numCoinsEmittedPerBlock;
        }

        public double AverageBlockCreationSpan
        {
            get { return averageBlockCreationSpan; }
            set { averageBlockCreationSpan = value; }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public double MinerHashRate
        {
            get { return minerHashRate; }
            set { minerHashRate = value; }
        }
        public double NetworkHashRate
        {
            get { return networkHashRate; }
            set { networkHashRate = value; }
        }
        public double NormalizationFactor
        {
            get { return normalizationFactor; }
            set { normalizationFactor = value; }
        }
        public double NumCoinsEmittedPerBlock
        {
            get { return numCoinsEmittedPerBlock; }
            set { numCoinsEmittedPerBlock = value; }
        }
    }

    /** Read-only thread safe Lazy loaded Singleton Instance class with the minimum data fields needed to calculate one miners average share of total coins mined in a time period */
    public sealed class NTSP_NormalizedAverageShareOfEmittedCoins : NormalizedAverageShareOfEmittedCoins
    {
        //static readonly Lazy<NTSP_NormalizedAverageShareOfEmittedCoins> lazy =
    //new Lazy<NTSP_NormalizedAverageShareOfEmittedCoins>(() => new NTSP_NormalizedAverageShareOfEmittedCoins());

        public NTSP_NormalizedAverageShareOfEmittedCoins(double averageBlockCreationSpan, TimeSpan duration, double minerHashRate, double networkHashRate, double normalizationFactor, double numCoinsEmittedPerBlock) :
          base(averageBlockCreationSpan, duration, minerHashRate, networkHashRate, normalizationFactor, numCoinsEmittedPerBlock)
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

    //    public FromJSON1(int difficulty, int height, decimal hashrate, long current_emission, long last_reward, int last_timestamp)
    //    {
    //        Difficulty = difficulty;
    //        Height = height;
    //        HashRate = hashrate;
    //        Current_emission = current_emission;
    //        Last_reward = last_reward;
    //        Last_timestamp = last_timestamp;
    //    }

        //public CryptoCoins.CoinStatsCryptoNote ConvertToCryptoNoteCoinStats()
        //{
    //        CryptoCoins.CoinStatsCryptoNote t = new CryptoCoins.CoinStatsCryptoNote(CoinsE.XMR, DateTime.Now, TimeSpan.Zero, TimeSpan.FromMinutes(1.00), HashRate, Last_reward, Difficulty);
    //        return t;
    //    }

    //    public long Current_emission { get; set; }
    //    public int Difficulty { get; }
    //    public decimal HashRate { get; set; }
    //    public int Height { get; set; }
    //    public long Last_reward { get; set; }
    //    public int Last_timestamp { get; set; }
    //}

    //class Program
    //{

    //    FromJSON1 fromJSON1 = new FromJSON1("http://moneroblocks.info/api/get_stats/");

    //}
}

