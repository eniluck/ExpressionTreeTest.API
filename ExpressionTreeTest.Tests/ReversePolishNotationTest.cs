using ExpressionTreeTest.DataAccess.MSSQL;
using NUnit.Framework;
using System.Reflection;

namespace ExpressionTreeTest.Tests
{
    public class ReversePolishNotationTest
    {
        [Test]
        public void GetExpression_shouldReturnRPNString()
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            string testString = "(1 & 2) | (3 & 4)";
            string result = rpn.GetRpnStringRule(testString);
            Assert.AreEqual(result, "1 2 & 3 4 & | ");
        }

        [Test]
        [TestCase("( 0 & 1) | 2")]
        [TestCase("( 0 & 1) | (2 & 3 )")]
        public void CheckBrackets_shouldReturnTrue(string input)
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            Assert.True(rpn.CheckBrackets(input));
        }

        [Test]
        [TestCase("( 0 & 1) | 2)")]
        [TestCase(" 0 & 1) | 2)")]
        [TestCase("( 0 & 1 | 2")]
        [TestCase("( 0 & 1 ( | 2")]
        [TestCase("( 0 ) 1 ) ( 2 ( 3 )")]
        public void CheckBrackets_shouldReturnFalse(string input)
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            Assert.False(rpn.CheckBrackets(input));
        }

        [Test]
        [TestCase("()0123456789&| ")]
        public void CheckString_shouldReturnTrue(string input)
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            Assert.True(rpn.CheckString(input));
        }

        [Test]
        [TestCase("()0123456789&| xyz")]
        public void CheckString_shouldReturnFalse(string input)
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            Assert.False(rpn.CheckString(input));
        }
    }
}
