using System;
using System.Linq;
using NUnit.Framework;

namespace RomanNumerals.UnitTests
{
    [TestFixture]
    public class ToRomanUnitTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(4000)]
        [TestCase(4001)]
        public void BadRangeTest(int badInput)
        {
            Assert.That(() => RomanNumerals.ToRoman(badInput),
                Throws.TypeOf<ArgumentOutOfRangeException>());

            string result;
            Assert.That(() => RomanNumerals.TryToRoman(badInput, out result),
                Is.False);
        }

        private static int[] GoodInput { get; } =
            Enumerable.Range(1, 3999).ToArray();

        [Test]
        [TestCaseSource(nameof(GoodInput))]
        public void GoodRangeTest(int goodInput)
        {
            Assert.That(() => RomanNumerals.ToRoman(goodInput),
                Throws.Nothing);
            string result;
            Assert.That(() => RomanNumerals.TryToRoman(goodInput, out result),
                Is.True);
        }

        [Test]
        [TestCase(1, ExpectedResult = "I")]
        [TestCase(5, ExpectedResult = "V")]
        [TestCase(10, ExpectedResult = "X")]
        [TestCase(50, ExpectedResult = "L")]
        [TestCase(100, ExpectedResult = "C")]
        [TestCase(500, ExpectedResult = "D")]
        [TestCase(1000, ExpectedResult = "M")]
        public string SingleNumberTest(int number)
        {
            return RomanNumerals.ToRoman(number);
        }

        [Test]
        [TestCase(4, ExpectedResult = "IV")]
        [TestCase(9, ExpectedResult = "IX")]
        [TestCase(40, ExpectedResult = "XL")]
        [TestCase(90, ExpectedResult = "XC")]
        [TestCase(400, ExpectedResult = "CD")]
        [TestCase(900, ExpectedResult = "CM")]
        public string SpecialNumberTest(int number)
        {
            return RomanNumerals.ToRoman(number);
        }

        [Test]
        [TestCase(2, ExpectedResult = "II")]
        [TestCase(3, ExpectedResult = "III")]
        [TestCase(6, ExpectedResult = "VI")]
        [TestCase(7, ExpectedResult = "VII")]
        [TestCase(8, ExpectedResult = "VIII")]
        [TestCase(11, ExpectedResult = "XI")]
        [TestCase(20, ExpectedResult = "XX")]
        [TestCase(30, ExpectedResult = "XXX")]
        [TestCase(200, ExpectedResult = "CC")]
        [TestCase(300, ExpectedResult = "CCC")]
        [TestCase(2000, ExpectedResult = "MM")]
        [TestCase(3000, ExpectedResult = "MMM")]
        public string TestCases(int number)
        {
            return RomanNumerals.ToRoman(number);
        }

        [Test]
        [TestCaseSource(nameof(GoodInput))]
        public void TestAll(int number)
        {
            var roman = RomanNumerals.ToRoman(number);
            var roundTrip = RomanNumerals.ParseRoman(roman);

            Assert.That(roundTrip, Is.EqualTo(number));
        }
    }
}