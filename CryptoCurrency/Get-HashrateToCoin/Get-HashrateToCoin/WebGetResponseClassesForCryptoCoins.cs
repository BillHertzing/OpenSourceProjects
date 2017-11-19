using System;
namespace ATAP.Cryptocurrency.WebGetClasses {
    public class XMRMoneroBlocksCoinStats {
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
}

