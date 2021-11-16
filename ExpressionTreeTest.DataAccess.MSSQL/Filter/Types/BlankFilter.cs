using ExpressionTreeTest.DataAccess.MSSQL.Models;
using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class BlankFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            return Expression.Equal(memberExpression, Expression.Constant(""));
        }
    }
}
