using NUnit.Framework;

namespace Star.Tests
{
    [Parallelizable(scope: ParallelScope.All)]
    [TestFixture]
    public class StarTests : BaseStarTests
    {
        [Test]
        public void Test1([Values("One It's home time", "One It's second home time")] string homeMsg,
            [Values("One It's meal time", "One It's second meal time")] string mealsMsg,
            [Values("One It's chef time")] string chefMsg,
            [Values("One It's country time")] string countryMsg,
            [Values("One It's dinner time")] string dinnerMsg,
            [Values("One It's feedback time")] string feedbackMsg)
        {
            ProDinner
                .NavigateToHome()
                .HomeActionOne(homeMsg)
                .HomeActionTwo(homeMsg)
                .NavigateToMeals()
                .MealsActionOne(mealsMsg)
                .MealsActionTwo(mealsMsg)
                .NavigateToChefs()
                .ChefActionOne(chefMsg)
                .ChefActionTwo(chefMsg)
                .NavigateToCountries()
                .CountryActionOne(countryMsg)
                .CountryActionTwo(countryMsg)
                .NavigateToDinners()
                .DinnerActionOne(dinnerMsg)
                .DinnerActionTwo(dinnerMsg)
                .NavigateToFeedback()
                .FeedbackActionOne(feedbackMsg)
                .FeedbackActionTwo(feedbackMsg);
        }

        [Test]
        public void HomePageGridSize()
        {
            ProDinner
                .NavigateToHome()
                .ChangePageSize(5)
                .VerifyCurrentPageSize()
                .ChangePageSize(10)
                .VerifyCurrentPageSize()
                .ChangePageSize(20)
                .VerifyCurrentPageSize()
                .ChangePageSize(50)
                .VerifyCurrentPageSize();
        }
    }
}
