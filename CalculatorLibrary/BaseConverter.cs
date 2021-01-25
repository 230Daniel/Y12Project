using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CalculatorLibrary
{
    public static class BaseConverter
    {
        private static char[] _characters = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public static string IntegerToBase(BigInteger inputNumber, int numberBase)
        {
            List<BigInteger> outputCharIndexes = new List<BigInteger>();

            while (inputNumber > 0)
            {
                BigInteger remainder = inputNumber % numberBase;
                outputCharIndexes.Add(remainder);
                inputNumber /= numberBase;
            }

            List<char> output = outputCharIndexes.Select(x => _characters[(int) x]).Reverse().ToList();
            return string.Concat(output);
        }

        public static BigInteger BaseToInteger(string inputNumber, int numberBase)
        {
            List<char> inputCharacters = inputNumber.ToList();
            inputCharacters.Reverse();

            BigInteger output = 0;
            BigInteger position = 1;
            foreach (char character in inputCharacters)
            {
                int value = Array.IndexOf(_characters, character);
                output += value * position;
                position *= numberBase;
            }

            return output;
        }

        public static char[] GetValidCharactersForBase(int numberBase)
        {
            return _characters.Take(numberBase).ToArray();
        }
    }
}
