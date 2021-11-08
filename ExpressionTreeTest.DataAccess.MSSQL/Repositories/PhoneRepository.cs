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
            var predicate = GetExpression<PhoneExtendedInformation>(filterParams, filterCondition);

            var filteredEntities = phonesJoin.Where(predicate);
            //filteredEntities = phonesJoin.Where(t=> ( (1==2) & (5==6) ) | (3==4) & (7==8));

            return filteredEntities;
        }

        public Expression<Func<T, bool>> GetExpression<T>(List<FilterParam> filterParams, string filterCondition)
        {
            if (filterParams == null || filterParams.Count == 0)
                return null;

            //.Where(p => EF.Property<string>(p, filter.FieldName) == null);
            ParameterExpression param = Expression.Parameter(typeof(T), "p");

            /* Expression exp = null;*/

            //Expression.Or
            //Expression.And

            // Обработать список по строке filter
            //exp = GetExpression<T>(param, filterParams[0]);
            ReversePolishNotation rpn = new ReversePolishNotation();
            string rpnString = rpn.GetExpression(filterCondition);

            Expression exp = FormPredicate<T>(rpnString, filterParams, param);

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public Expression FormPredicate<T>(string input, List<FilterParam> filterParams, ParameterExpression param)
        {
            Expression result = null; //Результат
            Stack<Expression> temp = new Stack<Expression>(); //Временный стек для решения

            for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (char.IsDigit(input[i])) {
                    string stringIndex = string.Empty;

                    while (!ReversePolishNotation.IsDelimeter(input[i]) && !ReversePolishNotation.IsOperator(input[i])) //Пока не разделитель
                    {
                        stringIndex += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }

                    Expression exp = GetExpression<T>(param, filterParams[int.Parse(stringIndex)]);

                    temp.Push(exp); //Записываем в стек
                    i--;
                }
                else if (ReversePolishNotation.IsOperator(input[i])) //Если символ - оператор
                {
                    //Берем два последних значения из стека
                    Expression left = temp.Pop();
                    Expression right = temp.Pop();

                    switch (input[i]) //И производим над ними действие, согласно оператору
                    {
                        case '|':
                            result =
                          Expression.Or(left, right);
                            break;
                        case '&':
                            result =
                          Expression.And(left, right);
                            break;
                    }
                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterParam filter)
        {
            MemberExpression member = Expression.Property(param, filter.FieldName);
            ConstantExpression filterConstant = Expression.Constant(filter.FieldValue);
            ConstantExpression nullConstant = Expression.Constant(null);
            ConstantExpression blankStringConstant = Expression.Constant("");

            // получить тип поля и выбрать его возможные фильтры

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
