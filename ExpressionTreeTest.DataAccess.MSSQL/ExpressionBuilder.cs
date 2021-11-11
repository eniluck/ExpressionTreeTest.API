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
            
            ConstantExpression filterConstant= null;

            Type propertyType = GetUnderlyingPropertyType<T>(filter.FieldName);
            if ( (
                filter.FilterType == FilterType.Null ||
                filter.FilterType == FilterType.NotNull) ) 
            {
                // var convertedFilterContant = Expression.Convert(filterConstant, propertyType);
                //filterConstant = convertedFilterContant;
                //nullConstant = Expression.Constant(null, member.Type);

                // cast member as nullable <member>
                //member = (MemberExpression)Expression.Convert(member, typeof(int?));
            } else {
                filterConstant = GetConstantExpression<T>(filter);
            }
            
            ConstantExpression blankStringConstant = Expression.Constant("");

            // Проверить что данное свойство можно фильтровать данным типом 
            if (CheckTypeByFieldType<T>(filter) == false)
                throw new Exception("Filter type must be supported by property value.");
            
            switch (filter.FilterType) {
                case FilterType.Null:
                    if (Nullable.GetUnderlyingType(member.Type) == null) { 
                        return Expression.Equal(Expression.Convert(member, GetNullableType(member.Type)), Expression.Constant(null));
                    }
                    else
                        return Expression.Equal(member, Expression.Constant(null));
                case FilterType.NotNull:
                    if (Nullable.GetUnderlyingType(member.Type) == null) {
                        return Expression.NotEqual(Expression.Convert(member, GetNullableType(member.Type)), Expression.Constant(null));
                    }
                    else
                        return Expression.NotEqual(member, Expression.Constant(null));
                case FilterType.Equals:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.Equal(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.Equal(member, filterConstant);
                case FilterType.NotEquals:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.NotEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.NotEqual(member, filterConstant);
                    
                case FilterType.Blank:
                    return Expression.Equal(member, blankStringConstant);
                case FilterType.NotBlank:
                    return Expression.NotEqual(member, blankStringConstant);
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
                case FilterType.NotEndsWith:
                    return Expression.Not(
                            Expression.Call(member, endsWithMethod, filterConstant));
                case FilterType.GreaterThan:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.GreaterThan(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.GreaterThan(member, filterConstant);
                case FilterType.GreaterThanOrEqual:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.GreaterThanOrEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.GreaterThanOrEqual(member, filterConstant);
                case FilterType.LessThan:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.LessThan(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.LessThan(member, filterConstant);
                case FilterType.LessThanOrEqual:
                    if (Nullable.GetUnderlyingType(member.Type) != null) {
                        return Expression.LessThanOrEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    }
                    else
                        return Expression.LessThanOrEqual(member, filterConstant);
            }
            return null;
        }

        // https://stackoverflow.com/questions/108104/how-do-i-convert-a-system-type-to-its-nullable-version
        private Type GetNullableType(Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            type = Nullable.GetUnderlyingType(type) ?? type; // avoid type becoming null
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }


        private ConstantExpression GetConstantExpression<T>(FilterParam filter)
        {
            var typeString = GetUnderlyingPropertyType<T>(filter.FieldName).ToString();
            var typeToConvert = GetPropertyType<T>(filter.FieldName);

            if (typeString == "System.String")
                return Expression.Constant(filter.FieldValue);

            if (typeString == "System.DateTime") 
            {
                DateTime value;
                var result = DateTime.TryParse(filter.FieldValue,out value);
                if (result == false)
                    throw new Exception($"Value must be datetime. But was: {filter.FieldValue}");
               
                return Expression.Constant(value);
            }
            if (typeString == "System.Decimal")
            {
                decimal value;
                var result = decimal.TryParse(filter.FieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be decimal. But was: {filter.FieldValue}");
                return Expression.Constant(value);
            }

            if (
                (typeString == "System.Int32") ||
                (typeString == "System.Int16")) 
            {
                int value;
                var result = int.TryParse(filter.FieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be integer. But was: {filter.FieldValue}");
                return Expression.Constant(value);
            }

            return null;
        }

        /// <summary>
        /// Проверить что данное свойство можно фильтровать данным типом.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="filter">Наименование поля.</param>
        /// <returns>Результат.</returns>
        public bool CheckTypeByFieldType<T>(FilterParam filter)
        {
            var typeString = GetUnderlyingPropertyType<T>(filter.FieldName).ToString();
            var filterType = filter.FilterType;

            if (typeString == "System.String") 
            {
                StringFilterType value = (StringFilterType)filterType;
                return Enum.IsDefined(typeof(StringFilterType), value);
            }

            if (typeString == "System.DateTime") 
            {
                DateFilterType value = (DateFilterType)filterType;
                return Enum.IsDefined(typeof(DateFilterType), value);
            }

            if ((typeString == "System.Decimal") ||
                (typeString == "System.Int32") ||
                (typeString == "System.Int16")) 
            {
                NumberFilterType value = (NumberFilterType)filterType;
                return Enum.IsDefined(typeof(NumberFilterType), value);
            }

            return false;
        }

        /// <summary>
        /// Проверить наличие свойства в указанном типе.
        /// </summary>
        /// <typeparam name="T">Параметр типа в котором будет происходить поиск свойства.</typeparam>
        /// <param name="propertyName">Имя свойства для поиска в выбранном типе.</param>
        /// <returns>Результат проверки наличия свойства в указанном типе.</returns>
        public bool CheckPropertyNameIsExisted<T>(string propertyName)
        {
            return typeof(T).GetProperties().Any(p => p.Name == propertyName);
        }

        public Type GetPropertyType<T>(string propertyName)
        {
            Type registryObjectType = typeof(T);

            var registryProperty = registryObjectType.GetProperties().Where(p => p.Name == propertyName).FirstOrDefault();

            // https://stackoverflow.com/questions/8550209/c-sharp-reflection-how-to-get-the-type-of-a-nullableint

            return registryProperty.PropertyType;
        }

        public Type GetUnderlyingPropertyType<T>(string propertyName)
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

            return propertyType;
        }
    }
}
