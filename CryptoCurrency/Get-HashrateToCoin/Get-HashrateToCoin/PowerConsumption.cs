using System;
using System.ComponentModel;
using System.Globalization;
using UnitsNet;

namespace ATAP.CryptoCurrency
{
    public class PowerConsumption
    {
        double watts;
        TimeSpan period;
        public PowerConsumption()
        {
            this.watts = default(double);
            this.period = default(TimeSpan);
        }
        public PowerConsumption(double w, TimeSpan period)
        {
            this.watts = w;
            this.period = period;
        }
        public double Watts { get => watts; set => watts = value; }
        public TimeSpan Period { get => period; set => period = value; }
        public override string ToString() { return $"{this.watts}-{this.period}"; }
    }

    public class PowerConsumptionConverter : ExpandableObjectConverter
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
                return new PowerConsumption();
            }

            if (value is string)
            {
                double w;
                TimeSpan period;
                //ToDo better validation on string to be sure it conforms to  "double:TimeSpan"
                string[] s = ((string)value).Split('-');
                if (s.Length != 2 || !double.TryParse(s[0], out w) || !TimeSpan.TryParse(s[1], out period)) throw new ArgumentException("Object is not a string of format double:int", "value");
                return new PowerConsumption(w,period);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is PowerConsumption))
                {
                    throw new ArgumentException( "Invalid object, is not a PowerConsumption", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return "";
                }

                PowerConsumption powerConsumption = (PowerConsumption)value;
                return powerConsumption.ToString();

            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
    }



}
