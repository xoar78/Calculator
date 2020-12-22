using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Sum_6Plus2_Returned()
        {
            string result = ParserCalculator.Calculate("6+2");
            Assert.AreEqual("8", result);
        }
        [TestMethod]
        public void Div_9Div3_Returned()
        {
            string result = ParserCalculator.Calculate("9/3");
            Assert.AreEqual("3", result);
        }
        [TestMethod]
        public void TestBr()
        {
            string result = ParserCalculator.Calculate("4+(2+3)*4/(10-5)");
            Assert.AreEqual("8", result);
        }
        [TestMethod]
        public void TestBr2()
        {
            string result = ParserCalculator.Calculate("((4))+(2*3)-(1+2)*3-(10/((7-2)-1))");
            Assert.AreEqual("-1,5000", result);
        }
        [TestMethod]
        public void TestEr1()
        {
            string result = ParserCalculator.Calculate("(8+5");
            Assert.AreEqual("Error", result);
        }
        [TestMethod]
        public void TestEr2()
        {
            string result = ParserCalculator.Calculate("8+5)");
            Assert.AreEqual("Error", result);
        }
        [TestMethod]
        public void TestEr3()
        {
            string result = ParserCalculator.Calculate("1/2*3+");
            Assert.AreEqual("Error", result);
        }
        [TestMethod]
        public void Test8()
        {
            string result = ParserCalculator.Calculate("10/3*6+3");
            Assert.AreEqual("23", result);
        }
        [TestMethod]
        public void Test9()
        {
            string result = ParserCalculator.Calculate("8,6+4,5");
            Assert.AreEqual("13,1000", result);
        }
        [TestMethod]
        public void Test10()
        {
            string result = ParserCalculator.Calculate("8,6+45");
            Assert.AreEqual("53,6000", result);
        }
        [TestMethod]
        public void Test11()
        {
            string result = ParserCalculator.Calculate("86+4,5");
            Assert.AreEqual("90,5000", result);
        }

    }
}
