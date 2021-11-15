using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotEndsWithFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            return Expression.Not(
                Expression.Call(memberExpression, endsWithMethod, constantExpression)
            );
        }
    }
}
