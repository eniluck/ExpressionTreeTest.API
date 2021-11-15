using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class NotNullFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right, MethodInfo method)
        {
            if (Nullable.GetUnderlyingType(left.Type) == null)
                return Expression.NotEqual(Expression.Convert(left, GetNullableType(left.Type)), Expression.Constant(null));
            else
                return Expression.NotEqual(left, Expression.Constant(null));
        }
    }
}
