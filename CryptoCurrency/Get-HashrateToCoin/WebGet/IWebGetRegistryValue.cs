using System;

using System.Net.Http;
using Polly;

namespace ATAP.WebGet
{
    public interface IWebGetRegistryValue
    {
        Policy Pol { get; set; }
        HttpRequestMessage Req { get; set; }
        WebGetHttpResponseHowToHandle Rsp { get; set; }
    }
}
