using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class ContainsFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right, MethodInfo method)
        {
            return Expression.Call(left, method, right);
        }
    }
}
