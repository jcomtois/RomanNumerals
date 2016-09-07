using System;
using System.Linq;
using System.Text;

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

        private static readonly int[] MappedValues = {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};

        public static string ToRoman(int number)
        {
            if (number <= 0 || number >= 4000)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Must be number 1-3999");
            }

            return MapValue(number);
        }

        public static bool TryToRoman(int number, out string result)
        {
            result = default(string);
            try
            {
                result = ToRoman(number);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryParseRoman(string romanNumeral, out int result)
        {
            result = default(int);
            try
            {
                result = ParseRoman(romanNumeral);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
                throw new InvalidRomanNumeralException(romanNumeral);
            }

            if (upper.Length == 1)
            {
                return ParseSingleLetter(upper);
            }

            if (ContainsFourInARow(upper))
            {
                throw new InvalidRomanNumeralException(romanNumeral);
            }

            var value = ParseRomanNumeral(upper);

            if (value > 0)
            {
                return value;
            }

            throw new InvalidOperationException($"'{romanNumeral}' - Should not be able to enter this state");
        }

        private static string MapValue(int number)
        {
            switch (number)
            {
                case 1:
                    return One;
                case 4:
                    return One + Five;
                case 5:
                    return Five;
                case 9:
                    return One + Ten;
                case 10:
                    return Ten;
                case 40:
                    return Ten + Fifty;
                case 50:
                    return Fifty;
                case 90:
                    return Ten + OneHundred;
                case 100:
                    return OneHundred;
                case 400:
                    return OneHundred + FiveHundred;
                case 500:
                    return FiveHundred;
                case 900:
                    return OneHundred + OneThousand;
                case 1000:
                    return OneThousand;
            }

            var sb = new StringBuilder();

            var remainder = number;

            foreach (var value in MappedValues)
            {
                while (remainder >= value)
                {
                    remainder -= value;
                    sb.Append(MapValue(value));
                }
            }

            return sb.ToString();
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
                                throw new InvalidRomanNumeralException(romanNumeral);
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (nextLetter == One || string.IsNullOrWhiteSpace(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case Five:
                        if (nextLetter == One || string.IsNullOrWhiteSpace(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case Ten:
                        if (nextLetter == Fifty || nextLetter == OneHundred)
                        {
                            if (previousLetter == Ten)
                            {
                                throw new InvalidRomanNumeralException(romanNumeral);
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (string.IsNullOrWhiteSpace(nextLetter) || new[] {Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case Fifty:
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] {Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case OneHundred:
                        if (nextLetter == FiveHundred || nextLetter == OneThousand)
                        {
                            if (previousLetter == OneHundred)
                            {
                                throw new InvalidRomanNumeralException(romanNumeral);
                            }
                            sum -= ParseSingleLetter(currentLetter);
                        }
                        else if (string.IsNullOrWhiteSpace(nextLetter) || new[] {OneHundred, Fifty, Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case FiveHundred:
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] {OneHundred, Fifty, Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        break;
                    case OneThousand:
                        if (previousLetter == OneHundred && nextLetter == OneHundred)
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        if (!string.IsNullOrWhiteSpace(previousLetter) && !new[] {OneThousand, OneHundred}.Contains(previousLetter))
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
                        }
                        if (string.IsNullOrWhiteSpace(nextLetter) || new[] {OneThousand, FiveHundred, OneHundred, Fifty, Ten, Five, One}.Contains(nextLetter))
                        {
                            sum += ParseSingleLetter(currentLetter);
                        }
                        else
                        {
                            throw new InvalidRomanNumeralException(romanNumeral);
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
            foreach (var letter in romanNumeral.Select(c => c.ToString()))
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

        private static int ParseSingleLetter(string romanNumeral)
        {
            switch (romanNumeral)
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
                    throw new InvalidRomanNumeralException(romanNumeral);
            }
        }
    }
}