using System;
using System.Collections;
using System.Collections.Generic;

namespace ATAP.WebGet
{
    public class WebGetRegistry : IEnumerable<WebGetRegistryValue>, IWebGetRegistry
    {
        Dictionary<WebGetRegistryKey, WebGetRegistryValue> registry;
        public WebGetRegistry()
        {
            registry = new Dictionary<WebGetRegistryKey, WebGetRegistryValue>();
        }

        public Dictionary<WebGetRegistryKey, WebGetRegistryValue> Registry { get => registry; set => registry = value; }

        // to use a collection initializer, the collection must implement GetEnumerator in two different ways, 
        public IEnumerator<WebGetRegistryValue> GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            //forces use of the non-generic implementation on the Values collection
            return ((IEnumerable)registry.Values).GetEnumerator();
        }
        // to use a collection initializer, the collection must implement Add 
        public void Add(WebGetRegistryKey webGetRegistryKey, WebGetRegistryValue webGetRegistryValue)
        {
            registry.Add(webGetRegistryKey, webGetRegistryValue);
        }
        // to use a [] as an indexer the collection must implement the ITEM property
        public WebGetRegistryValue this[WebGetRegistryKey key] { get {return registry[key]; } set { registry[key] = value; } }
    }
}
