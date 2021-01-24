using System;
using System.Collections.Generic;
using System.Linq;
using Star.Core.Types;

namespace Star.Core.I18n
{
    public static class I18N
    {
        public static readonly IList<Country> Countries = new List<Country>();

        public static readonly Country Argentina = new Country("Argentina", CountryCode.Argentina, Region.LatinAmerica,
            CultureName.Argentina);
        public static readonly Country Canada = new Country("Canada", CountryCode.Canada, Region.Canada, CultureName.Canada);
        public static readonly Country Colombia = new Country("Colombia", CountryCode.Colombia, Region.LatinAmerica,
            CultureName.Colombia);
        public static readonly Country Chile = new Country("Chile", CountryCode.Chile, Region.LatinAmerica, CultureName.Chile);
        public static readonly Country France = new Country("France", CountryCode.France, Region.Emea, CultureName.France);
        public static readonly Country FrenchCanada = new Country("Canada", CountryCode.Canada, Region.FrenchCanada,
            CultureName.FrenchCanada);
        public static readonly Country Germany = new Country("Deutschland", CountryCode.Germany, Region.Emea, CultureName.Germany);
        public static readonly Country Ireland = new Country("Ireland", CountryCode.Ireland, Region.Emea, CultureName.Ireland);
        public static readonly Country Italy = new Country("Italia", CountryCode.Italy, Region.Emea, CultureName.Italy);
        public static readonly Country Mexico = new Country("México", CountryCode.Mexico, Region.LatinAmerica, CultureName.Mexico);
        public static readonly Country Peru = new Country("Peru", CountryCode.Peru, Region.LatinAmerica, CultureName.Peru);
        public static readonly Country Spain = new Country("España", CountryCode.Spain, Region.Emea, CultureName.Spain);
        public static readonly Country Slovakia = new Country("Slovakia", CountryCode.Slovakia, Region.Emea, CultureName.Slovakia);
        public static readonly Country UnitedKingdom = new Country("United Kingdom", CountryCode.UnitedKingdom, Region.Emea,
            CultureName.UnitedKingdom);
        public static readonly Country UnitedStates = new Country("United States", CountryCode.UnitedStates, Region.UnitedStates,
            CultureName.UnitedStates);
        public static readonly Country Brazil = new Country("Brasil", CountryCode.Brazil, Region.LatinAmerica,
            CultureName.Brazil);
        public static readonly Country Australia = new Country("Australia", CountryCode.Australia, Region.APJ,
           CultureName.Australia);
        public static readonly Country NewZealand = new Country("New Zealand", CountryCode.NewZealand, Region.APJ,
           CultureName.NewZealand);
        public static readonly Country Japan = new Country("Japan", CountryCode.Japan, Region.APJ,
           CultureName.Japan);
        public static readonly Country China = new Country("China", CountryCode.China, Region.APJ,
           CultureName.China);

        static I18N()
        {
            Countries.Add(Argentina);
            Countries.Add(Canada);
            Countries.Add(Colombia);
            Countries.Add(Chile);
            Countries.Add(France);
            Countries.Add(FrenchCanada);
            Countries.Add(Germany);
            Countries.Add(Ireland);
            Countries.Add(Italy);
            Countries.Add(Mexico);
            Countries.Add(Peru);
            Countries.Add(Spain);
            Countries.Add(UnitedKingdom);
            Countries.Add(UnitedStates);
            Countries.Add(Slovakia);
            Countries.Add(Brazil);
            Countries.Add(Australia);
            Countries.Add(NewZealand);
            Countries.Add(Japan);
            Countries.Add(China);
        }

        public static Country GetCountryByCode(string code)
        {
            var c = Countries.FirstOrDefault(country => country.Code == code);

            if (c != null)
                return c;

            throw new ArgumentException($"Unknown country '{code}'");
        }
    }
}
