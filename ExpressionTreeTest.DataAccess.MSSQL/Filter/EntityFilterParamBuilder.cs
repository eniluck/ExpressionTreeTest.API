using ExpressionTreeTest.DataAccess.MSSQL.Converters;
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
            entityFilterParam.FilterValue = GetValue(filterParam.FieldValue, entityFilterParam.Property.PropertyType);

            return entityFilterParam;
        }

        Dictionary<string, IStringToTypeConverter> stringConverters = new Dictionary<string, IStringToTypeConverter>() {
            { "System.String", new StringToStringConverter()},
            { "System.DateTime", new StringToDateTimeConverter()},
            { "System.Decimal", new StringToDecimalConverter()},
            { "System.Int32", new StringToIntConverter()},
            { "System.Int16", new StringToIntConverter()},
        };

        private object GetValue(string fieldValue, Type type)
        {
            if (fieldValue == null)
                return null;

            var baseTypeString = GetBaseType(type).ToString();

            IStringToTypeConverter converter;
            var result = stringConverters.TryGetValue(baseTypeString, out converter);
            if (result == false)
                throw new Exception($"Converter to type '{baseTypeString}' not found in registered converters.");

            return converter.Convert(fieldValue);
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
