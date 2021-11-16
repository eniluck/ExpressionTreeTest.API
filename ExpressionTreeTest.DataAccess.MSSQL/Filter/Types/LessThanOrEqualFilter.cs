using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class LessThanOrEqualFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.LessThanOrEqual(memberExpression, Expression.Convert(filter.FilterType.GetConstant(filter.FilterValue), GetNullableType(memberExpression.Type)));
            else
                return Expression.LessThanOrEqual(memberExpression, filter.FilterType.GetConstant(filter.FilterValue));
        }
    }
}
