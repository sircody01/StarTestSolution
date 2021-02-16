using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class MealsPage : ProDinnerPage
    {
        public MealsPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public MealsPage MealsActionOne(string mealsTime)
        {
            Test.DataCache<SimplePOCO>().Announcement = mealsTime;
            Test.Logger.Info("Doing meals action one");
            return this;
        }

        public MealsPage MealsActionTwo(string mealsTime)
        {
            Assert.AreEqual(mealsTime, Test.DataCache<SimplePOCO>().Announcement);
            Test.Logger.Info("Doing meals action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Meal", "Did not land on the meals page");
            return true;
        }
    }
}
