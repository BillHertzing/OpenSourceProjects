using System;
using UnitsNet;

namespace ATAP.CryptoCurrency
{
    public class HashRate
    {
        double hashRatePerTimeSpan;
        TimeSpan hashRateTimeSpan;
        public HashRate(double hashRatePerTimeSpan, TimeSpan hashRateSpan)
        {
            this.hashRatePerTimeSpan = hashRatePerTimeSpan;
            this.hashRateTimeSpan = hashRateSpan;
        }
        public double HashRatePerTimeSpan { get { return hashRatePerTimeSpan; } set { hashRatePerTimeSpan = value; } }
        public TimeSpan HashRateTimeSpan { get { return hashRateTimeSpan; } set { hashRateTimeSpan = value; } }

        // overload operator +
        public static HashRate operator +(HashRate a, HashRate b)
        {
            if (a.hashRateTimeSpan == b.hashRateTimeSpan)
            {
                return new HashRate(a.hashRatePerTimeSpan + b.hashRatePerTimeSpan, a.hashRateTimeSpan);
            } else
            {
                return new HashRate(a.hashRatePerTimeSpan + (b.hashRatePerTimeSpan * (a.hashRateTimeSpan.Ticks / b.hashRateTimeSpan.Ticks)), a.hashRateTimeSpan);
            }
            
        }
        // overload operator -
        public static HashRate operator -(HashRate a, HashRate b)
        {
            if (a.hashRateTimeSpan == b.hashRateTimeSpan)
            {
                return new HashRate(a.hashRatePerTimeSpan - b.hashRatePerTimeSpan, a.hashRateTimeSpan);
            }
            else
            {
                return new HashRate(a.hashRatePerTimeSpan - (b.hashRatePerTimeSpan * (a.hashRateTimeSpan.Ticks / b.hashRateTimeSpan.Ticks)), a.hashRateTimeSpan);
            }

        }

        // overload operator *
        public static HashRate operator *(HashRate a, HashRate b)
        {
            if (a.hashRateTimeSpan == b.hashRateTimeSpan)
            {
                return new HashRate(a.hashRatePerTimeSpan * b.hashRatePerTimeSpan, a.hashRateTimeSpan);
            }
            else
            {
                return new HashRate(a.hashRatePerTimeSpan * (b.hashRatePerTimeSpan * (a.hashRateTimeSpan.Ticks / b.hashRateTimeSpan.Ticks)), a.hashRateTimeSpan);
            }

        }
        // overload operator *
        public static HashRate operator /(HashRate a, HashRate b)
        {
            if (a.hashRateTimeSpan == b.hashRateTimeSpan)
            {
                return new HashRate(a.hashRatePerTimeSpan / b.hashRatePerTimeSpan, a.hashRateTimeSpan);
            }
            else
            {
                return new HashRate(a.hashRatePerTimeSpan / (b.hashRatePerTimeSpan * (a.hashRateTimeSpan.Ticks / b.hashRateTimeSpan.Ticks)), a.hashRateTimeSpan);
            }

        }

        public static HashRate ChangeTimeSpan(HashRate a, HashRate b)
        {
            // no parameter checking
            double normalizedTimeSpan = a.HashRateTimeSpan.Ticks / b.HashRateTimeSpan.Ticks;
            return new HashRate(a.hashRatePerTimeSpan * (a.hashRateTimeSpan.Ticks / b.hashRateTimeSpan.Ticks), a.hashRateTimeSpan);
        }
    }
}
