using ExpressionTreeTest.DataAccess.MSSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpressionTreeTest.DataAccess.MSSQL
{
    public class PhonesContext : DbContext
    {
        public PhonesContext(DbContextOptions<PhonesContext> options) : base(options)
        {

        }

        public DbSet<Phone> Phones { get; set; }
        public DbSet<SimCardFormat> SimCardFormats { get; set; }
    }
}
