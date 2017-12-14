using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Registry;
namespace ATAP.WebGet
{
    // Within an application, there should only be one static instance of a HTPClient. This class provides that, and a set of static async tasks to interact with it.
    public interface IWebGet
    {
        Task<T> ASyncWebGetFast<T>(WebGetRegistryKey reqID);
        Task<string> ASyncWebGetFast(WebGetRegistryKey reqID);
        Task<string> ASyncWebGetFast(Policy p, HttpRequestMessage httpRequestMessage);
        Task<T> AsyncWebGetSafe<T>(WebGetRegistryKey reqID);
        Task<T> WebGetFast<T>(WebGetRegistryKey reqID);

        List<HttpStatusCode> HttpStatusCodesWorthRetrying { get; set ; }
        PolicyRegistry PolicyRegistry { get; set ; }
        WebGetRegistry WebGetRegistry { get ; set; }
    }
}
