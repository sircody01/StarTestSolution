namespace Star.Core.Types
{
    public class Country
    {
        public string Code;
        public string CultureName;
        public string Name;
        public string Region;

        public Country(string name, string code, string region, string cultureName)
        {
            Name = name;
            Code = code;
            Region = region;
            CultureName = cultureName;
        }
    }
}
