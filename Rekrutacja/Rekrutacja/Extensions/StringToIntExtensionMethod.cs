using System;
using System.Linq;

namespace Rekrutacja.Extensions
{
    public static class StringToIntExtensionMethod
    {
        //'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        private readonly static char[] validNumberDecipalSeparators = { ',', '.' };
        private readonly static char[] validOtherNumberCharacters = { ' ', '-' };

        /// <summary>
        /// Konwertuje liczbę zapisaną w postaci tekstu na int, pomijając jej wartość ułamkową
        /// </summary>
        /// <param name="str">Liczba zapisana w postaci tekstu</param>
        /// <returns>Wartoć całkowita liczby zapisanej w postaci tekstu, 0 w przypadku istnienia niedozwolonych znaków w tekcie - innych niż cyfry, spacja, kropka i przecinek</returns>
        public static int ToInt(this string str)
        {
            if (!ValidateInputString(str))
            {
                throw new ArgumentException("Nieprawidłowe znaki wewnątrz ciągu wejściowego.");
            }

            int indexOfDecimalSeparator = str.IndexOfAny(validNumberDecipalSeparators);

            string integralPartOfNumber;
            if (indexOfDecimalSeparator != -1)
            {
                integralPartOfNumber = str.Substring(0, indexOfDecimalSeparator);
            }
            else
            {
                integralPartOfNumber = str;
            }

            bool isNumberNegative = false;
            if (integralPartOfNumber[0] == '-')
            {
                isNumberNegative = true;
                integralPartOfNumber = integralPartOfNumber.Substring(1);
            }

            integralPartOfNumber = integralPartOfNumber.Replace(" ", string.Empty);

            int returnValue = 0;
            for (int i = 0; i < integralPartOfNumber.Length; i++)
            {
                if (isNumberNegative)
                {
                    returnValue = checked(returnValue - integralPartOfNumber[i].ToInt() * (int)Math.Pow(10, integralPartOfNumber.Length - i - 1));
                }
                else
                {
                    returnValue = checked(returnValue + integralPartOfNumber[i].ToInt() * (int)Math.Pow(10, integralPartOfNumber.Length - i - 1));
                }
            }

            return returnValue;
        }

        public static int ToInt(this char c)
        {
            return c - '0';
        }

        private static bool ValidateInputString(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;

            bool containsOnlyValidCharacters = str.All(x => char.IsDigit(x) || validNumberDecipalSeparators.Contains(x) || validOtherNumberCharacters.Contains(x));
            bool containsAtLeastOneDigit = str.Count(x => char.IsDigit(x)) >= 1;
            bool containsNoMoreThanOneDecimalSeparator = str.Count(x => x == ',' || x == '.') <= 1;
            bool containsOptionalNegativeSignAsFirstCharacter = str.IndexOf('-') <= 0;
            bool containsNoMoreThanOneNegativeSigns = str.Count(x => x == '-') <= 1;
            bool startsWithDigit = char.IsDigit(str[0]);
            bool startsWithNegativeSignAndThenDigit = str.Length > 1 && str[0] == '-' && char.IsDigit(str[1]);

            return containsOnlyValidCharacters
                && containsAtLeastOneDigit
                && containsNoMoreThanOneDecimalSeparator
                && containsOptionalNegativeSignAsFirstCharacter
                && containsNoMoreThanOneNegativeSigns
                && (startsWithDigit || startsWithNegativeSignAndThenDigit);
        }
    }
}
