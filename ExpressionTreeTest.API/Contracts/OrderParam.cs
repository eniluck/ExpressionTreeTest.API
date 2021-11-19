namespace ExpressionTreeTest.API.Contracts
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
        public string Order { get; set; }
    }
}
