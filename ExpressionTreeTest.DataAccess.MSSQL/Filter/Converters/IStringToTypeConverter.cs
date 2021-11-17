using System;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Converters
{
    interface IStringToTypeConverter
    {
        object Convert(string value);
    }
}
