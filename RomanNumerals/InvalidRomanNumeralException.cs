using System;

namespace RomanNumerals
{
    public class InvalidRomanNumeralException : Exception
    {
        public InvalidRomanNumeralException(string numeral)
            :base($"'{numeral}' is an invalid Roman Numeral")
        {
        }
    }
}
