using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    public interface IFilter
    {
        string GetName();
        Expression GetExpression<T>(MemberExpression memberExpression, EntityFilterParam<T> filter);
        ConstantExpression GetConstant(object filterValue);
    }
}
