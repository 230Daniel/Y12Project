using System;
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
            string value = BaseConverter.IntegerToBase(Value, numberBase);
            return value == "0" ? "" : value;
        }
    }
}
