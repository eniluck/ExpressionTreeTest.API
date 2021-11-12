using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.Tests
{
    public class ExpressionBuilderTest
    {
        public class TestClass
        {
            public string StringField { get; set; }
            public int IntField { get; set; }
            public int? NullableIntField { get; set; }
            public decimal DecimalField { get; set; }
            public decimal? NullableDecimalField { get; set; }
            public DateTime? DateTimeField { get; set; }
            public DateTime? NullableDateTimeField { get; set; }
        }

        private ExpressionBuilder _expressionBuilder;

        [SetUp]
        public void Setup()
        {
            _expressionBuilder = new ExpressionBuilder();
        }

        [Test]
        [TestCase("StringField")]
        [TestCase("IntField")]
        [TestCase("NullableIntField")]
        [TestCase("DecimalField")]
        [TestCase("NullableDecimalField")]
        [TestCase("DateTimeField")]
        [TestCase("NullableDateField")]
        public void CheckPropertyNameIsExisted_ShouldReturnTrue(string fieldName)
        {
            var result = _expressionBuilder.CheckPropertyNameIsExisted<TestClass>(fieldName);
            Assert.True(result);
        }

        [Test]
        [TestCase("")]
        [TestCase("asdf")]
        [TestCase("datefield")]
        public void CheckPropertyNameIsExisted_ShouldReturnFalse(string fieldName)
        {
            var result = _expressionBuilder.CheckPropertyNameIsExisted<TestClass>(fieldName);
            Assert.False(result);
        }

        [Test]
        [TestCase("StringField", "System.String")]
        [TestCase("IntField", "System.Int32")]
        [TestCase("NullableIntField", "System.Int32")]
        [TestCase("DecimalField", "System.Decimal")]
        [TestCase("NullableDecimalField", "System.Decimal")]
        [TestCase("DateTimeField", "System.DateTime")]
        [TestCase("NullableDateField", "System.DateTime")]
        public void GetPropertyType_shouldReturnString(string fieldName, string typeStringParam)
        {
            var typeString = _expressionBuilder.GetUnderlyingPropertyType<TestClass>(fieldName).ToString();
            Assert.AreEqual(typeString, typeStringParam);
        }

        [Test]
        [TestCase("StringField", FilterType.NotNull, null)]
        [TestCase("DateTimeField", FilterType.LessThan, "10.10.2021")]
        public void CheckTypeByFieldType_shouldReturnTrue(string fieldName, FilterType filterType, string fieldValue)
        {
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };
            var result = _expressionBuilder.CheckTypeByFieldType<TestClass>(filterParam);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("StringField", FilterType.LessThan, "10")]
        public void CheckTypeByFieldType_shouldReturnFalse(string fieldName, FilterType filterType, string fieldValue)
        {
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };
            var result = _expressionBuilder.CheckTypeByFieldType<TestClass>(filterParam);

            Assert.IsFalse(result);
        }


        /*
         Null = FilterType.Null,
        NotNull = FilterType.NotNull,
        Equals = FilterType.Equals,
        NotEquals = FilterType.NotEquals,
        Blank = FilterType.Blank,
        NotBlank = FilterType.NotBlank,

        Contains = FilterType.Contains,
        NotContains = FilterType.NotContains,
        StartsWith = FilterType.StartsWith,
        NotStartWith = FilterType.NotStartWith,
        EndsWith = FilterType.EndsWith,
        NotEndWith = FilterType.NotEndWith,
         */

        [Test]
        [TestCase("StringField", FilterType.Null, null)]
        [TestCase("StringField", FilterType.NotNull, null)]
        [TestCase("StringField", FilterType.Blank, null)]
        [TestCase("StringField", FilterType.NotBlank, null)]
        [TestCase("StringField", FilterType.Equals, "test")]
        [TestCase("StringField", FilterType.NotEquals, "test")]
        [TestCase("StringField", FilterType.Contains, "test")]
        [TestCase("StringField", FilterType.NotContains, "test")]
        [TestCase("StringField", FilterType.StartsWith, "test")]
        [TestCase("StringField", FilterType.NotStartWith, "test")]
        [TestCase("StringField", FilterType.EndsWith, "test")]
        [TestCase("StringField", FilterType.NotEndsWith, "test")]
        public void GetExpression_typeString_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("IntField", FilterType.Null, null)]
        [TestCase("IntField", FilterType.NotNull, null)]
        [TestCase("IntField", FilterType.Equals, "1")]
        [TestCase("IntField", FilterType.NotEquals, "1")]
        [TestCase("IntField", FilterType.GreaterThan, "1")]
        [TestCase("IntField", FilterType.GreaterThanOrEqual, "1")]
        [TestCase("IntField", FilterType.LessThan, "1")]
        [TestCase("IntField", FilterType.LessThanOrEqual, "1")]
        public void GetExpression_typeInt_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableIntField", FilterType.Null, null)]
        [TestCase("NullableIntField", FilterType.NotNull, null)]
        [TestCase("NullableIntField", FilterType.Equals, "1")]
        [TestCase("NullableIntField", FilterType.NotEquals, "1")]
        [TestCase("NullableIntField", FilterType.GreaterThan, "1")]
        [TestCase("NullableIntField", FilterType.GreaterThanOrEqual, "1")]
        [TestCase("NullableIntField", FilterType.LessThan, "1")]
        [TestCase("NullableIntField", FilterType.LessThanOrEqual, "1")]
        public void GetExpression_NullableIntField_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("DecimalField", FilterType.Null, null)]
        [TestCase("DecimalField", FilterType.NotNull, null)]
        [TestCase("DecimalField", FilterType.Equals, "1")]
        [TestCase("DecimalField", FilterType.NotEquals, "1")]
        [TestCase("DecimalField", FilterType.GreaterThan, "1")]
        [TestCase("DecimalField", FilterType.GreaterThanOrEqual, "1")]
        [TestCase("DecimalField", FilterType.LessThan, "1")]
        [TestCase("DecimalField", FilterType.LessThanOrEqual, "1")]
        public void GetExpression_DecimalField_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableDecimalField", FilterType.Null, null)]
        [TestCase("NullableDecimalField", FilterType.NotNull, null)]
        [TestCase("NullableDecimalField", FilterType.Equals, "1")]
        [TestCase("NullableDecimalField", FilterType.NotEquals, "1")]
        [TestCase("NullableDecimalField", FilterType.GreaterThan, "1")]
        [TestCase("NullableDecimalField", FilterType.GreaterThanOrEqual, "1")]
        [TestCase("NullableDecimalField", FilterType.LessThan, "1")]
        [TestCase("NullableDecimalField", FilterType.LessThanOrEqual, "1")]
        public void GetExpression_NullableDecimalField_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("DateTimeField", FilterType.Null, null)]
        [TestCase("DateTimeField", FilterType.NotNull, null)]
        [TestCase("DateTimeField", FilterType.Equals, "01.01.2021")]
        [TestCase("DateTimeField", FilterType.NotEquals, "01.01.2021")]
        [TestCase("DateTimeField", FilterType.GreaterThan, "01.01.2021")]
        [TestCase("DateTimeField", FilterType.GreaterThanOrEqual, "01.01.2021")]
        [TestCase("DateTimeField", FilterType.LessThan, "01.01.2021")]
        [TestCase("DateTimeField", FilterType.LessThanOrEqual, "01.01.2021")]
        public void GetExpression_DateTimeField_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableDateTimeField", FilterType.Null, null)]
        [TestCase("NullableDateTimeField", FilterType.NotNull, null)]
        [TestCase("NullableDateTimeField", FilterType.Equals, "01.01.2021")]
        [TestCase("NullableDateTimeField", FilterType.NotEquals, "01.01.2021")]
        [TestCase("NullableDateTimeField", FilterType.GreaterThan, "01.01.2021")]
        [TestCase("NullableDateTimeField", FilterType.GreaterThanOrEqual, "01.01.2021")]
        [TestCase("NullableDateTimeField", FilterType.LessThan, "01.01.2021")]
        [TestCase("NullableDateTimeField", FilterType.LessThanOrEqual, "01.01.2021")]
        public void GetExpression_NullableDateTimeField_shouldReturnResult(string fieldName, FilterType filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var result = _expressionBuilder.GetExpression<TestClass>(param, filterParam);

            Assert.NotNull(result);
        }

        //TODO: негативные кейсы для _expressionBuilder.GetExpression.

        [Test]
        public void test()
        {
            /*var result = typeof(Queryable).GetMethod("OrderBy", new Type[] { typeof(string) });
            var result2 = typeof(Queryable).GetMethod("OrderByDescending", new Type[] { typeof(string) });*/
            var result__1 = typeof(Queryable).GetMethods().Where(method => method.Name == "OrderBy");
            var result__2 = typeof(Queryable).GetMethods().Where(method => method.Name == "OrderByDescending");

            MethodInfo orderbyMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);
            MethodInfo orderbyDescendingMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);
            Assert.NotNull(1);
        }
        
    }
}
