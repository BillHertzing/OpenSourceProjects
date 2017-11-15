using System;
using System.ComponentModel;
using System.Globalization;
namespace ATAP.CryptoCurrency {
    public class Fees {
        double feeAsAPercent;
        public Fees()
        {
            feeAsAPercent = default(double);
        }
        public Fees(double feeAsAPercent)
        {
            this.feeAsAPercent = feeAsAPercent;
        }
        public override string ToString()
        {
            return $"{FeeAsAPercent}";
        }
        public double FeeAsAPercent { get => feeAsAPercent; set => feeAsAPercent = value; }
    }
    public class FeesConverter : ExpandableObjectConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if(destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext
            context, CultureInfo culture, object value)
        {
            if(value == null)
            {
                return new Fees();
            }

            if(value is string)
            {
                double d;
                if(!double.TryParse(value as string, out d))
                {
                    throw new ArgumentException("Object is not a string of format double", "value");
                }

                return new Fees(d);
            }

            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(value != null)
            {
                if(!(value is Fees))
                {
                    throw new ArgumentException("Invalid object, is not a Fees", "value");
                }
            }

            if(destinationType == typeof(string))
            {
                if(value == null)
                {
                    return ((value as Fees).FeeAsAPercent).ToString();
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
