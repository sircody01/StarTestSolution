using Star.Core;
using Star.Core.Page;

namespace Star.Pages.Navigation
{
    public abstract class ProDinnerNavigationProvider : BaseNavigationProvider
    {
        #region Constructors

        protected ProDinnerNavigationProvider(ref IWebTest test)
            : base(ref test)
        {
        }

        #endregion

        #region Properties


        #endregion

        #region Constants


        #endregion

        #region Public navigate methods

        public HomePage NavigateToHome()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/";
            return new HomePage(ref Test);
        }

        public MealsPage NavigateToMeals()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/Meal";
            return new MealsPage(ref Test);
        }

        public ChefPage NavigateToChefs()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/Chef";
            return new ChefPage(ref Test);
        }

        public CountryPage NavigateToCountries()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/Country";
            return new CountryPage(ref Test);
        }

        public DinnerPage NavigateToDinners()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/Dinner";
            return new DinnerPage(ref Test);
        }

        public FeedbackPage NavigateToFeedback()
        {
            WebDriver.Url = "https://prodinner.aspnetawesome.com/Feedback";
            return new FeedbackPage(ref Test);
        }

        #endregion
    }
}
