using System;

using System.Net.Http;
using Polly;
using System.Collections.Generic;
using ATAP.WebGet;
using System.Collections;

namespace ATAP.WebGet
{


    public class WebGetHttpResponseCharacteristics : IWebGetHttpResponseCharacteristics
    {

        bool follow3xx;
        int follow3xxDepth;
        Type typeName;
        public WebGetHttpResponseCharacteristics()
        {
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
}
