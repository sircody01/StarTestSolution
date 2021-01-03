using NUnit.Framework;
using Star.Core;

namespace Star.Pages
{
    public partial class DinnerPage : ProDinnerPage
    {
        public DinnerPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public DinnerPage DinnerActionOne()
        {
            TestContext.WriteLine("Doing dinner action one");
            return this;
        }

        public DinnerPage DinnerActionTwo()
        {
            TestContext.WriteLine("Doing dinner action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Dinner", "Did not land on the dinner page");
            return true;
        }
    }
}
