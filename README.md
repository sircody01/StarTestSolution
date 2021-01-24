# This is the `Software Test Automation Revealed aka S.T.A.R.` automation testing solution. It demonstrates a number of best practices in test automation with heavy emphasis on UI and Selenium based web tests. I use it in my weekly demonstrations posted on my YouTube channel called S.T.A.R.: https://www.youtube.com/channel/UCKTRl3jf0PsKT0zQJdKWbqg
## Repo tags `v1.0` and `Episode-1` go with episode 1 of my YouTube channel. It demonstrates the advantages of fluent style test coding. Here is an example of a fluent style test:
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
## Repo tags `v2.0` and `Episode-2` go with episode 2 of my YouTube channel. It demonstrates the concept of a `Global Data Cache` and how this is advantageous in web tests. Here is an example of how to use the global data cache. Note the `Test.DataCache<>` lines of code:
```csharp
namespace Star.Models
{
    public class SimplePOCO
    {
        public string Announcement { get; set; }
    }
}
```
```csharp
        public HomePage HomeActionOne(string homeMsg)
        {
            // Store homeMsg in the global data cache for this test
            Test.DataCache<SimplePOCO>().Announcement = homeMsg;
            TestContext.WriteLine("Doing home action one");
            return this;
        }

        public HomePage HomeActionTwo(string homeMsg)
        {
            // Verify that the value of the Announcement field stored in the Global Data Cache equals homeMsg
            Assert.AreEqual(homeMsg, Test.DataCache<SimplePOCO>().Announcement);
            TestContext.WriteLine("Doing home action two");
            return this;
        }
```
## Repo tags `v3.0` and `Episode-3` go with episode 3 of my YouTube channel. It demonstrates how to take advantage of `Fluent Assertions`. Here is an example of a fluent assertion:
```csharp
        public HomePage VerifyCurrentPageSize()
        {
            DinnersGridRows.Count.Should().Be(Test.DataCache<HomePageModel>().ExpectedPageSize);
            return this;
        }
```
