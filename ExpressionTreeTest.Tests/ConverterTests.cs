using ExpressionTreeTest.DataAccess.MSSQL.Converters;
using NUnit.Framework;
using System;

namespace ExpressionTreeTest.Tests
{
    public class ConverterTests
    {
        [Test]
        [TestCase("01.01.2021", 1,1,2021)]
        [TestCase("28.02.2010", 28,2,2010)]
        public void StringToDateTimeConverter_shouldReturnResult(string value, int day, int month, int year)
        {
            DateTime dateTime = new DateTime(year, month, day);
            StringToDateTimeConverter toDateTimeConverter = new StringToDateTimeConverter();
            var convertedValue = toDateTimeConverter.Convert(value);

            Assert.True(convertedValue is DateTime );
            Assert.AreEqual(dateTime, convertedValue);
        }

        [Test]
        [TestCase("28.28.2010")]
        [TestCase("строка")]
        public void StringToDateTimeConverter_shouldReturnException(string value)
        {
            StringToDateTimeConverter toDateTimeConverter = new StringToDateTimeConverter();

            Assert.Throws<Exception>(() => { toDateTimeConverter.Convert(value); });
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase("1.2", 1.2)]
        [TestCase("1,2", 1.2)]
        public void StringToDecimalConverter_shouldReturnResult(string value, decimal decimalValue)
        {
            StringToDecimalConverter toDecimalConverter = new StringToDecimalConverter();
            var convertedValue = toDecimalConverter.Convert(value);

            Assert.True(convertedValue is decimal);
            Assert.AreEqual(decimalValue, convertedValue);
        }

        [Test]
        [TestCase("1dd")]
        [TestCase("12 3")]
        public void StringToDecimalConverter_shouldReturnException(string value)
        {
            StringToDecimalConverter toDecimalConverter = new StringToDecimalConverter();

            Assert.Throws<Exception>(() => { toDecimalConverter.Convert(value); });
        }

        [Test]
        [TestCase("1", 1)]
        [TestCase("-2", -2)]
        public void StringToIntConverter_shouldReturnResult(string value, int intValue)
        {
            StringToIntConverter toIntConverter = new StringToIntConverter();
            var convertedValue = toIntConverter.Convert(value);

            Assert.True(convertedValue is int);
            Assert.AreEqual(intValue, convertedValue);
        }

        [Test]
        [TestCase("1dd")]
        [TestCase("12 3")]
        public void StringToIntConverter_shouldReturnException(string value)
        {
            StringToDecimalConverter toIntConverter = new StringToDecimalConverter();

            Assert.Throws<Exception>(() => { toIntConverter.Convert(value); });
        }
    }
}
