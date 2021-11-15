using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NullFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right, MethodInfo method)
        {
            if (Nullable.GetUnderlyingType(left.Type) == null)
                return Expression.Equal(Expression.Convert(left, GetNullableType(left.Type)), Expression.Constant(null));
            else
                return Expression.Equal(left, Expression.Constant(null));
        }
    }
}
