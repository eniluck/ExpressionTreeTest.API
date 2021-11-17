using System;

namespace ExpressionTreeTest.DataAccess.MSSQL.Converters
{
    public static class StringToTypeHelper
    {
        //https://stackoverflow.com/questions/3502493/is-there-any-generic-parse-function-that-will-convert-a-string-to-any-type-usi
        public static T ChangeType<T>(this string value)
        {
            try { 
                var result = (T)Convert.ChangeType(value, typeof(T));
                return result;
            } catch {
                throw new Exception($"Cannot cast {value} to {typeof(T)}.");
            }
        }
    }
}
