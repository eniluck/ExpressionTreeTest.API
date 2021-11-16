using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotBlankFilter : FilterBase, IFilter
    {
        public Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter)
        {
            return Expression.NotEqual(memberExpression, Expression.Constant(""));
        }
    }
}
