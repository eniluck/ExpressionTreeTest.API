using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotEqualsFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            if (Nullable.GetUnderlyingType(memberExpression.Type) != null)
                return Expression.NotEqual(memberExpression, Expression.Convert(filter.FilterType.GetConstant(filter.FilterValue), GetNullableType(memberExpression.Type)));
            else
                return Expression.NotEqual(memberExpression, filter.FilterType.GetConstant(filter.FilterValue));
        }
    }
}
