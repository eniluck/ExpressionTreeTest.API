using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    public interface IFilter
    {
        string GetName();
        Expression GetExpression(MemberExpression left, ConstantExpression right);
    }
}
