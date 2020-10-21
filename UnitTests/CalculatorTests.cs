using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;

namespace UnitTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void ConvertToBinary()
        {
            Number number = new Number(50);
            Assert.AreEqual("110010", number.AsBase(2));
        }

        [TestMethod]
        public void ConvertToB16()
        {
            Number number = new Number(50);
            Assert.AreEqual("32", number.AsBase(16));
        }

        [TestMethod]
        public void ConvertToInt()
        {
            Number number = new Number("110010", 2);

            Assert.AreEqual(50, number.Value);
        }

        [TestMethod]
        public void AddNumbers()
        {
            Number number1 = new Number(10);
            Number number2 = new Number(5);

            Number expectedNumber = new Number(15);
            Number actualNumber = number1 + number2;

            Assert.AreEqual(expectedNumber.Value, actualNumber.Value);
        }
    }
}
