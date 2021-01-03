using NUnit.Framework;
using Star.Core;

namespace Star.Pages
{
    public partial class ChefPage : ProDinnerPage
    {
        public ChefPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public ChefPage ChefActionOne()
        {
            TestContext.WriteLine("Doing chef action one");
            return this;
        }

        public ChefPage ChefActionTwo()
        {
            TestContext.WriteLine("Doing chef action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Chef", "Did not land on the chef page");
            return true;
        }
    }
}
