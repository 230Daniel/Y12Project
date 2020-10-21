using System;

namespace CalculatorLibrary
{
    public class Number
    {
        public int Value { get; set; }

        public Number(int value)
        {
            Value = value;
        }

        public string AsBase(int numberBase)
        {
            return Convert.ToString(Value, numberBase);
        }

        public static Number operator +(Number a, Number b)
        {
            return new Number(a.Value + b.Value);
        }

        public static Number operator -(Number a, Number b)
        {
            return new Number(a.Value - b.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
