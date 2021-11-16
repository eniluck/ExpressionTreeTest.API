using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    class NotStartsWithFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });

            return Expression.Not(
                Expression.Call(memberExpression, startsWithMethod, filter.FilterType.GetConstant(filter.FilterValue))
            );
        }
    }
}
