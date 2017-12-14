using System;
using ATAP.DateTimeUtilities;
namespace ATAP.CryptoCurrency
{
    public interface ICryptoCoinNetworkInfo : ICryptoCoinE, IDTSandSpan, IHashRate, IAvgBlockTime
    {
    }
}
