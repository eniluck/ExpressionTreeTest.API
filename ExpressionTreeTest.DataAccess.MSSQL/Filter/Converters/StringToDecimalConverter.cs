using System;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Converters
{
    class StringToDecimalConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            return value.ChangeType<Decimal>();
        }

        /*
         if (baseTypeString == "System.Decimal") {
                decimal value;
                var result = decimal.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be decimal. But was: {fieldValue}.");
                return value;
            }
         */
    }
}
