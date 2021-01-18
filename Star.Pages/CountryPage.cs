using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class CountryPage : ProDinnerPage
    {
        public CountryPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public CountryPage CountryActionOne(string countrMsg)
        {
            Test.DataCache<SimplePOCO>().Announcement = countrMsg;
            TestContext.WriteLine("Doing country action one");
            return this;
        }

        public CountryPage CountryActionTwo(string countrMsg)
        {
            Assert.AreEqual(countrMsg, Test.DataCache<SimplePOCO>().Announcement);
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
