using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class FeedbackPage : ProDinnerPage
    {
        public FeedbackPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public FeedbackPage FeedbackActionOne(string dinnerMsg)
        {
            Test.DataCache<SimplePOCO>().Announcement = dinnerMsg;
            Test.Logger.Info("Doing feedback action one");
            return this;
        }

        public FeedbackPage FeedbackActionTwo(string dinnerMsg)
        {
            Assert.AreEqual(dinnerMsg, Test.DataCache<SimplePOCO>().Announcement);
            Test.Logger.Info("Doing feedback action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Feedback", "Did not land on the feedback page");
            return true;
        }
    }
}
