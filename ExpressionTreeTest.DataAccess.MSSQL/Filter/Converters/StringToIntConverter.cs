using System;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Converters
{
    public class StringToIntConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            return value.ChangeType<int>();
        }
        /*
         * if (
                (baseTypeString == "System.Int32") ||
                (baseTypeString == "System.Int16")) {
                int value;
                var result = int.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be integer. But was: {fieldValue}.");
                return value;
            }
         */
    }
}
