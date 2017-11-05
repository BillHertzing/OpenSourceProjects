using System;
using ATAP.WebGet;
using Xunit;
namespace ATAP.WebGetUnitTests {
    public class WebGetUnitTests {
        [Fact]
        void WebGetRequestIDSetAndGet()
        {
            string s = @"WebRequestIDStringForXMRMonero";
            var a = new WebRequestIDT(s);
            Assert.Equal(s, a.WebRequestIDStr);
            
        }
        [Fact]
        void WebGetBuilder001StaticAndInstanceConstructorsReturnTheSameType()
        {
            var a = new WebGetBuilder();
            var b = WebGetBuilder.CreateNew();
            Assert.Equal(a.GetType(), b.GetType());
        }
        
       [Fact(Skip = "Need to figure out how to compare types")]
        void WebGetBuilder002StaticConstructorsReturnsWebGetBuilderType()
        {
            var a = WebGetBuilder.CreateNew();
            Assert.IsType(a.GetType(), typeof(WebGet.WebGetBuilder));
        }
        [Fact(Skip = "Need to figure out how to compare types")]
        void WebGetBuilder003BuildMethodReturnsWebGetType()
        {
            var a = WebGetBuilder.CreateNew().Build();
            var b = Type.GetType("ATAP.WebGet.WebGet");
            Assert.IsType(a.GetType(),b);
        }

    }
}
