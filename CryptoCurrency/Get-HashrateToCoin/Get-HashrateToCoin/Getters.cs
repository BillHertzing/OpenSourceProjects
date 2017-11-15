using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
namespace ATAP.CryptoCurrency
{
    public enum Getters
    {
        [Description("HTTPClient")]
        HTTPClient,
        [Description("File")]
        File
    }
}

    //public interface IFileGetterArgs : IGetterArgs {
    //    string Path { get; }
    //}
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




 //   public class Getter : IGetter, IGetterArgs {
 //       Getters _kind;
 //       public Getter(Getters kind)
 //       {
 //           _kind = kind;
 //       }
 //       Dictionary<Getters, IGetterArgs> GetterArgsDict { get; set; }
 //       public IGetterArgs GetterArgs { get { switch(_kind) { case Getters.HTTPClient: return GetterArgsHTTPClient.GetterArgs; case Getters.File: return GetterArgsFile.GetterArgs; default: throw new NotSupportedException("This Getter does not support getting a getter kind of {0}"); } }
 //           set { switch(_kind) { case Getters.HTTPClient: GetterArgsHTTPClient.GetterArgs = value; break; case Getters.File: GetterArgsFile.GetterArgs = value; break; default: throw new NotSupportedException("This Getter does not support setting a getter kind of {0}"); } } }
 //       public IGetterArgs GetterArgsFile { get { if(GetterArgsDict.ContainsKey(Getters.File)) { return GetterArgsDict[Getters.File]; }
 //               else
 //               {
 //                   // It seems that google for "localization of exception message" indicates that only ValidationException or BusinessLayerException should be localized
 //                   throw new NotSupportedException("This Getter has no GetterArgs for a Getters.File get");

 //               } }
 //           set {
 //               //ToDo: determine if validation of not null is needed
 //GetterArgsDict[Getters.File] = value; } }
 //       public IGetterArgs GetterArgsHTTPClient { get { if(GetterArgsDict.ContainsKey(Getters.HTTPClient)) { return GetterArgsDict[Getters.HTTPClient]; }
 //               else
 //               {
 //                   throw new NotSupportedException("This Getter has no GetterArgs for a Getters.HTTPClient get");

 //               } }
 //           set {
 //               //ToDo: determine if validation of not null is needed
 //GetterArgsDict[Getters.HTTPClient] = value; } }
 //       public Getters Kind { get { return _kind; } }
 //   }
    // GetterBuilder interfaces and class
    //public interface IGetterBuilderGetSetGetterArgs {
    //    IGetter GetSetGetterArgs(IGetterArgs getterArgs);
    //}
    //public interface IGetterBuilder {
    //    IGetter Build(Getters kind);
    //}
    //public class GetterBuilder : IGetterBuilder, IGetterBuilderGetSetGetterArgs {
    //    public GetterBuilder() { }
    //    public IGetter Build(Getters kind)
    //    {
    //        IGetter g;
    //        switch(kind) { case Getters.HTTPClient:
    //                g = new Getter(Getters.HTTPClient);
    //                break;
    //            case Getters.File:
    //                g = new Getter(Getters.File);
    //                break;
    //            default:
    //                //ToDo add an exception message for unsupported enumerations
    //                throw new NotSupportedException(); }
    //        return g;
    //    }
    //    public IGetter Build(Getters kind, IGetterArgs getterArgs)
    //    {
    //        IGetter g;
    //        switch(kind) { case Getters.HTTPClient:
    //                g = new Getter(Getters.HTTPClient);
    //                break;
    //            case Getters.File:
    //                g = new Getter(Getters.File);
    //                break;
    //            default:
    //                //ToDo add an exception message for unsupported enumerations
    //                throw new NotSupportedException(); }
    //        return g;
    //    }
    //    public IGetter GetSetGetterArgs(IGetterArgs getterArgs)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


