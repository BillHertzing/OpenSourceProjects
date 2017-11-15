using System;
namespace ATAP.Cryptocurrency.WebGetClasses
{
    public class XMRMoneroBlocksCoinStats
    {
        long current_Emission;
        readonly int difficulty;
        decimal hashRate;
        int height;
        long last_Reward;
        int last_Timestamp;
        public XMRMoneroBlocksCoinStats(int difficulty, int height, decimal hashrate, long current_emission, long last_reward, int last_timestamp)
        {
            this.difficulty = difficulty;
            this.height = height;
            this.hashRate = hashrate;
            this.current_Emission = current_emission;
            this.last_Reward = last_reward;
            this.last_Timestamp = last_timestamp;
        }
        public long Current_emission { get { return current_Emission; } }
        public int Difficulty { get { return difficulty; } }
        public decimal HashRate { get { return hashRate; } }
        public int Height { get { return height; } }
        public long Last_reward { get { return last_Reward; } }
        public int Last_timestamp { get { return last_Timestamp; } }
    }



    //https://api.coinbase.com/v2/prices/BTC-USD/spot        
    public class api_coinBase_com_v2_prices_BTC_USD_spot
    {
        public api_coinBase_com_v2_prices_BTC_USD_spot_Data data { get; set; }
    }

    public class api_coinBase_com_v2_prices_BTC_USD_spot_Data
    {
        public string _base { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
    }

    //https://api.blockchain.info/charts/hash-rate?format=json

    public class api_blockChain_info_charts_hash_rate
    {
        public string status { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string period { get; set; }
        public string description { get; set; }
        public Value[] values { get; set; }
    }

    public class Value
    {
        public int x { get; set; }
        public float y { get; set; }
    }

    //https://chain.so//api/v2/get_info/BTC

    public class chain_so_api_v2_get_info
    {
        public string status { get; set; }
        public chain_so_api_v2_get_info_Data data { get; set; }
    }

    public class chain_so_api_v2_get_info_Data
    {
        public string name { get; set; }
        public string acronym { get; set; }
        public string network { get; set; }
        public string symbol_htmlcode { get; set; }
        public string url { get; set; }
        public string mining_difficulty { get; set; }
        public int unconfirmed_txs { get; set; }
        public int blocks { get; set; }
        public string price { get; set; }
        public string price_base { get; set; }
        public int price_update_time { get; set; }
        public string hashrate { get; set; }
    }

}
