using System;
using System.Threading.Tasks;
using Fiver.Api.HttpClient;
using Newtonsoft.Json;

namespace ATAP.CryptoCurrency
{
    public interface IResponse<out T>
    {
        T Value { get; }
    }

    public class GetterHTTPClient<T> //: IResponse<T>
    {
        //T _value;

        //T Value { get => _value; }

        public async Task<T> GetAsync(IGetterArgsHTTPClient hTTPClientGetterArgs)
        {
            var requestUri = hTTPClientGetterArgs.URI;
            //ToDO: Better validation on getter
            if (string.IsNullOrWhiteSpace(requestUri))
            {
                //ToDo: Better error handling on the throw
                throw new ArgumentException("message", nameof(requestUri));
            }
            // TODO: add validation tests on the requestUri string to ensure it passes basic URI rules
            var response = await HttpRequestFactory.Get(requestUri);
            // TODO: throw appropriate exception if a bad response is received
            // ToDo: parse response based on collection of requestUri rules - generalize the conversion based on the enumerations
            // This is for XMR stats
            var rstr = response.ContentAsJson();
            // parse the JSON
            //ToDo: encapsulate the specific type to which the JSON returned by that specific URI can be Converted by the JSONConvert<specificType> into the method parameter
            //XMRMoneroblocksCoinStats stats = JsonConvert.DeserializeObject<XMRMoneroblocksCoinStats>(response.ContentAsJson());
            //var x = stats.ConvertToCryptoNoteCoinStats();
            // toDo return x as type T
            return default(T);

        }

        public T GetSync(IGetterArgsHTTPClient hTTPClientGetterArgs)
        {
            return GetAsync(hTTPClientGetterArgs).GetAwaiter().GetResult();
        }

    }
}
