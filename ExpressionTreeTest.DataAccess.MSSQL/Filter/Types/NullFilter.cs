using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NullFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) == null)
                return Expression.Equal(Expression.Convert(memberExpression, GetNullableType(memberExpression.Type)), Expression.Constant(null));
            else
                return Expression.Equal(memberExpression, Expression.Constant(null));
        }
    }
}
