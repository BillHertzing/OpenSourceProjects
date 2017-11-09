using System;
namespace ATAP.WebGet
{
    public class WebGetRegistryKey : IWebGetRegistryKey
    {
        string registryKey;
        public WebGetRegistryKey(string registryKey)
        {
            this.registryKey = registryKey;
        }
        public string RegistryKey { get => registryKey; set => registryKey = value; }
    }
}
