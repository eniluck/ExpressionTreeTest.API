using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class LessThanFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.LessThan(memberExpression, Expression.Convert(constantExpression, GetNullableType(memberExpression.Type)));
            else
                return Expression.LessThan(memberExpression, constantExpression);
        }
    }
}
