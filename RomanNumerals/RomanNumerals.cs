using System;
using System.Linq;

namespace RomanNumerals
{
    public static class RomanNumerals
    {
        private const string One = "I";
        private const string Five = "V";
        private const string Ten = "X";
        private const string Fifty = "L";
        private const string OneHundred = "C";
        private const string FiveHundred = "D";
        private const string OneThousand = "M";

        private static readonly string[] ValidStrings =
            {One, Five, Ten, Fifty, OneHundred, FiveHundred, OneThousand};

        public static string ToRoman(int number)
        {
            return string.Empty;
        }

        public static int ParseRoman(string romanNumeral)
        {
            if (romanNumeral == null)
            {
                throw new ArgumentNullException(nameof(romanNumeral));
            }

            var upper = romanNumeral.ToUpperInvariant();
            
            if (!ContainsValidLetters(upper))
            {
                throw new InvalidRomanNumeralException();
            }

            if (upper.Length == 1)
            {
                return ParseSingleLetter(upper);
            }

            if (ContainsFourInARow(upper))
            {
                throw new InvalidRomanNumeralException();
            }


            var value = ParseRomanNumeral(upper);

            if (value > 0)
            {
                return value;
            }

            throw new NotImplementedException();
        }

        private static int ParseRomanNumeral(string romanNumeral)
        {
            var numeral = romanNumeral.Select(c => c.ToString()).ToArray();

            var previousLetter = string.Empty;
            var sum = 0;
            for (var index = 0; index < numeral.Length; index++)
            {
                var currentLetter = numeral[index];
                var nextLetter = index == numeral.Length - 1 
                    ? string.Empty 
                    : numeral[index + 1];

                switch (currentLetter)
                {
                    case One:
                        if (nextLetter == Five || nextLetter == Ten)
                        {
                            if (previousLetter == One || previousLetter == Five)
                            {
                                throw new InvalidRomanNumeralException();
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (nextLetter == One || string.IsNullOrWhiteSpace(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case Five:
                        if (nextLetter == One || string.IsNullOrWhiteSpace(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case Ten:
                        if (nextLetter == Fifty || nextLetter == OneHundred)
                        {
                            if (previousLetter == Ten)
                            {
                                throw new InvalidRomanNumeralException();
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (string.IsNullOrWhiteSpace(nextLetter) || new [] {Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case Fifty:
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] { Ten, Five, One }.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case OneHundred:
                        if (nextLetter == FiveHundred || nextLetter == OneThousand)
                        {
                            if (previousLetter == OneHundred)
                            {
                                throw new InvalidRomanNumeralException();
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (string.IsNullOrWhiteSpace(nextLetter) || new[] {OneHundred, Fifty, Ten, Five, One }.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case FiveHundred:
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] { OneHundred, Ten, Five, One }.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    case OneThousand:
                        if (previousLetter == OneHundred && nextLetter == OneHundred)
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        if (!string.IsNullOrWhiteSpace(previousLetter) && !new [] {OneThousand, OneHundred}.Contains(previousLetter))
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] {OneThousand, FiveHundred, OneHundred, Fifty, Ten, Five, One }.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException();
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }


                previousLetter = currentLetter;
            }

            return sum;
        }

        private static bool ContainsFourInARow(string romanNumeral)
        {
            var last = string.Empty;
            foreach(var letter in romanNumeral.Select(c => c.ToString()))
            {
                if (last.Contains(letter))
                {
                    last += letter;
                }
                else
                {
                    last = letter;
                }
                if (last.Length > 3)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ContainsValidLetters(string romanNumeral)
        {
            if (string.IsNullOrWhiteSpace(romanNumeral))
            {
                return false;
            }

            return romanNumeral
                .Select(c => c.ToString())
                .All(s => ValidStrings.Contains(s));

        }

        private static int ParseSingleLetter(string upper)
        {
            switch (upper)
            {
                case One:
                    return 1;
                case Five:
                    return 5;
                case Ten:
                    return 10;
                case Fifty:
                    return 50;
                case OneHundred:
                    return 100;
                case FiveHundred:
                    return 500;
                case OneThousand:
                    return 1000;
                default:
                    throw new InvalidRomanNumeralException();
            }
        }
    }
}
