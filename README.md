# This is the S.T.A.R. automation testing solution. It demonstrates a number of best practices in test automation with heavy emphasis on UI and Selenium based tests. I use it in my weekly demonstrations of my S.T.A.R. YouTube channel: https://www.youtube.com/channel/UCKTRl3jf0PsKT0zQJdKWbqg
## v1.0 is shown in episode 1. It demonstrates the advantages of fluent style test coding. Here is an example of a fluent style test:
```csharp
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
```
