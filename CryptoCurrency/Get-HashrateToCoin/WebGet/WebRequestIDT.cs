using System;
namespace ATAP.WebGet
{
    public class WebRequestIDT : IWebRequestIDT
    {
        string webRequestIDStr;
        public WebRequestIDT(string webRequestIDStr)
        {
            this.webRequestIDStr = webRequestIDStr;
        }
        public string WebRequestIDStr { get => webRequestIDStr; set => webRequestIDStr = value; }
    }
}
