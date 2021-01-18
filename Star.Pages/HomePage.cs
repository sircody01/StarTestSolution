using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class HomePage : ProDinnerPage
    {
        public HomePage(ref IWebTest test)
            : base(ref test)
        {

        }

        public HomePage HomeActionOne(string homeMsg)
        {
            Test.DataCache<SimplePOCO>().Announcement = homeMsg;
            TestContext.WriteLine("Doing home action one");
            return this;
        }

        public HomePage HomeActionTwo(string homeMsg)
        {
            Assert.AreEqual(homeMsg, Test.DataCache<SimplePOCO>().Announcement);
            TestContext.WriteLine("Doing home action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/", "Did not land on the home page");
            return true;
        }
    }
}
