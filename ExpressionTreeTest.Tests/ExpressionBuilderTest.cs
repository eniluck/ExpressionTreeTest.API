using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Filter;
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
        private EntityFilterParamBuilder<TestClass> _paramBuilder;

        [SetUp]
        public void Setup()
        {
            _expressionBuilder = new ExpressionBuilder();
            _paramBuilder = new EntityFilterParamBuilder<TestClass>();
        }
        /*
        [Test]
        [TestCase("StringField")]
        [TestCase("IntField")]
        [TestCase("NullableIntField")]
        [TestCase("DecimalField")]
        [TestCase("NullableDecimalField")]
        [TestCase("DateTimeField")]
        [TestCase("NullableDateTimeField")]
        public void CheckPropertyNameIsExisted_ShouldReturnTrue(string fieldName)
        {
            var result = _expressionBuilder.CheckPropertyNameIsExisted<TestClass>(fieldName);
            Assert.True(result);
        }

        [Test]
        [TestCase("")]
        [TestCase("asdf")]
        [TestCase("datefield")]
        [TestCase(null)]
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
        [TestCase("NullableDateTimeField", "System.DateTime")]
        public void GetPropertyType_shouldReturnString(string fieldName, string typeStringParam)
        {
            var typeString = _expressionBuilder.GetUnderlyingPropertyType<TestClass>(fieldName).ToString();
            Assert.AreEqual(typeString, typeStringParam);
        }*/

        /*[Test]
        [TestCase("StringField", "!null", null)]
        [TestCase("DateTimeField", "<", "10.10.2021")]
        public void CheckTypeByFieldType_shouldReturnTrue(string fieldName, string filterType, string fieldValue)
        {
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };
            var result = _expressionBuilder.CheckFilterByFieldType<TestClass>(filterParam);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("StringField", "<", "10")]
        public void CheckTypeByFieldType_shouldReturnFalse(string fieldName, string filterType, string fieldValue)
        {
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };
            var result = _expressionBuilder.CheckFilterByFieldType<TestClass>(filterParam);

            Assert.IsFalse(result);
        }*/

        [Test]
        [TestCase("StringField", "null", null)]
        [TestCase("StringField", "!null", null)]
        [TestCase("StringField", "blank", null)]
        [TestCase("StringField", "!blank", null)]
        [TestCase("StringField", "equals", "test")]
        [TestCase("StringField", "!equals", "test")]
        [TestCase("StringField", "contains", "test")]
        [TestCase("StringField", "!contains", "test")]
        [TestCase("StringField", "starts", "test")]
        [TestCase("StringField", "!starts", "test")]
        [TestCase("StringField", "ends", "test")]
        [TestCase("StringField", "!ends", "test")]
        public void GetExpression_typeString_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("IntField", "null", null)]
        [TestCase("IntField", "!null", null)]
        [TestCase("IntField", "equals", "1")]
        [TestCase("IntField", "!equals", "1")]
        [TestCase("IntField", ">", "1")]
        [TestCase("IntField", ">=", "1")]
        [TestCase("IntField", "<", "1")]
        [TestCase("IntField", "<=", "1")]
        public void GetExpression_typeInt_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableIntField", "null", null)]
        [TestCase("NullableIntField", "!null", null)]
        [TestCase("NullableIntField", "equals", "1")]
        [TestCase("NullableIntField", "!equals", "1")]
        [TestCase("NullableIntField", ">", "1")]
        [TestCase("NullableIntField", ">=", "1")]
        [TestCase("NullableIntField", "<", "1")]
        [TestCase("NullableIntField", "<=", "1")]
        public void GetExpression_NullableIntField_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("DecimalField", "null", null)]
        [TestCase("DecimalField", "!null", null)]
        [TestCase("DecimalField", "equals", "1")]
        [TestCase("DecimalField", "!equals", "1")]
        [TestCase("DecimalField", ">", "1")]
        [TestCase("DecimalField", ">=", "1")]
        [TestCase("DecimalField", "<", "1")]
        [TestCase("DecimalField", "<=", "1")]
        public void GetExpression_DecimalField_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableDecimalField", "null", null)]
        [TestCase("NullableDecimalField", "!null", null)]
        [TestCase("NullableDecimalField", "equals", "1")]
        [TestCase("NullableDecimalField", "!equals", "1")]
        [TestCase("NullableDecimalField", ">", "1")]
        [TestCase("NullableDecimalField", ">=", "1")]
        [TestCase("NullableDecimalField", "<", "1")]
        [TestCase("NullableDecimalField", "<=", "1")]
        public void GetExpression_NullableDecimalField_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("DateTimeField", "null", null)]
        [TestCase("DateTimeField", "!null", null)]
        [TestCase("DateTimeField", "equals", "01.01.2021")]
        [TestCase("DateTimeField", "!equals", "01.01.2021")]
        [TestCase("DateTimeField", ">", "01.01.2021")]
        [TestCase("DateTimeField", ">", "01.01.2021")]
        [TestCase("DateTimeField", "<", "01.01.2021")]
        [TestCase("DateTimeField", "<=", "01.01.2021")]
        public void GetExpression_DateTimeField_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }

        [Test]
        [TestCase("NullableDateTimeField", "null", null)]
        [TestCase("NullableDateTimeField", "!null", null)]
        [TestCase("NullableDateTimeField", "equals", "01.01.2021")]
        [TestCase("NullableDateTimeField", "!equals", "01.01.2021")]
        [TestCase("NullableDateTimeField", ">", "01.01.2021")]
        [TestCase("NullableDateTimeField", ">", "01.01.2021")]
        [TestCase("NullableDateTimeField", "<", "01.01.2021")]
        [TestCase("NullableDateTimeField", "<=", "01.01.2021")]
        public void GetExpression_NullableDateTimeField_shouldReturnResult(string fieldName, string filterType, string fieldValue)
        {
            ParameterExpression param = Expression.Parameter(typeof(TestClass), "p");
            var filterParam = new FilterParam() {
                FieldName = fieldName,
                FilterType = filterType,
                FieldValue = fieldValue
            };

            var entityFilterParam = _paramBuilder.BuildByFilterParam(filterParam);

            var result = _expressionBuilder.GetExpression<TestClass>(param, entityFilterParam);

            Assert.NotNull(result);
        }
    }
}
