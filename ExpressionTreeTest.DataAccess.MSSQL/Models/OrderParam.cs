namespace ExpressionTreeTest.DataAccess.MSSQL.Models
{
    /// <summary>
    /// Параметр сортировки.
    /// </summary>
    public class OrderParam
    {
        /// <summary>
        /// Имя столбца.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        public OrderType Order { get; set; }
    }
}
