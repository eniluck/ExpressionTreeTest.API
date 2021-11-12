using System.Collections.Generic;

namespace ExpressionTreeTest.DataAccess.MSSQL.Models
{
    /// <summary>
    /// Модель для запроса.
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// Список параметров для фильтрации запроса.
        /// </summary>
        public List<FilterParam> FilterParams { get; set; }

        /// <summary>
        /// Правило выполенния операций. ( 0 & 1 ) | ( 2 & 3 ) 
        /// </summary>
        public string FilterRule { get; set; }

        /// <summary>
        /// Список параметров для сортировки запроса.
        /// </summary>
        public List<OrderParam> OrderParams { get; set; }

        /// <summary>
        /// Номер страницы запроса.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Размер страницы запроса.
        /// </summary>
        public int PageSize { get; set; }
    }
}
