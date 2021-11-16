using ExpressionTreeTest.DataAccess.MSSQL.Filter.Types;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    public class EntityFilterParamBuilder<T>
    {
        public EntityFilterParam<T> BuildByFilterParam( FilterParam filterParam)
        {
            EntityFilterParam<T> entityFilterParam = new EntityFilterParam<T>();
            entityFilterParam.EntityType = typeof(T);
            entityFilterParam.Property = GetProperty(filterParam.FieldName);
            entityFilterParam.FilterType = GetFilter(filterParam.FilterType);

            if ( !(entityFilterParam.FilterType is NullFilter) || !(entityFilterParam.FilterType is NotNullFilter))
                entityFilterParam.FilterValue = GetValue(filterParam.FieldValue, entityFilterParam.Property.PropertyType);

            return entityFilterParam;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetValue(string fieldValue, Type type)
        {
            //TODO: Переписать.
            var baseTypeString = type.ToString();

            if (baseTypeString == "System.String")
                return fieldValue;

            if (baseTypeString == "System.DateTime") {
                DateTime value;
                var result = DateTime.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be datetime. But was: {fieldValue}.");

                return value;
            }

            if (baseTypeString == "System.Decimal") {
                decimal value;
                var result = decimal.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be decimal. But was: {fieldValue}.");
                return value;
            }

            if (
                (baseTypeString == "System.Int32") ||
                (baseTypeString == "System.Int16")) {
                int value;
                var result = int.TryParse(fieldValue, out value);
                if (result == false)
                    throw new Exception($"Value must be integer. But was: {fieldValue}.");
                return value;
            }

            throw new Exception($"Not known type: {baseTypeString}.");
        }

        /// <summary>
        /// Получить базовый тип.
        /// Если тип Nullable будет получен базовый тип.
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="propertyName">Имя для поиска.</param>
        /// <returns></returns>
        private Type GetBaseType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                return Nullable.GetUnderlyingType(type);
            }
            
            return type;
        }

        Dictionary<string, IFilter> registeredFilters = new Dictionary<string, IFilter>() {
            {"blank", new BlankFilter()},
            {"!blank", new NotBlankFilter()},
            {"contains", new ContainsFilter()},
            {"!contains", new NotContainsFilter()},
            {"ends", new EndsWithFilter()},
            {"!ends", new NotEndsWithFilter()},
            {"equals", new EqualsFilter()},
            {"!equals", new NotEqualsFilter()},
            {">", new GreaterThanFilter()},
            {">=", new GreaterThanOrEqualFilter()},
            {"<", new LessThanFilter()},
            {"<=", new LessThanOrEqualFilter()},
            {"null", new NullFilter()},
            {"!null", new NotNullFilter()},
            {"starts", new StartsWithFilter()},
            {"!starts", new NotStartsWithFilter()},
        };

        /// <summary>
        /// Получить класс фильтра по строке.
        /// </summary>
        /// <param name="filterType"></param>
        /// <returns></returns>
        private IFilter GetFilter(string filterType)
        {
            IFilter filter;
            var result = registeredFilters.TryGetValue(filterType, out filter);
            if (result == false)
                throw new Exception($"Filter type '{filterType}' not found in registered filters.");

            return filter;
        }

        /// <summary>
        /// Получить информацию о свойстве по имени в данной сущности.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Информация о свойстве.</returns>
        private PropertyInfo GetProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new Exception("Property name cannot be null");

            Type entityType = typeof(T);

            PropertyInfo propertyInfo = entityType
                .GetProperties()
                .Where(p => p.Name == propertyName)
                .FirstOrDefault();

            if (propertyInfo == null)
                throw new Exception($"Property not found in '{entityType.Name}' by name: '{propertyName}'.");

            return propertyInfo;
        }
    }
}
