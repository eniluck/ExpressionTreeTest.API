using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotContainsFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            return Expression.Not(Expression.Call(memberExpression, containsMethod, constantExpression));
        }
    }
}
