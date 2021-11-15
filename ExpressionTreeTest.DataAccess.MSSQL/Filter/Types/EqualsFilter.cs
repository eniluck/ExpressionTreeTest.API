using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class EqualsFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression constantExpression)
        {
            if (Nullable.GetUnderlyingType(left.Type) != null)
                return Expression.Equal(left, Expression.Convert(constantExpression, GetNullableType(left.Type)));
            else
                return Expression.Equal(left, constantExpression);
        }
    }
}
