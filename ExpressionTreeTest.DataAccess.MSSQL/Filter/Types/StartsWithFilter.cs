using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class StartsWithFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression left, ConstantExpression right)
        {
            return Expression.Call(left, method, right);
        }
    }
}
