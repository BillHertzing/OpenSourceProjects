using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATAP.WebGet {
    public interface IWebRequestDictT {
        Dictionary<WebRequestIDT, WebRequestDataT> WebRequests { get; set; }
    }
    public class WebGet : IWebRequestDictT
    {
        Dictionary<WebRequestIDT, WebRequestDataT> webRequests;
        public WebGet()
        {
        }

        public Dictionary<WebRequestIDT, WebRequestDataT> WebRequests { get => webRequests; set => webRequests = value; }

        //public Task AsyncGet()
        //{
        //}
        //public SyncGet()
        //{
        //}
    }
    public interface IWebGetBuilder {
        WebGet Build();
    }
    public class WebGetBuilder : IWebGetBuilder {
        Dictionary<WebRequestIDT, WebRequestDataT> WebRequests { get; set; }
        public WebGetBuilder()
        {
        }
         public WebGet Build()
        {
            return new WebGet();
        }
        public static WebGetBuilder CreateNew()
        {
            return new WebGetBuilder();
        }
    }
}
