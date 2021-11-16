using ExpressionTreeTest.DataAccess.MSSQL.Models;
using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class ContainsFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

            return Expression.Call(memberExpression, containsMethod, filter.FilterType.GetConstant(filter.FilterValue));
        }
    }
}
