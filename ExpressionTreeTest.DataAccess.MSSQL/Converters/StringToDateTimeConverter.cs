using System;
using System.Globalization;

namespace ExpressionTreeTest.DataAccess.MSSQL.Converters
{
    public class StringToDateTimeConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            DateTime convertedValue;
            var result = DateTime.TryParseExact(value,
                                           "dd'.'MM'.'yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out convertedValue);
            if (result == false)
                throw new Exception($"Cannot cast {value} to {typeof(DateTime)}.");

            return convertedValue;
        }
    }
}
