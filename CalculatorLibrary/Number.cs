using System;

namespace CalculatorLibrary
{
    public class Number
    {
        public long Value { get; }
        public bool Invalid { get; protected set; }

        public Number(long value)
        {
            Value = value;
        }

        public Number(string value, int numberBase)
        {
            try
            {
                Value = Convert.ToInt64(value, numberBase);
            }
            catch (OverflowException)
            {
                Value = long.MaxValue;
            }
            catch
            {
                Value = 0;
            }
        }

        public string AsBase(int numberBase)
        {
            string value = Convert.ToString(Value, numberBase);
            return value == "0" ? "" : value;
        }

        public static Number operator +(Number a, Number b)
        {
            return new Number(a.Value + b.Value);
        }

        public static Number operator -(Number a, Number b)
        {
            return new Number(a.Value - b.Value);
        }

        public static bool operator ==(Number a, Number b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Number a, Number b)
        {
            return a.Value != b.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Number) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private bool Equals(Number other)
        {
            return Value == other.Value;
        }
    }
}
