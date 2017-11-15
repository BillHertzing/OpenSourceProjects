using System;
using System.ComponentModel;
using System.Globalization;

namespace ATAP.CryptoCurrency

    //ToDO: Either delete this, or eventually, the dictionary key may become this type, but for now, the dictionary will use a string key
{
    [TypeConverter(typeof(MinerConfigIDTConverter))]
        public class MinerConfigIDT
    {
        string iD;
        public MinerConfigIDT(string id)
        {
            iD = id;
        }
        public MinerConfigIDT()
        {
            iD = default(string);
        }
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public override string ToString() { return iD; }
    }
    public class MinerConfigIDTConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(
            ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext
            context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return new MinerConfigIDT();
            }

            if (value is string)
            {

                return new MinerConfigIDT((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is MinerConfigIDT))
                {
                    throw new ArgumentException(
                        "Invalid MinerConfigIDT", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return default(string);
                }

                MinerConfigIDT MinerConfigIDT = (MinerConfigIDT)value;
                return MinerConfigIDT.ToString();

            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
    }
}
