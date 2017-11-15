using System;


namespace ATAP.CryptoCurrency
{
    public class HashRate
    {
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
}
