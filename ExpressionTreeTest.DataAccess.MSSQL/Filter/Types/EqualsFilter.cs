using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class EqualsFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.Equal(memberExpression, Expression.Convert(filter.FilterType.GetConstant(filter.FilterValue), GetNullableType(memberExpression.Type)));
            else
                return Expression.Equal(memberExpression, filter.FilterType.GetConstant(filter.FilterValue));
        }
    }
}
