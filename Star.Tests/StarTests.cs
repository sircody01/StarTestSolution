using NUnit.Framework;

namespace Star.Tests
{
    [TestFixture]
    public class StarTests : BaseStarTests
    {
        [Test]
        public void Test()
        {
            ProDinner
                .NavigateToHome()
                .HomeActionOne()
                .HomeActionTwo()
                .NavigateToMeals()
                .MealsActionOne()
                .MealsActionTwo()
                .NavigateToChefs()
                .ChefActionOne()
                .ChefActionTwo()
                .NavigateToCountries()
                .CountryActionOne()
                .CountryActionTwo()
                .NavigateToDinners()
                .DinnerActionOne()
                .DinnerActionTwo()
                .NavigateToFeedback()
                .FeedbackActionOne()
                .FeedbackActionTwo();
        }
    }
}
