using System;

using System.Net.Http;
using Polly;
using ATAP.WebGet;

namespace ATAP.WebGet
{
    public class WebGetRegistryValue : IWebGetRegistryValue
    {
        Policy pol;
        HttpRequestMessage req;
        WebGetHttpResponseCharacteristics rsp;
        public WebGetRegistryValue(Policy pol, HttpRequestMessage req, WebGetHttpResponseCharacteristics rsp)
        {
            this.pol = pol;
            this.req = req;
            this.rsp = rsp;
        }
        //public WebGetRegistryValue(string pol, HttpRequestMessage req, WebGetHttpResponseCharacteristics rsp)
        //{
        //    this.pol = pol;
        //    this.req = req;
        //    this.rsp = rsp;
        //}
        public Policy Pol { get => pol; set => pol = value; }
        public HttpRequestMessage Req { get => req; set => req = value; }
        public WebGetHttpResponseCharacteristics Rsp { get => rsp; set => rsp = value; }

        //public ICloneable();
    }
}

public interface IWebGetRegistryValueBuilder
{
    WebGetRegistryValue Build();
}
public class WebGetRegistryValueBuilder : IWebGetRegistryValueBuilder
{
    Policy pol;
    HttpRequestMessage req;
    WebGetHttpResponseCharacteristics rsp;
    public WebGetRegistryValueBuilder()
    {
    }
    public IWebGetRegistryValueBuilder AddPolicy(Policy pol)
    {
        this.pol = pol;
        return this;
    }
    public IWebGetRegistryValueBuilder AddRequest(HttpRequestMessage req)
    {
        this.req = req;
        return this;
    }
    public IWebGetRegistryValueBuilder AddResponse(WebGetHttpResponseCharacteristics rsp)
    {
        this.rsp = rsp;
        return this;
    }
    public WebGetRegistryValue Build()
    {
        return new WebGetRegistryValue(pol, req, rsp);
    }
    public static IWebGetRegistryValueBuilder CreateNew()
    {
        return new WebGetRegistryValueBuilder();
    }

}