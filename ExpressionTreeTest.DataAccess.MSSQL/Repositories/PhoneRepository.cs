using AutoMapper;
using ExpressionTreeTest.DataAccess.MSSQL.Entities;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressionTreeTest.DataAccess.MSSQL.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly PhonesContext _phonesContext;
        private readonly IMapper _mapper;
        private ExpressionBuilder _expressionBuilder;

        public PhoneRepository(PhonesContext phonesContext, IMapper mapper)
        {
            _phonesContext = phonesContext;
            _mapper = mapper;

            // TODO: Extract interface and add DI
            _expressionBuilder = new ExpressionBuilder();
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
            var phonesJoin =
                from p in _phonesContext.Phones
                join scf in _phonesContext.SimCardFormats on p.SimCardFormatId equals scf.Id into scfs
                from p_scf_result in scfs.DefaultIfEmpty()
                select new PhoneExtendedInformation() {
                    Id = p.Id,
                    Name = p.Name,
                    ReleaseYear = p.ReleaseYear,
                    Color = p.Color,
                    ScreenDiagonal = p.ScreenDiagonal,
                    SimCardCount = p.SimCardCount,
                    SimCardFormatId = p.SimCardFormatId,
                    SimCardFormatName = p_scf_result.Name
                };

            //TODO: Перенести в howto / пример использования. var t = _phonesContext.Phones.AsQueryable<Phone>(); 

            var filteredPhones = _expressionBuilder.GetFilteredEntities(phonesJoin, query.FilterParams, query.FilterRule);
            var orderedPhones = _expressionBuilder.GetOrderedEntities(filteredPhones, query.OrderParams);

            //получаем общее количество
            var count = await filteredPhones.CountAsync();

            // Делим на страницы
            var pageNumber = query.PageNumber;
            var pageSize = query.PageSize;
            var pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(pageSize)));

            List<PhoneExtendedInformation> data;

            if (query.OrderParams != null) { 
                data = await orderedPhones
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
            } else {
                data = await filteredPhones
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
            }

            return new PhoneExtendedInformationResult() {
                Phones = data,
                Count = count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PageCount = pageCount
            };
        }
    }
}
