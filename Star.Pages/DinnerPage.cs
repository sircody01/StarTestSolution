using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class DinnerPage : ProDinnerPage
    {
        public DinnerPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public DinnerPage DinnerActionOne(string dinnerMsg)
        {
            TestContext.WriteLine("Doing dinner action one");
            Test.DataCache<SimplePOCO>().Announcement = dinnerMsg;
            return this;
        }

        public DinnerPage DinnerActionTwo(string dinnerMsg)
        {
            TestContext.WriteLine("Doing dinner action two");
            Assert.AreEqual(dinnerMsg, Test.DataCache<SimplePOCO>().Announcement);
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Dinner", "Did not land on the dinner page");
            return true;
        }
    }
}
