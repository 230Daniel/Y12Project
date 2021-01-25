using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CalculatorLibrary
{
    public static class BaseConverter
    {
        public static char[] Characters { get; set; }

        public static string IntegerToBase(BigInteger inputNumber, int numberBase)
        {
            if (numberBase > Characters.Length) return "";

            List<BigInteger> outputCharIndexes = new List<BigInteger>();

            while (inputNumber > 0)
            {
                BigInteger remainder = inputNumber % numberBase;
                outputCharIndexes.Add(remainder);
                inputNumber /= numberBase;
            }

            List<char> output = outputCharIndexes.Select(x => Characters[(int) x]).Reverse().ToList();
            return output.Count > 0 ? string.Concat(output) : "";
        }

        public static BigInteger BaseToInteger(string inputNumber, int numberBase)
        {
            if (numberBase > Characters.Length) return 0;

            List<char> inputCharacters = inputNumber.ToList();
            inputCharacters.Reverse();

            BigInteger output = 0;
            BigInteger position = 1;
            foreach (char character in inputCharacters)
            {
                int value = Array.IndexOf(Characters, character);
                output += value * position;
                position *= numberBase;
            }

            return output;
        }

        public static char[] GetValidCharactersForBase(int numberBase)
        {
            return Characters.Take(numberBase).ToArray();
        }
    }
}
