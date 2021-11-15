using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class LessThanOrEqualFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.LessThanOrEqual(memberExpression, Expression.Convert(constantExpression, GetNullableType(memberExpression.Type)));
            else
                return Expression.LessThanOrEqual(memberExpression, constantExpression);
        }
    }
}
