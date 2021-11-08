using ExpressionTreeTest.DataAccess.MSSQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeTest.Tests
{
    public class ReversePolishNotationTest
    {
        [Test]
        public void GetExpression_shouldReturnRPNString()
        {
            ReversePolishNotation rpn = new ReversePolishNotation();
            string testString = "(1 & 2) | (3 & 4)";
            string result = rpn.GetExpression(testString);
            Assert.AreEqual(result, "1 2 & 3 4 & | ");
        }
    }
}
