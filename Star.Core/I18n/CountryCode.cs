using System.Globalization;

namespace Star.Core.I18n
{
    public static class CountryCode
    {
        public const string Argentina = "ar";
        public const string Canada = "ca";
        public const string Colombia = "co";
        public const string Chile = "cl";
        public const string France = "fr";
        public const string Germany = "de";
        public const string Ireland = "ie";
        public const string Italy = "ia";
        public const string Mexico = "mx";
        public const string Peru = "pe";
        public const string Spain = "es";
        public const string Slovakia = "sk";
        public const string UnitedKingdom = "uk";
        public const string UnitedStates = "us";
        public const string Brazil = "br";
        public const string NewZealand = "nz";
        public const string Japan = "jp";
        public const string China = "cn";
        public const string Australia = "au";

        public static string GetCurrencySymbol(string countryCode)
        {
            var info = CultureInfo.GetCultureInfo(I18N.GetCountryByCode(countryCode).CultureName);
            return info.NumberFormat.CurrencySymbol;
        }
    }
}
