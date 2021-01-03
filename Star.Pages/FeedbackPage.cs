using NUnit.Framework;
using Star.Core;

namespace Star.Pages
{
    public partial class FeedbackPage : ProDinnerPage
    {
        public FeedbackPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public FeedbackPage FeedbackActionOne()
        {
            TestContext.WriteLine("Doing feedback action one");
            return this;
        }

        public FeedbackPage FeedbackActionTwo()
        {
            TestContext.WriteLine("Doing feedback action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Feedback", "Did not land on the feedback page");
            return true;
        }
    }
}
