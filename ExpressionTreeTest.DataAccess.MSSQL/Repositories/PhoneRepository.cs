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

            IQueryable<PhoneExtendedInformation> filteredPhones = GetFilteredPhones(PhonesJoin, query.FilterParams);

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

        public IQueryable<PhoneExtendedInformation> GetFilteredPhones(IQueryable<PhoneExtendedInformation> phonesJoin, List<FilterParam> filterParams)
        {
            var predicate = GetExpression<PhoneExtendedInformation>(filterParams);

            var filteredEntities = phonesJoin.Where(predicate);

            return filteredEntities;
        }

        public static Expression<Func<T, bool>> GetExpression<T>(List<FilterParam> filterParams)
        {
            if (filterParams == null || filterParams.Count == 0)
                return null;

            //.Where(p => EF.Property<string>(p, filter.FieldName) == null);
            ParameterExpression param = Expression.Parameter(typeof(T), "p");

            Expression exp = null;

            exp = GetExpression<T>(param, filterParams[0]);

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterParam filter)
        {
            MemberExpression member = Expression.Property(param, filter.FieldName);
            ConstantExpression filterConstant = Expression.Constant(filter.FieldValue);
            ConstantExpression nullConstant = Expression.Constant(null);
            ConstantExpression blankStringConstant = Expression.Constant("");

            switch (filter.FilterType) {
                case FilterType.Equals:
                    return Expression.Equal(member, filterConstant);
                case FilterType.NotEquals:
                    return Expression.NotEqual(member, filterConstant);
                case FilterType.Blank:
                    return Expression.Equal(member, blankStringConstant);
                case FilterType.NotBlank:
                    return Expression.NotEqual(member, blankStringConstant);
                case FilterType.Null:
                    return Expression.Equal(member, nullConstant);
                case FilterType.NotNull:
                    return Expression.NotEqual(member, nullConstant);
                case FilterType.Contains:
                    return Expression.Call(member, containsMethod, filterConstant);
                case FilterType.NotContains:
                    return Expression.Not(
                                Expression.Call(member, containsMethod, filterConstant));
                case FilterType.StartsWith:
                    return Expression.Call(member, startsWithMethod, filterConstant);
                case FilterType.NotStartWith:
                    return Expression.Not(
                                Expression.Call(member, startsWithMethod, filterConstant));
                case FilterType.EndsWith:
                    return Expression.Call(member, endsWithMethod, filterConstant);
                case FilterType.NotEndWith:
                    return Expression.Not(
                            Expression.Call(member, endsWithMethod, filterConstant));
                case FilterType.GreaterThan:
                    return Expression.GreaterThan(member, filterConstant);
                case FilterType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, filterConstant);
                case FilterType.LessThan:
                    return Expression.LessThan(member, filterConstant);
                case FilterType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, filterConstant);
            }
            return null;
        }

        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
    }
}
