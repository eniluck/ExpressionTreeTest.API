using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    public interface IFilter
    {
        string GetName();
        Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression);
    }
}
