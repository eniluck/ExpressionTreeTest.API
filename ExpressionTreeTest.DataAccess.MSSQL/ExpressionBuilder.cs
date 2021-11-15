using ExpressionTreeTest.DataAccess.MSSQL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL
{
    public class ExpressionBuilder
    {
        private readonly MethodInfo _containsMethod;
        private readonly MethodInfo _startsWithMethod;
        private readonly MethodInfo _endsWithMethod;

        private readonly ReversePolishNotation _rpn;

        public ExpressionBuilder()
        {
            _rpn = new ReversePolishNotation();
            _containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            _startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            _endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        }

        public IOrderedQueryable<T> GetOrderedEntities<T>(IQueryable<T> entities, List<OrderParam> orderParams)
        {
            if (orderParams == null || orderParams.Count == 0)
                return null;

            IOrderedQueryable<T> orderedEntities = null;

            if (orderParams[0].Order == OrderType.Asc)
                orderedEntities = entities.OrderBy(p => EF.Property<string>(p, orderParams[0].FieldName));
            else //if (orderParams[0].Order == "desc")
                orderedEntities = entities.OrderByDescending(p => EF.Property<string>(p, orderParams[0].FieldName));

            for (int i = 1; i < orderParams.ToArray().Length; i++) {
                //orderedPhones = orderedPhones.OrderBy(p => EF.Property<string>(p, orderParams[i].FieldName));

                if (orderParams[0].Order == OrderType.Asc)
                    orderedEntities = orderedEntities.OrderBy(p => EF.Property<string>(p, orderParams[i].FieldName));
                else //if (orderParams[0].Order == "desc")
                    orderedEntities = orderedEntities.OrderByDescending(p => EF.Property<string>(p, orderParams[i].FieldName));
            }

            return orderedEntities;
        }

        /// <summary>
        /// Получить отфильтрованные результаты.
        /// </summary>
        /// <typeparam name="T">Тип сущности для фильтрации.</typeparam>
        /// <param name="entities">Набор значений для фильтрации.</param>
        /// <param name="filterParams">Параметры фильтрации.</param>
        /// <param name="filterRule">Правило фильтрации.</param>
        /// <returns></returns>
        public IQueryable<T> GetFilteredEntities<T>(IQueryable<T> entities, List<FilterParam> filterParams, string filterRule)
        {
            var predicate = GetWherePredicate<T>(filterParams, filterRule);

            var filteredEntities = entities.Where(predicate);

            return filteredEntities;
        }

        /// <summary>
        /// Получить предикат для операции where.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="filterParams">Параметры фильтрации.</param>
        /// <param name="filterRule">Правило фильтрации.</param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetWherePredicate<T>(List<FilterParam> filterParams, string filterRule)
        {
            if (filterParams == null || filterParams.Count == 0)
                return null;

            ParameterExpression expParam = Expression.Parameter(typeof(T), "p");

            string rpnStringRule = _rpn.GetRpnStringRule(filterRule);

            Expression exp = _rpn.FormWherePredicate<T>(rpnStringRule, filterParams, expParam);

            return Expression.Lambda<Func<T, bool>>(exp, expParam);
        }

        /// <summary>
        /// Создать выражение из параметра по фильтру.
        /// </summary>
        /// <typeparam name="T">Параметр типа.</typeparam>
        /// <param name="expParam">Параметр для дерева выражений.</param>
        /// <param name="filter">Параметр фильтрации.</param>
        /// <returns>Выражение.</returns>
        public Expression GetExpression<T>(ParameterExpression expParam, FilterParam filter)
        {
            MemberExpression member = Expression.Property(expParam, filter.FieldName);
            
            ConstantExpression filterConstant= null;

            //Type propertyType = GetUnderlyingPropertyType<T>(filter.FieldName);
            if (filter.FilterType == FilterType.Null ||
                filter.FilterType == FilterType.NotNull) 
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
            if (CheckFilterByFieldType<T>(filter) == false)
                throw new Exception("Filter type must be supported by property value.");
            
            switch (filter.FilterType) {
                case FilterType.Null:
                    if (Nullable.GetUnderlyingType(member.Type) == null) 
                        return Expression.Equal(Expression.Convert(member, GetNullableType(member.Type)), Expression.Constant(null));
                    else
                        return Expression.Equal(member, Expression.Constant(null));
                case FilterType.NotNull:
                    if (Nullable.GetUnderlyingType(member.Type) == null) 
                        return Expression.NotEqual(Expression.Convert(member, GetNullableType(member.Type)), Expression.Constant(null));
                    else
                        return Expression.NotEqual(member, Expression.Constant(null));
                case FilterType.Equals:
                    if (Nullable.GetUnderlyingType(member.Type) != null) 
                        return Expression.Equal(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.Equal(member, filterConstant);
                case FilterType.NotEquals:
                    if (Nullable.GetUnderlyingType(member.Type) != null) 
                        return Expression.NotEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.NotEqual(member, filterConstant);
                case FilterType.Blank:
                    return Expression.Equal(member, blankStringConstant);
                case FilterType.NotBlank:
                    return Expression.NotEqual(member, blankStringConstant);
                case FilterType.Contains:
                    return Expression.Call(member, _containsMethod, filterConstant);
                case FilterType.NotContains:
                    return Expression.Not(
                                Expression.Call(member, _containsMethod, filterConstant));
                case FilterType.StartsWith:
                    return Expression.Call(member, _startsWithMethod, filterConstant);
                case FilterType.NotStartWith:
                    return Expression.Not(
                                Expression.Call(member, _startsWithMethod, filterConstant));
                case FilterType.EndsWith:
                    return Expression.Call(member, _endsWithMethod, filterConstant);
                case FilterType.NotEndsWith:
                    return Expression.Not(
                            Expression.Call(member, _endsWithMethod, filterConstant));
                case FilterType.GreaterThan:
                    if (Nullable.GetUnderlyingType(member.Type) != null)
                        return Expression.GreaterThan(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.GreaterThan(member, filterConstant);
                case FilterType.GreaterThanOrEqual:
                    if (Nullable.GetUnderlyingType(member.Type) != null)
                        return Expression.GreaterThanOrEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.GreaterThanOrEqual(member, filterConstant);
                case FilterType.LessThan:
                    if (Nullable.GetUnderlyingType(member.Type) != null)
                        return Expression.LessThan(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.LessThan(member, filterConstant);
                case FilterType.LessThanOrEqual:
                    if (Nullable.GetUnderlyingType(member.Type) != null)
                        return Expression.LessThanOrEqual(member, Expression.Convert(filterConstant, GetNullableType(member.Type)));
                    else
                        return Expression.LessThanOrEqual(member, filterConstant);
            }
            return null;
        }

        /// <summary>
        /// Конвертирует тип в nullable версию этого типа.
        /// https://stackoverflow.com/questions/108104/how-do-i-convert-a-system-type-to-its-nullable-version
        /// </summary>
        /// <param name="type">Тип для конвертации.</param>
        /// <returns>Nullable тип.</returns>
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
        public bool CheckFilterByFieldType<T>(FilterParam filter)
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
