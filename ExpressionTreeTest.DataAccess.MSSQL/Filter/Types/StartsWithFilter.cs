﻿using System;
using System.Linq.Expressions;

namespace ExpressionTreeTest.DataAccess.MSSQL.Filter.Types
{
    public class StartsWithFilter : FilterBase, IFilter
    {
        public Expression GetExpression(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });

            return Expression.Call(memberExpression, startsWithMethod, constantExpression);
        }
    }
}
