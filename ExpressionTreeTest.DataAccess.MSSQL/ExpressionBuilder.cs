using ExpressionTreeTest.DataAccess.MSSQL.Filter;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL
{
    public class ExpressionBuilder
    {
        private readonly ReversePolishNotation _rpn;

        public ExpressionBuilder()
        {
            _rpn = new ReversePolishNotation();
        }

        public IOrderedQueryable<T> GetOrderedEntities<T>(IQueryable<T> entities, List<OrderParam> orderParams)
        {
            if (orderParams == null || orderParams.Count == 0)
                return null;

            IOrderedQueryable<T> orderedEntities = null;

            if (orderParams[0].Order == "asc")
                orderedEntities = entities.OrderBy(p => EF.Property<string>(p, orderParams[0].FieldName));
            else
                orderedEntities = entities.OrderByDescending(p => EF.Property<string>(p, orderParams[0].FieldName));

            for (int i = 1; i < orderParams.ToArray().Length; i++) {
                if (orderParams[0].Order == "asc")
                    orderedEntities = orderedEntities.OrderBy(p => EF.Property<string>(p, orderParams[i].FieldName));
                else
                    orderedEntities = orderedEntities.OrderByDescending(p => EF.Property<string>(p, orderParams[i].FieldName));
            }

            return orderedEntities;
        }

        /// <summary>
        /// Получить отфильтрованные результаты.
        /// </summary>
        /// <typeparam name="T">Тип сущности для фильтрации.</typeparam>
        /// <param name="entities">Набор значений для фильтрации.</param>
        /// <param name="filterParams">Параметры фильтрации.</param>
        /// <param name="filterRule">Правило фильтрации.</param>
        /// <returns></returns>
        public IQueryable<T> GetFilteredEntities<T>(IQueryable<T> entities, List<FilterParam> filterParams, string filterRule)
        {
            List<EntityFilterParam<T>> entityFilterParams = new List<EntityFilterParam<T>>();

            EntityFilterParamBuilder<T> builder = new EntityFilterParamBuilder<T>();

            foreach (var item in filterParams) {
                entityFilterParams.Add(builder.BuildByFilterParam(item));
            }

            var predicate = GetWherePredicate<T>(entityFilterParams, filterRule);

            var filteredEntities = entities.Where(predicate);

            return filteredEntities;
        }

        /// <summary>
        /// Получить предикат для операции where.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="filterParams">Параметры фильтрации.</param>
        /// <param name="filterRule">Правило фильтрации.</param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetWherePredicate<T>(List<EntityFilterParam<T>> filterParams, string filterRule)
        {
            if (filterParams == null || filterParams.Count == 0)
                return null;

            ParameterExpression expParam = Expression.Parameter(typeof(T), "p");

            string rpnStringRule = _rpn.GetRpnStringRule(filterRule);

            Expression exp = _rpn.FormWherePredicate<T>(rpnStringRule, filterParams, expParam);

            return Expression.Lambda<Func<T, bool>>(exp, expParam);
        }

        /// <summary>
        /// Создать выражение из параметра по фильтру.
        /// </summary>
        /// <typeparam name="T">Параметр типа.</typeparam>
        /// <param name="expParam">Параметр для дерева выражений.</param>
        /// <param name="filter">Параметр фильтрации.</param>
        /// <returns>Выражение.</returns>
        public Expression GetExpression<T>(ParameterExpression expParam, EntityFilterParam<T> filter)
        {
            MemberExpression member = Expression.Property(expParam, filter.Property.Name);
            return filter.FilterType.GetExpression<T>(member, filter);
        }
    }
}
