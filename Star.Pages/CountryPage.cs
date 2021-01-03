using NUnit.Framework;
using Star.Core;

namespace Star.Pages
{
    public partial class CountryPage : ProDinnerPage
    {
        public CountryPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public CountryPage CountryActionOne()
        {
            TestContext.WriteLine("Doing country action one");
            return this;
        }

        public CountryPage CountryActionTwo()
        {
            TestContext.WriteLine("Doing country action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Country", "Did not land on the country page");
            return true;
        }
    }
}
