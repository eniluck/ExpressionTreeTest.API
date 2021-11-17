using System;
using System.ComponentModel;
using System.Reflection;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter
{
    /// <summary>
    /// Параметр фильтрации для типа.
    /// </summary>
    public class EntityFilterParam<T>
    {
        /// <summary>
        /// К какой сущности относится.
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Свойство для фильтрации.
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Тип фильтра.
        /// </summary>
        public IFilter FilterType { get; set; }

        /// <summary>
        /// Значение фильтра в зависимости от типа свойства.
        /// </summary>
        public object FilterValue { get; set; }
    }
}
