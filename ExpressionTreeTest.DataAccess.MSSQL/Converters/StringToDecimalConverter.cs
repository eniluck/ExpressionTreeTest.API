using System;
using System.Globalization;

namespace ExpressionTreeTest.DataAccess.MSSQL.Converters
{
    public class StringToDecimalConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            decimal convertedValue;

            NumberStyles style = NumberStyles.AllowDecimalPoint;
            CultureInfo culture = CultureInfo.InvariantCulture;

            var result = decimal.TryParse(value.Replace(',', '.'), style, culture, out convertedValue);
            if (result == false)
                throw new Exception($"Cannot cast {value} to {typeof(decimal)}.");

            return convertedValue;
        }

        //return value.ChangeType<decimal>();
    }
}
