using System;
using System.Collections.Generic;
namespace ATAP.CryptoCurrency
{
    public interface IHashRatesDict
    {
        Dictionary<CoinsE, HashRate> HashRates { get; set; }
    }
}
