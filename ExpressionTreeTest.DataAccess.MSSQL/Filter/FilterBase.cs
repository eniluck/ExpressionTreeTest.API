using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    public class FilterBase
    {
        /// <summary>
        /// Конвертирует тип в nullable версию этого типа.
        /// https://stackoverflow.com/questions/108104/how-do-i-convert-a-system-type-to-its-nullable-version
        /// </summary>
        /// <param name="type">Тип для конвертации.</param>
        /// <returns>Nullable тип.</returns>
        public Type GetNullableType(Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            type = Nullable.GetUnderlyingType(type) ?? type; // avoid type becoming null
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }

        public string GetName()
        {
            return this.GetType().Name.Replace("Filter", "");
        }

        public ConstantExpression GetConstant(object filterValue)
        {
            return Expression.Constant(filterValue);
        }
    }
}
