using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class LessThanFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.LessThan(memberExpression, Expression.Convert(filter.FilterType.GetConstant(filter.FilterValue), GetNullableType(memberExpression.Type)));
            else
                return Expression.LessThan(memberExpression, filter.FilterType.GetConstant(filter.FilterValue));
        }
    }
}
