using System;
using ATAP.DateTimeUtilities;
namespace ATAP.CryptoCurrency
{
    public interface ICryptoCoin : ICryptoCoinE, IDTSandSpan, IHashRate, IAvgBlockTime
    {
    }
}
