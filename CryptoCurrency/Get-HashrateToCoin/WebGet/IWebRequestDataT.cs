using Polly;

namespace ATAP.WebGet
{
    public interface IWebRequestDataT
    {
        Policy P { get; set; }
        string Uri { get; set; }
        string Verb { get; set; }
    }
}