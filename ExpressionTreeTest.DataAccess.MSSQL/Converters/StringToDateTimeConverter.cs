using System;

namespace ExpressionTreeTest.DataAccess.MSSQL.Converters
{
    public class StringToDateTimeConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            return value.ChangeType<DateTime>();
        }

        /*
         if (baseTypeString == "System.DateTime") {
                DateTime value;
                var result = DateTime.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be datetime. But was: {fieldValue}.");

                return value;
            }
         */
    }
}
