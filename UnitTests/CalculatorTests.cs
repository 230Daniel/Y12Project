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
    }
}
