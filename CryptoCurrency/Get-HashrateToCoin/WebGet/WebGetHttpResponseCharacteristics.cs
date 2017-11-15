using System;

using System.Net.Http;
using Polly;
using System.Collections.Generic;
using ATAP.WebGet;
using System.Collections;

namespace ATAP.WebGet
{


    public class WebGetHttpResponseHowToHandle : IWebGetHttpResponseWebGetHttpResponseHowToHandle
    {

        bool follow3xx;
        int follow3xxDepth;
        Type typeName;
        public WebGetHttpResponseHowToHandle()
        {
        }
        public WebGetHttpResponseHowToHandle(Type typeName)
        {
            this.typeName = typeName;
        }
        public bool Follow3xx { get => follow3xx; set => follow3xx = value; }
        public int Follow3xxDepth { get => follow3xxDepth; set => follow3xxDepth = value; }
        public Type TypeName { get => typeName; set => typeName = value; }
        // ToDo: Handle 3xx response headers
        // ToDo: Handle set cookies in response headers
        // ToDo: Handle 401 authentication responses
        // ToDo: handle strings
        // ToDo: handle YAML
        // ToDo: handle JSON
    }

    public interface IWebGetHttpResponseHowToHandle
    {
        WebGetHttpResponseHowToHandle Build();
    }
    public class WebGetHttpResponseHowToHandleBuilder
    {
        Type typeName;
        public WebGetHttpResponseHowToHandleBuilder()
        {
        }

        public WebGetHttpResponseHowToHandleBuilder AddTypeName(Type typeName)
        {
            this.typeName = typeName;
            return this;
        }
        public WebGetHttpResponseHowToHandle Build()
        {
            return new WebGetHttpResponseHowToHandle(typeName);
        }
        public static WebGetHttpResponseHowToHandleBuilder CreateNew()
        {
            return new WebGetHttpResponseHowToHandleBuilder();
        }
    }
    }
