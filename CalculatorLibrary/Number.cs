using System;
using System.Linq;
using System.Numerics;

namespace CalculatorLibrary
{
    public class Number
    {
        public BigInteger Value { get; }

        public Number(BigInteger value)
        {
            Value = value;
        }

        public Number(string value, int numberBase)
        {
            Value = BaseConverter.BaseToInteger(value, numberBase);
        }

        public string AsBase(int numberBase)
        {
            return BaseConverter.IntegerToBase(Value, numberBase);
        }
    }
}
