using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotNullFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) == null)
                return Expression.NotEqual(Expression.Convert(memberExpression, GetNullableType(memberExpression.Type)), Expression.Constant(null));
            else
                return Expression.NotEqual(memberExpression, Expression.Constant(null));
        }
    }
}
