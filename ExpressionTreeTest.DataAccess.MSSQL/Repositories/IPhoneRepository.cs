using ExpressionTreeTest.DataAccess.MSSQL.Models;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Repositories
{
    public interface IPhoneRepository
    {
        Task<PhoneExtendedInformationResult> GetAllInformationByParams(QueryParams query);
    }
}