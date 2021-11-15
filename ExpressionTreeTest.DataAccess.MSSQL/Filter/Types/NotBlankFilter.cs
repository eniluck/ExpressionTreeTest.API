using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotBlankFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            return Expression.NotEqual(memberExpression, Expression.Constant(""));
        }
    }
}
