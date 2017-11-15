using System;
using System.ComponentModel;
using System.Globalization;

namespace ATAP.CryptoCurrency
{
    public class PowerConsumption
    {
        double powerConsumed;
        int powerConsumedUOM;
        public PowerConsumption()
        {
            this.powerConsumed = default(double);
            this.powerConsumedUOM = default(int);
        }
        public PowerConsumption(double powerConsumed, int powerConsumedUOM)
        {
            this.powerConsumed = powerConsumed;
            this.powerConsumedUOM = powerConsumedUOM;
        }
        public double PowerConsumed { get => powerConsumed; set => powerConsumed = value; }
        public int PowerConsumedUOM { get => powerConsumedUOM; set => powerConsumedUOM = value; }
        public override string ToString() { return $"{this.powerConsumed}:{this.powerConsumedUOM}"; }
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
                double pc;
                int uom;
                //ToDo better validation on string to be sure it conforms to  "double:integer"
                string[] s = ((string)value).Split(':');
                if (s.Length != 2 || !Double.TryParse(s[0], out pc) || !int.TryParse(s[1], out uom)) throw new ArgumentException("Object is not a string of format double:int", "value");
                return new PowerConsumption(pc,uom);
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
                    return " : ";
                }

                PowerConsumption powerConsumption = (PowerConsumption)value;
                return powerConsumption.ToString();

            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
    }



}
