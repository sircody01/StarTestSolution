using System;
using System.Globalization;
using System.Linq;
using Star.Core.I18n;
using Star.Core.Types;

namespace Star.Core.Utilities
{
    public static class ConversionHelpers
    {
        public static decimal PriceStringToDecimal(this string priceString, string countryCode)
        {
            var country = I18N.GetCountryByCode(countryCode);
            var culture = new CultureInfo(country.CultureName);

            var canParse = decimal.TryParse(priceString, NumberStyles.Currency, culture, out decimal price);
            if (canParse)
                return price;

            // Fall back to United States
            canParse = decimal.TryParse(priceString, NumberStyles.Currency, new CultureInfo(CultureName.UnitedStates),
                out price);
            if (canParse)
                return price;

            throw new FormatException(
                $"Invalid string '{priceString}' passed in to PriceStringToDecimal. CurrencySymbol: '{culture.NumberFormat.CurrencySymbol}', CurrencyDecimalDigits: '{culture.NumberFormat.CurrencyDecimalDigits}' , CurrencyDecimalSeparator: '{culture.NumberFormat.CurrencyDecimalSeparator}'");
        }

        public static bool IsDateFormatValid(this string date, Country country)
        {
            var culture = new CultureInfo(country.CultureName);

            return DateTime.TryParse(date, culture, DateTimeStyles.None, out DateTime result);
        }

        public static string ExtractNumbers(string text)
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }
    }
}
