using NUnit.Framework;
using Star.Core;

namespace Star.Pages
{
    public partial class HomePage : ProDinnerPage
    {
        public HomePage(ref IWebTest test)
            : base(ref test)
        {

        }

        public HomePage HomeActionOne()
        {
            TestContext.WriteLine("Doing home action one");
            return this;
        }

        public HomePage HomeActionTwo()
        {
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
