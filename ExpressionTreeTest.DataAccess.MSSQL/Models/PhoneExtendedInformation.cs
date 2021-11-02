namespace ExpressionTreeTest.DataAccess.MSSQL.Models
{
    public class PhoneExtendedInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ReleaseYear { get; set; }
        public decimal? ScreenDiagonal { get; set; }
        public int? SimCardCount { get; set; }
        public int? SimCardFormatId { get; set; }
        public string SimCardFormatName { get; set; }
    }
}
