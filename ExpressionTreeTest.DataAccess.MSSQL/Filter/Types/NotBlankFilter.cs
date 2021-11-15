using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotBlankFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right, MethodInfo method)
        {
            return Expression.NotEqual(left, Expression.Constant(""));
        }
    }
}
