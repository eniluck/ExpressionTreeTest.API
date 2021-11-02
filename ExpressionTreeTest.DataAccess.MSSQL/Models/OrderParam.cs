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
        public string ColumnName { get; set; }

        /// <summary>
        /// Порядок:
        ///     По возрастанию (0-9):   Asc
        ///     По убвыванию (9-0)  :   Desc
        /// </summary>
        public string Order { get; set; }
    }
}
