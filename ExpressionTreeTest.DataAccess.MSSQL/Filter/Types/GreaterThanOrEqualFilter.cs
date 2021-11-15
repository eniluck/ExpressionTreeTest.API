using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class GreaterThanOrEqualFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.GreaterThanOrEqual(memberExpression, Expression.Convert(constantExpression, GetNullableType(memberExpression.Type)));
            else
                return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }
    }
}
