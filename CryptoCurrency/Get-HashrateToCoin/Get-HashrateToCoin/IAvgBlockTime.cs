using System;
namespace ATAP.CryptoCurrency
{
    public interface IAvgBlockTime
    {
        TimeSpan AvgBlockTime { get; set; }
    }
}
