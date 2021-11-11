using AutoMapper;
using ExpressionTreeTest.DataAccess.MSSQL.Entities;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly PhonesContext _phonesContext;
        private readonly IMapper _mapper;

        public PhoneRepository(PhonesContext phonesContext, IMapper mapper)
        {
            _phonesContext = phonesContext;
            _mapper = mapper;
        }

        public async Task<List<Phone>> GetPhonesAsync()
        {
            var phones = await _phonesContext
                .Phones
                .AsNoTracking()
                .ToListAsync();

            return phones;
        }

        public async Task<PhoneExtendedInformationResult> GetAllInformationByParams(QueryParams query)
        {
            var PhonesJoin =
                from p in _phonesContext.Phones
                join scf in _phonesContext.SimCardFormats on p.SimCardFormatId equals scf.Id into scfs
                from p_scf_result in scfs.DefaultIfEmpty()
                select new PhoneExtendedInformation() {
                    Id = p.Id,
                    Name = p.Name,
                    ReleaseYear = p.ReleaseYear,
                    ScreenDiagonal = p.ScreenDiagonal,
                    SimCardCount = p.SimCardCount,
                    SimCardFormatId = p.SimCardFormatId,
                    SimCardFormatName = p_scf_result.Name
                };

            IQueryable<PhoneExtendedInformation> filteredPhones = GetFilteredPhones(PhonesJoin, query.FilterParams, query.filterConditions);

            //получаем общее количество
            var count = await filteredPhones.CountAsync();

            /// Делим на страницы
            var pageNumber = query.PageNumber;
            var pageSize = query.PageSize;
            var pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(pageSize)));

            var data = await filteredPhones
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var result = new PhoneExtendedInformationResult() {
                Phones = data,
                Count = count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PageCount = pageCount
            };

            return result;
        }

        public IQueryable<PhoneExtendedInformation> GetFilteredPhones(IQueryable<PhoneExtendedInformation> phonesJoin, List<FilterParam> filterParams, string filterCondition)
        {
            var predicate = new ExpressionBuilder().GetPredicate<PhoneExtendedInformation>(filterParams, filterCondition);

            var filteredEntities = phonesJoin.Where(predicate);

            return filteredEntities;
        }
    }
}
