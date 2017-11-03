using System;
using System.Collections.Generic;
using System.Text;

namespace ATAPCryptocurrency
{


    public class XMRMoneroblocksCoinStats 
    {
        public int Difficulty { get; }
        public int Height { get; set; }
        public Decimal Hashrate { get; set; }
        public long Current_emission { get; set; }
        public long Last_reward { get; set; }
        public int Last_timestamp { get; set; }

        public XMRMoneroblocksCoinStats(int difficulty, int height, decimal hashrate, long current_emission, long last_reward, int last_timestamp)
        {
            this.Difficulty = difficulty;
            this.Height = height;
            this.Hashrate = hashrate;
            this.Current_emission = current_emission;
            this.Last_reward = last_reward;
            this.Last_timestamp = last_timestamp;
        }

        //public CryptoCoins.CoinStatsCryptoNote ConvertToCryptoNoteCoinStats ()
        //{
        //    CryptoCoins.CoinStatsCryptoNote t = new CryptoCoins.CoinStatsCryptoNote(CoinsE.XMR, DateTime.Now, TimeSpan.Zero, TimeSpan.FromMinutes(1.00), this.Hashrate, this.Last_reward, this.Difficulty);
        //    return t;
        //}
    }

}
