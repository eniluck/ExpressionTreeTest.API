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
        /// Порядок выполенния операций. ( 1 & 2 ) | ( 3 & 4 ) 
        /// </summary>
        public string filterConditions { get; set; }

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
