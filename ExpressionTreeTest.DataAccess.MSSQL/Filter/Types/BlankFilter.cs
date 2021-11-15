using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class BlankFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            return Expression.Equal(memberExpression, Expression.Constant(""));
        }
    }
}
