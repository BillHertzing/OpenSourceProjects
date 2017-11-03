using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ATAP.CryptoCurrency
{
/*
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description{
            get {
                string displayName = _resource.GetString(_resourceKey);
                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }
    */
    public static class ExtensionHelpers
    {
        // The C# V6 way...
        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }

        public interface IAttribute<out T>
        {
            T Value { get; }
        }
        public sealed class Symbol : Attribute, IAttribute<string>
        {
            public string Value { get; }
            public Symbol(string value) { Value = value; }
            public static implicit operator string(Symbol v) { return v.Value; }
        }

        public sealed class Algorithm : Attribute, IAttribute<string>
        {
            public string Value { get; }
            public Algorithm(string value) { Value = value; }
            public static implicit operator string(Algorithm v) { return v.Value; }
        }

        public sealed class Proof : Attribute, IAttribute<string>
        {
            public string Value { get; }
            public Proof(string value) { Value = value; }
            public static implicit operator string(Proof v) { return v.Value; }
        }


        public static CustomAttributeType GetAttributeValue<CustomAttributeName, CustomAttributeType>(this Enum value)
        {
            var x =
   // The enumeration value passed as the parameter to the GetSymbol method call
   value
       // Get the the specific enumeration type
       .GetType()
       // Gets the FieldInfo object for this specific  value of the enumeration
       .GetField(value.ToString())
       // If the field info object is not null, get a custom attribute of type T from this specific value of the enumeration
       ?.GetCustomAttributes(typeof(CustomAttributeName), false)
       .FirstOrDefault();
            // If the result is not null, return it as CustomAttributeType, else return the default value for that CustomAttributeType
            if (x == null) { return default(CustomAttributeType); }
            IAttribute<CustomAttributeType> z = x as IAttribute<CustomAttributeType>;
            return z.Value;
        }
    }
}
