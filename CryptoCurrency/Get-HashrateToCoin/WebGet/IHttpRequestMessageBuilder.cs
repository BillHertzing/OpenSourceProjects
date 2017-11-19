using System;
using System.Net.Http;
namespace ATAP.WebGet
{
    public interface IHttpRequestMessageBuilder
    {
        HttpRequestMessage Build();
    }
}
