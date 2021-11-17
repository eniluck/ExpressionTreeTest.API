namespace ExpressionTreeTest.DataAccess.MSSQL.Converters
{
    public class StringToStringConverter : IStringToTypeConverter
    {
        public object Convert(string value)
        {
            return value;
        }
    }
}
