using ExpressionTreeTest.DataAccess.MSSQL;
using ExpressionTreeTest.DataAccess.MSSQL.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.Tests
{
    public class ExpressionBuilderTest
    {
        public class TestClass
        {
            public string StringField { get; set; }
            public int SomeField { get; set; }
            public DateTime? DateField { get; set; }
        }

        private ExpressionBuilder _expressionBuilder;

        [SetUp]
        public void Setup()
        {
            _expressionBuilder = new ExpressionBuilder();
        }

        [Test]
        [TestCase("StringField")]
        [TestCase("SomeField")]
        [TestCase("DateField")]
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
        [TestCase("SomeField", "System.Int32")]
        [TestCase("DateField", "System.DateTime")]
        public void GetPropertyType_shouldReturnString(string fieldName, string typeString)
        {
            var result = _expressionBuilder.GetPropertyType<TestClass>(fieldName);
            Assert.AreEqual(result, typeString);
        }

        [Test]
        [TestCase("StringField", FilterType.NotNull, null)]
        [TestCase("DateField", FilterType.LessThan, "10.10.2021")]
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
    }
}
