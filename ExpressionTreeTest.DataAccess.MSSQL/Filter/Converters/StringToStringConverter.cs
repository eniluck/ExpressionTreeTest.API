namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Converters
{
    public class StringToStringConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            return value;
        }
    }
}
