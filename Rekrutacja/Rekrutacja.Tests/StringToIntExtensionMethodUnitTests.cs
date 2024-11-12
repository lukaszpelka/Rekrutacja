using FluentAssertions;
using NUnit.Framework;
using Rekrutacja.Extensions;
using System;
using System.Linq;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class StringToIntExtensionMethodUnitTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase(",")]
        [TestCase(".")]
        [TestCase("-")]
        [TestCase("abc")]
        [TestCase("-abc")]
        [TestCase("123,123,123")]
        [TestCase("-123,123,123")]
        [TestCase("123.123.123")]
        [TestCase("-123.123.123")]
        [TestCase("123,123.123")]
        [TestCase("-123,123.123")]
        [TestCase("123,123123a")]
        [TestCase("-123,123123a")]
        [TestCase(",123 123 123")]
        [TestCase("-,123 123 123")]
        [TestCase("-123-")]
        [TestCase(" 123")]
        public void StringToIntExtensionMethod_ShouldThrowArgumentException_WhenInputStringIsInvalid(string input)
        {
            Action action = () => _ = input.ToInt();
            action.Should().Throw<ArgumentException>();
        }

        [TestCase("2147483648")]
        [TestCase("-2147483649")]
        [TestCase("2 147 483 648")]
        [TestCase("-2 147 483 649")]
        [TestCase("2147483648.123")]
        [TestCase("-2147483649.123")]
        [TestCase("2147483648,123")]
        [TestCase("-2147483649,123")]
        [TestCase("2 147 483 648.123")]
        [TestCase("-2 147 483 649.123")]
        [TestCase("2 147 483 648,123")]
        [TestCase("-2 147 483 649,123")]
        public void StringToIntExtensionMethod_ShouldThrowOverflowException_WhenInputStringIsTooBigForInteger(string input)
        {
            Action action = () => _ = input.ToInt();
            action.Should().Throw<OverflowException>();
        }

        [TestCase("0", 0)]
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("2147483647", 2147483647)]
        [TestCase("-2147483648", -2147483648)]
        [TestCase("1,11", 1)]
        [TestCase("-1,11", -1)]
        [TestCase("1.11", 1)]
        [TestCase("-1.11", -1)]
        [TestCase("123 456,11", 123456)]
        [TestCase("-123 456,11", -123456)]
        [TestCase("123 456.11", 123456)]
        [TestCase("-123 456.11", -123456)]
        public void StringToIntExtensionMethod_ShouldReturnInt_WhenInputStringIsValid(string input, int expected)
        {
            int actual = input.ToInt();
            expected.Should().Be(actual);
        }       
    }
}
