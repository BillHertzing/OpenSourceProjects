using Fiver.Api.HttpClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using static ATAP.CryptoCurrency.ExtensionHelpers;

namespace ATAP.CryptoCurrency
{
        public enum Getters
        {
            [Description("HTTPClient")]
            HTTPClient,
            [Description("File")]
            File
        }


    //public class GetterArgs : IGetterArgs
    //{
    //    GetterArgs _getterArgs;

    //    public GetterArgs GetterArgs { get => _GetterArgs;  }

    //    public GetterArgs(GetterArgsv GetterArgs)
    //    {
    //        _getterArgs = GetterArgs;
    //    }
    //}

    public interface IFileGetterArgs : IGetterArgs
    {
        string Path { get; }
    }

    //public class FileGetterArgs : GetterArgs, IFileGetterArgs
    //{
    //    private string _path;

    //    string IFileGetterArgs.Path { get { return _path; } }

    //    public FileGetterArgs(Getters getter, string path) : base(getter)
    //    {
    //        _path = path;
    //    }
    //    public FileGetterArgs(Getters getter) : this(getter, "") { }
    //}


   
    //public class SortableHTTPClientGetterArgs : HTTPClientGetterArgs, IComparable
    //{
    //    private int _sortPreference;
    //    private HTTPClientGetterArgs _a;
    //    public int SortPreference { get => _sortPreference; set => _sortPreference = value; }
    //    public HTTPClientGetterArgs A { get => _a; set => _a = value; }

    //    public SortableHTTPClientGetterArgs(int sortPreference, HTTPClientGetterArgs a):base(a.Getter)
    //    {
    //        SortPreference = sortPreference;
    //        A = a ?? throw new ArgumentNullException(nameof(a));
    //    }

    //    private class SortBySortpreference : IComparer
    //    {
    //        int IComparer.Compare(object a, object b)
    //        {
    //            SortableHTTPClientGetterArgs x = (SortableHTTPClientGetterArgs)a;
    //            SortableHTTPClientGetterArgs y = (SortableHTTPClientGetterArgs)b;
    //            if (x.SortPreference > y.SortPreference)
    //                return 1;
    //            if (x.SortPreference < y.SortPreference)
    //                return -1;
    //            else
    //                return 0;
    //        }
    //    }
    //    // Implement IComparable CompareTo to provide default sort order.
    //    int IComparable.CompareTo(object obj)
    //    {
    //        SortableHTTPClientGetterArgs x = (SortableHTTPClientGetterArgs)obj;
    //        return this.SortPreference.CompareTo(x.SortPreference);
    //    }
    //}
    //public class SortedListHTTPClientGetterArgs
    //{
    //    private SortableHTTPClientGetterArgs _list;

    //    public SortableHTTPClientGetterArgs List { get => _list; set => _list = value; }

    //    public SortedListHTTPClientGetterArgs(SortableHTTPClientGetterArgs list)
    //    {
    //        List = list ?? throw new ArgumentNullException(nameof(list));
    //    }


    //}

    public interface IGetterHTTPClient<T>
    {
        IGetterArgsHTTPClient GetterArgsHTTPClient { get; }
        Task<T> GetAsync(IGetterArgsHTTPClient hTTPClientGetterArgs);
    }

    //public class HTTPClientGetter : IHTTPClientGetter
    //{

    //    private HTTPClientGetterArgs _hTTPClientGetterArgs;
    //    public IGetterArgsHTTPClient HTTPClientGetterArgs { get { return _hTTPClientGetterArgs; } }

    //    public HTTPClientGetter(HTTPClientGetterArgs hTTPClientGetterArgs)
    //    {
    //        _hTTPClientGetterArgs = hTTPClientGetterArgs ?? throw new ArgumentNullException(nameof(hTTPClientGetterArgs));
    //    }

    //    public async Task<CoinStatsCryptoNote> GetAsync(IGetterArgsHTTPClient hTTPClientGetterArgs)
    //    {
    //        var requestUri = hTTPClientGetterArgs.URI;
    //        //ToDO: Better validation on getter
    //        if (string.IsNullOrWhiteSpace(requestUri))
    //        {
    //            //ToDo: Better error handling on the throw
    //            throw new ArgumentException("message", nameof(requestUri));
    //        }
    //        // TODO: add validation tests on the requestUri string to ensure it passes basic URI rules
    //        var response = await HttpRequestFactory.Get(requestUri);
    //        // TODO: throw appropriate exception if a bad response is received
    //        // ToDo: parse response based on collection of requestUri rules - generalize the conversion based on the enumerations
    //        // This is for XMR stats
    //        var rstr = response.ContentAsJson();
    //        // parse the JSON
    //        //ToDo: encapsulate the specific type to which the JSON returned by that specific URI can be Converted by the JSONConvert<specificType> into the method parameter
    //        XMRMoneroblocksCoinStats stats = JsonConvert.DeserializeObject<XMRMoneroblocksCoinStats>(response.ContentAsJson());
    //        CoinStatsCryptoNote cs = stats.ConvertToCryptoNoteCoinStats();
    //        return cs;
    //        ;
    //    }
    //}

