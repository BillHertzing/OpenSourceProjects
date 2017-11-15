using System;
using System.ComponentModel;
using System.Globalization;

namespace ATAP.CryptoCurrency
{
    [TypeConverter(typeof(MinerRigIDTConverter))]
    public class MinerRigIDT
    {
        string iD;
        public MinerRigIDT(string id)
        {
            iD = id;
        }
        public MinerRigIDT()
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
    public class MinerRigIDTConverter : ExpandableObjectConverter
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
                return new MinerRigIDT();
            }

            if (value is string)
            {

                    return new MinerRigIDT((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is MinerRigIDT))
                {
                    throw new ArgumentException(
                        "Invalid MinerRigIDT", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return default(string);
                }

                MinerRigIDT miningRigIDT = (MinerRigIDT)value;
                return miningRigIDT.ToString();

            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
    }
}
