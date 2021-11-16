using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotEndsWithFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            var endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            return Expression.Not(
                Expression.Call(memberExpression, endsWithMethod, filter.FilterType.GetConstant(filter.FilterValue))
            );
        }
    }
}