    public interface IGetterArgsHTTPClient : IGetterArgs
    {
        string Verb { get; }
        string URI { get; }
    }
    public class GetterArgsHTTPClient : IGetterArgs, IGetterArgsHTTPClient
    {

        private string _verb;
        private string _uri;

        string IGetterArgsHTTPClient.Verb { get { return _verb; } }
        string IGetterArgsHTTPClient.URI { get { return _uri; } }

        public IGetterArgs GetterArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public IGetterArgs GetterArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GetterArgsHTTPClient(string verb, string uri)
        {
            _verb = verb;
            _uri = uri;
        }

    }

    public class GetterArgs<T> : IGetterArgs, IGetterArgsHTTPClient
    {
        public GetterArgs(string verb, string uri)
        {
            _verb = verb;
            _uri = uri;
        }
        private string _verb;
        private string _uri;

        string IGetterArgsHTTPClient.Verb { get { return _verb; } }
        string IGetterArgsHTTPClient.URI { get { return _uri; } }

        IGetterArgs IGetterArgs.GetterArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public IGetterArgs GetterArgs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public GetterArgsHTTPClient(string verb, string uri)
        //{
        //    _verb = verb;
        //    _uri = uri;
        //}
    }

    public interface IGetterArgs
    {
        IGetterArgs GetterArgs { get; set; }
    }
    public interface IGetter
    {
        Getters Kind { get; }
    }

    public class Getter : IGetter, IGetterArgs
    {
        private Getters _kind;
        public Getters Kind { get { return _kind; } }

        private Dictionary<Getters, IGetterArgs> GetterArgsDict { get; set; }

        public IGetterArgs GetterArgsHTTPClient
        {
            get
            {
                if (GetterArgsDict.ContainsKey(Getters.HTTPClient))
                {
                    return GetterArgsDict[Getters.HTTPClient];
                }
                else
                {
                    throw new NotSupportedException("This Getter has no GetterArgs for a Getters.HTTPClient get");

                }
            }
            set
            {
                //ToDo: determine if validation of not null is needed
                GetterArgsDict[Getters.HTTPClient] = value;
            }
        }

        public IGetterArgs GetterArgsFile
        {
            get
            {
                if (GetterArgsDict.ContainsKey(Getters.File))
                {
                    return GetterArgsDict[Getters.File];
                }
                else
                {
                    // It seems that google for "localization of exception message" indicates that only ValidationException or BusinessLayerException should be localized
                    throw new NotSupportedException("This Getter has no GetterArgs for a Getters.File get");

                }
            }
            set
            {
                //ToDo: determine if validation of not null is needed
                GetterArgsDict[Getters.File] = value;
            }
        }
        public IGetterArgs GetterArgs {
            get {
                switch (_kind)
                {
                    case Getters.HTTPClient:
                        return GetterArgsHTTPClient.GetterArgs;
                    case Getters.File:
                        return GetterArgsFile.GetterArgs;
                    default:
                        throw new NotSupportedException("This Getter does not support getting a getter kind of {0}");
                }
            }
            set            {
                switch (_kind)
                {
                    case Getters.HTTPClient:
                        GetterArgsHTTPClient.GetterArgs = value;
                        break;
                    case Getters.File:
                        GetterArgsFile.GetterArgs = value;
                        break;
                    default:
                        throw new NotSupportedException("This Getter does not support setting a getter kind of {0}");
                }
            }
        }
        public Getter(Getters kind)
        {
            _kind = kind;
        }
    }

    // GetterBuilder interfaces and class
    public interface IGetterBuilderGetSetGetterArgs
    {
        IGetter GetSetGetterArgs(IGetterArgs getterArgs);
    }
    public interface IGetterBuilder
    {
        IGetter Build(Getters kind);
    }

    public class GetterBuilder: IGetterBuilder, IGetterBuilderGetSetGetterArgs
    {
        public GetterBuilder() { }
        public IGetter Build(Getters kind)
        {
            IGetter g;
            switch(kind)
            {
                case Getters.HTTPClient:
                    g = new Getter(Getters.HTTPClient);
                    break;
                case Getters.File:
                    g = new Getter(Getters.File);
                    break;
                default:
                    //ToDo add an exception message for unsupported enumerations
                    throw new NotSupportedException();
            }
            return g;
        }

        public IGetter Build(Getters kind,IGetterArgs getterArgs)
        {
            IGetter g;
            switch (kind)
            {
                case Getters.HTTPClient:
                    g = new Getter(Getters.HTTPClient);
                    break;
                case Getters.File:
                    g = new Getter(Getters.File);
                    break;
                default:
                    //ToDo add an exception message for unsupported enumerations
                    throw new NotSupportedException();
            }
            return g;
        }

        public IGetter GetSetGetterArgs(IGetterArgs getterArgs)
        {
            throw new NotImplementedException();
        }
    }

    //public class  getterdict1 {
    //    Tuple<Coins, IGetterArgs, Object> _dict;
    //    Tuple<Coins, IGetterArgs, Object> Dict { get { return _dict; } }
    //    public getterdict1() { _dict=new}
    //}


}

