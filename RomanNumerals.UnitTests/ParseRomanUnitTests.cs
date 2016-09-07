using System;
using System.Linq;
using NUnit.Framework;

namespace RomanNumerals.UnitTests
{
    [TestFixture]
    public class ParseRomanUnitTests
    {
        [Test]
        [TestCase("I", ExpectedResult = 1)]
        [TestCase("i", ExpectedResult = 1)]
        [TestCase("V", ExpectedResult = 5)]
        [TestCase("v", ExpectedResult = 5)]
        [TestCase("X", ExpectedResult = 10)]
        [TestCase("x", ExpectedResult = 10)]
        [TestCase("L", ExpectedResult = 50)]
        [TestCase("l", ExpectedResult = 50)]
        [TestCase("C", ExpectedResult = 100)]
        [TestCase("c", ExpectedResult = 100)]
        [TestCase("D", ExpectedResult = 500)]
        [TestCase("d", ExpectedResult = 500)]
        [TestCase("M", ExpectedResult = 1000)]
        [TestCase("m", ExpectedResult = 1000)]
        public int SingleLetterConversion(string letter)
        {
            return RomanNumerals.ParseRoman(letter);
        }

        [Test]
        [TestCase("II", ExpectedResult = 2)]
        [TestCase("III", ExpectedResult = 3)]
        [TestCase("XX", ExpectedResult = 20)]
        [TestCase("XXX", ExpectedResult = 30)]
        [TestCase("CC", ExpectedResult = 200)]
        [TestCase("CCC", ExpectedResult = 300)]
        [TestCase("MM", ExpectedResult = 2000)]
        [TestCase("MMM", ExpectedResult = 3000)]
        public int RepeatLetterConversion(string letters)
        {
            return RomanNumerals.ParseRoman(letters);
        }

        [Test]
        [TestCaseSource(nameof(ValidCharacters))]
        public void ValidLettersOnly(string letter)
        {
            Assert.That(() => RomanNumerals.ParseRoman(letter),
                Throws.Nothing);
        }

        [Test]
        [TestCaseSource(nameof(TestBadCharacters))]
        public void BadLettersThrow(string letter)
        {
            Assert.That(() => RomanNumerals.ParseRoman(letter),
                Throws.TypeOf<InvalidRomanNumeralException>());
        }

        [Test]
        public void NullInputThrows()
        {
            Assert.That(() => RomanNumerals.ParseRoman(null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void EmptyInputThrows()
        {
            Assert.That(() => RomanNumerals.ParseRoman(string.Empty),
                Throws.TypeOf<InvalidRomanNumeralException>());
        }

        private static string[] ValidCharacters { get; } =
            {"I", "i", "V", "v", "X", "x", "L", "l", "C", "c", "D", "d", "M", "m"};

        private static string[] TestBadCharacters { get; } = 
            Enumerable.Range(0x20, 0x7e - 0x20)
                      .Select(Convert.ToChar)
                      .Where(c => !char.IsControl(c))
                      .Select(c => c.ToString())
                      .Where(c => !ValidCharacters.Contains(c))
                      .ToArray();

        [Test]
        [TestCase("IIII")]
        [TestCase("XXXX")]
        [TestCase("CCCC")]
        [TestCase("MMMM")]
        public void FourInARowIsInvalid(string letters)
        {
            Assert.That(() => RomanNumerals.ParseRoman(letters),
                Throws.TypeOf<InvalidRomanNumeralException>());
        }

        [Test]
        [TestCase("IV", ExpectedResult = 4)]
        [TestCase("IX", ExpectedResult = 9)]
        [TestCase("XL", ExpectedResult = 40)]
        [TestCase("XC", ExpectedResult = 90)]
        [TestCase("CD", ExpectedResult = 400)]
        [TestCase("CM", ExpectedResult = 900)]
        public int SubtractiveBaseConversion(string letters)
        {
            return RomanNumerals.ParseRoman(letters);
        }

        [Test]
        [TestCase("IIV")]
        [TestCase("IIIV")]
        [TestCase("IL")]
        [TestCase("IC")]
        [TestCase("ID")]
        [TestCase("IM")]
        [TestCase("VX")]
        [TestCase("VL")]
        [TestCase("VC")]
        [TestCase("VD")]
        [TestCase("VM")]
        [TestCase("VV")]
        [TestCase("XXL")]
        [TestCase("XXXL")]
        [TestCase("XXC")]
        [TestCase("XXXC")]
        [TestCase("XD")]
        [TestCase("XM")]
        [TestCase("LL")]
        [TestCase("LC")]
        [TestCase("LD")]
        [TestCase("LM")]
        [TestCase("CCD")]
        [TestCase("CCCD")]
        [TestCase("CCM")]
        [TestCase("CCCM")]
        [TestCase("CMC")]
        [TestCase("DD")]
        [TestCase("DM")]
        [TestCase("MCCM")]
        [TestCase("VIX")]
        [TestCase("XVL")]
        [TestCase("XDL")]
        [TestCase("XIL")]
        public void InvalidNumerals(string letters)
        {
            Assert.That(() => RomanNumerals.ParseRoman(letters),
                Throws.TypeOf<InvalidRomanNumeralException>());
        }

        [Test]
        [TestCase("MCMLIV", ExpectedResult = 1954)]
        [TestCase("MCMXC", ExpectedResult = 1990)]
        [TestCase("MMXIV", ExpectedResult = 2014)]
        [TestCase("MDCCCXCIII", ExpectedResult = 1893)]
        [TestCase("MM", ExpectedResult = 2000)]
        [TestCase("MCMLXXVIII", ExpectedResult = 1978)]
        public int TestCases(string letters)
        {
            return RomanNumerals.ParseRoman(letters);
        }
    }
}
