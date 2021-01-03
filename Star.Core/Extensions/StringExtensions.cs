using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dinobots.Core.Extensions
{
    public static class StringExtensions
    {
        public static decimal FindCurrencyIn(this string currencyWithin, CultureInfo cultureInfo)
        {
            return RemoveNonDecimalCharacters(currencyWithin).CurrencyToDecimal(cultureInfo);
        }

        public static string RemoveNonDecimalCharacters(this string currencyWithin)
        {
            return Regex.Replace(currencyWithin, @"[^0-9|.|-]|[\|]", "", RegexOptions.Compiled);
        }

        public static decimal CurrencyToDecimal(this string currencyString, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(currencyString))
            {
                throw new ArgumentException("The parameter cannot be empty string", nameof(currencyString));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }
            return decimal.Parse(currencyString, NumberStyles.Currency, cultureInfo);
        }
    }
}
