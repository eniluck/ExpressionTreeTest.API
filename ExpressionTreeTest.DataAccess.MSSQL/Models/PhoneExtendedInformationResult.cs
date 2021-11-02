using System.Collections.Generic;

namespace ExpressionTreeTest.DataAccess.MSSQL.Models
{
    public class PhoneExtendedInformationResult
    {
        public List<PhoneExtendedInformation> Phones { get; set; }
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}