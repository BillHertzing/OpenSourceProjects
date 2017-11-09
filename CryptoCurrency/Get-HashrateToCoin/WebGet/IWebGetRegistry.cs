using System;
using System.Collections.Generic;

namespace ATAP.WebGet
{
    public interface IWebGetRegistry
    {
        void Add(WebGetRegistryKey webGetRegistryKey, WebGetRegistryValue webGetRegistryValue);

        Dictionary<WebGetRegistryKey, WebGetRegistryValue> Registry { get; set; }
    }
}
