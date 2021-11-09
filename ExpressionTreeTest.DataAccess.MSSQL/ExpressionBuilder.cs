using ExpressionTreeTest.DataAccess.MSSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL
{
    public class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        public Expression<Func<T, bool>> GetPredicate<T>(List<FilterParam> filterParams, string filterCondition)
        {
            if (filterParams == null || filterParams.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "p");

            ReversePolishNotation rpn = new ReversePolishNotation();
            string rpnString = rpn.Get(filterCondition);

            Expression exp = rpn.FormPredicate<T>(rpnString, filterParams, param);

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public Expression GetExpression<T>(ParameterExpression param, FilterParam filter)
        {
            MemberExpression member = Expression.Property(param, filter.FieldName);
            ConstantExpression filterConstant = Expression.Constant(filter.FieldValue);
            ConstantExpression nullConstant = Expression.Constant(null);
            ConstantExpression blankStringConstant = Expression.Constant("");

            // Проверить что данное свойство можно фильтровать данным типом 
            if (CheckTypeByFieldType<T>(filter) == false)
                throw new Exception("Filter type must be supported by property value.");
            
            switch (filter.FilterType) {
                case FilterType.Equals:
                    return Expression.Equal(member, filterConstant);
                case FilterType.NotEquals:
                    return Expression.NotEqual(member, filterConstant);
                case FilterType.Blank:
                    return Expression.Equal(member, blankStringConstant);
                case FilterType.NotBlank:
                    return Expression.NotEqual(member, blankStringConstant);
                case FilterType.Null:
                    return Expression.Equal(member, nullConstant);
                case FilterType.NotNull:
                    return Expression.NotEqual(member, nullConstant);
                case FilterType.Contains:
                    return Expression.Call(member, containsMethod, filterConstant);
                case FilterType.NotContains:
                    return Expression.Not(
                                Expression.Call(member, containsMethod, filterConstant));
                case FilterType.StartsWith:
                    return Expression.Call(member, startsWithMethod, filterConstant);
                case FilterType.NotStartWith:
                    return Expression.Not(
                                Expression.Call(member, startsWithMethod, filterConstant));
                case FilterType.EndsWith:
                    return Expression.Call(member, endsWithMethod, filterConstant);
                case FilterType.NotEndWith:
                    return Expression.Not(
                            Expression.Call(member, endsWithMethod, filterConstant));
                case FilterType.GreaterThan:
                    return Expression.GreaterThan(member, filterConstant);
                case FilterType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, filterConstant);
                case FilterType.LessThan:
                    return Expression.LessThan(member, filterConstant);
                case FilterType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, filterConstant);
            }
            return null;
        }


        public bool CheckTypeByFieldType<T>(FilterParam filter)
        {
            var type = GetPropertyType<T>(filter.FieldName);
            var filterType = filter.FilterType;

            if (type == "System.String") 
            {
                return Enum.IsDefined(typeof(StringFilterType), filterType);
                
            }

            if (type == "System.DateTime") 
            {
                return Enum.IsDefined(typeof(DateFilterType), filterType);
            }

            if (
                (type == "System.Decimal") ||
                (type == "System.Int32") ||
                (type == "System.Int16")
            ) 
            {
                return Enum.IsDefined(typeof(NumberFilterType), filterType);
            }

            return false;
        }

        public bool CheckPropertyNameIsExisted<T>(string propertyName)
        {
            return typeof(T).GetProperties().Any(p => p.Name == propertyName);
        }

        private string GetPropertyType<T>(string propertyName)
        {
            Type registryObjectType = typeof(T);

            var registryProperty = registryObjectType.GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();

            // https://stackoverflow.com/questions/8550209/c-sharp-reflection-how-to-get-the-type-of-a-nullableint
            Type propertyType;

            var type = registryProperty.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                propertyType = Nullable.GetUnderlyingType(type);
            }
            else {
                propertyType = registryProperty.PropertyType;
            }

            return propertyType.ToString();
        }
    }
}
