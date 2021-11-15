using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotEqualsFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right, MethodInfo method)
        {
            if (Nullable.GetUnderlyingType(left.Type) != null)
                return Expression.NotEqual(left, Expression.Convert(right, GetNullableType(left.Type)));
            else
                return Expression.NotEqual(left, right);
        }
    }
}
