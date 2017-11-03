using System;

using Polly;
namespace ATAP.WebGet {
    public interface IWebGet
    {
        Policy Policy { get; set;}
    }
    public class WebGet :IWebGet {
        Policy policy;
        public WebGet()
        {
        }
        public Policy Policy { get => policy; set => policy = value; }
    }
    public interface IWebGetBuilder
    {
        WebGet Build();
    }
    public interface IWebGetSetPolicy
    {
        IWebGetBuilder SetPolicy(Policy policy);
    }
    public class WebGetBuilder : IWebGetBuilder, IWebGetSetPolicy
    {
        Policy policy;
        public WebGetBuilder()
        {
        }
        public IWebGetBuilder SetPolicy(Policy policy) { this.policy = policy;
            return this;
        }
        public WebGet Build() { return new WebGet();
        }
        public Policy Policy { get { return policy; }
            set { policy = value; }
        }
    }
}
