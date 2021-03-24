# This is the `Software Test Automation Revealed aka S.T.A.R.` automation testing solution. It demonstrates a number of best practices in test automation with heavy emphasis on UI and Selenium based web tests. I use it in my weekly demonstrations posted on [my YouTube channel called S.T.A.R.](https://www.youtube.com/channel/UCKTRl3jf0PsKT0zQJdKWbqg)

## Version 5.0 adds fetching test input data from a database. In this project it uses MongoDB as the database of choice. You could use any database if you change how the database connection is made and reading the data from the database. The MongoDB NuGet's have already been added to the solution. Repo tags `v5.0` and `Episode-5` go with [episode 5](https://youtu.be/oexX1WMIClA) of my YouTube channel.

Here is an example test reading and consuming data from the database:
```csharp
    public class DinnerPageTests : BaseStarTests
    {
        [Test]
        public void CreateDinner()
        {
            var data = TestDataProvider.GetAll<DinnerModel>("STAR", "ProDinner", "Dinner");

            ProDinner
                .NavigateToDinners()
                .CreateDinner(data)
                .VerifyDinnerAdded(data);
        }
    }
```

## The URL to MongoDB Community edition can be found [here:](https://www.mongodb.com/try/download/community)

## The URL to MongoDB Atlas can be found [here:](https://www.mongodb.com/cloud/atlas)

## Version 4.0 adds logging via Apache log4net and PostSharp. To take advantage of this you MUST [download and install PostSharp](https://www.postsharp.net/download). During the install you will be asked which license option you wish to use. Please select the "Use PostSharp Community" option. This option is free to use and meets our logging needs perfectly. The log4net and PostSharp NuGet's have already been added to the solution. Repo tags `v4.0` and `Episode-4` go with [episode 4](https://youtu.be/YGP7TXVVuRI) of my YouTube channel. It demonstrates how to add automatic detailed logging by combining and taking advantage of Apache log4net and PostSharp community edition. Here's an example log of a successful test:
```
2021-02-14 06:55:35,342 [INFO] - Log file stored at: D:\S.T.A.R\StarTestSolution\Star.Tests\bin\Debug\netcoreapp3.1\TestResults\2021-02-14 06-55-35\Star.Tests.ProDinnerTests.Test1('One It's home time','One It's meal time','One It's chef time','One It's country time','One It's dinner time','One It's feedback time').log
2021-02-14 06:55:50,862 [INFO] - Running tests on Chrome version 88.0
2021-02-14 06:55:50,866 [INFO] - Completed Selenium WebDriver initialization. URL at start of test: https://prodinner.aspnetawesome.com/.
2021-02-14 06:55:54,003 [INFO] - >>> NavigateToHome ( [<no args>] )
2021-02-14 06:55:55,342 [INFO] - <<< NavigateToHome returned [Star.Pages.HomePage]
2021-02-14 06:55:55,342 [INFO] - >>> HomeActionOne ( [One It's home time] )
2021-02-14 06:55:55,342 [INFO] - Doing home action one
2021-02-14 06:55:55,342 [INFO] -   >>> InternalMethodOne ( [<no args>] )
2021-02-14 06:55:55,342 [INFO] -     >>> InternalMethodTwo ( [<no args>] )
2021-02-14 06:55:55,342 [INFO] -     <<< InternalMethodTwo returned [<null>]
2021-02-14 06:55:55,342 [INFO] -   <<< InternalMethodOne returned [<null>]
2021-02-14 06:55:55,342 [INFO] - <<< HomeActionOne returned [Star.Pages.HomePage]
2021-02-14 06:55:55,342 [INFO] - >>> HomeActionTwo ( [One It's home time] )
2021-02-14 06:55:55,342 [INFO] - Doing home action two
.
.
.
2021-02-14 06:56:00,337 [INFO] - >>> NavigateToFeedback ( [<no args>] )
2021-02-14 06:56:01,708 [INFO] - <<< NavigateToFeedback returned [Star.Pages.FeedbackPage]
2021-02-14 06:56:01,708 [INFO] - >>> FeedbackActionOne ( [One It's feedback time] )
2021-02-14 06:56:01,708 [INFO] - Doing feedback action one
2021-02-14 06:56:01,708 [INFO] - <<< FeedbackActionOne returned [Star.Pages.FeedbackPage]
2021-02-14 06:56:01,708 [INFO] - >>> FeedbackActionTwo ( [One It's feedback time] )
2021-02-14 06:56:01,708 [INFO] - Doing feedback action two
2021-02-14 06:56:01,708 [INFO] - <<< FeedbackActionTwo returned [Star.Pages.FeedbackPage]
2021-02-14 06:56:01,708 [INFO] - Test result: Passed
2021-02-14 06:56:01,708 [INFO] - Test duration: 00:00:23.2
```
Here's an example log of a failed test. Note the complete stack track and the exact line of our test code that threw the error `D:\S.T.A.R\StarTestSolution\Star.Pages\HomePage.cs:line 64`:
```
2021-02-14 06:42:15,076 [INFO] - Log file stored at: D:\S.T.A.R\StarTestSolution\Star.Tests\bin\Debug\netcoreapp3.1\Star.Tests.ProDinnerTests.ExceptionDemonstrationTest.log
2021-02-14 06:42:20,632 [INFO] - URL at start of test: https://prodinner.aspnetawesome.com/
2021-02-14 06:42:20,632 [INFO] - Running tests on Chrome version 88.0
2021-02-14 06:42:20,635 [INFO] - Completed Selenium WebDriver initialization. URL at start of test: https://prodinner.aspnetawesome.com/.
2021-02-14 06:42:23,737 [INFO] - >>> NavigateToHome ( [<no args>] )
2021-02-14 06:42:24,514 [INFO] - <<< NavigateToHome returned [Star.Pages.HomePage]
2021-02-14 06:42:24,515 [INFO] - >>> ThrowArtificialExceptionForDemo ( [<no args>] )
2021-02-14 06:42:25,686 [ERROR] - [Star.Pages.HomePage] !! NoSuchElementException in [ThrowArtificialExceptionForDemo]:OpenQA.Selenium.NoSuchElementException: no such element: Unable to locate element: {"method":"css selector","selector":"#InvalidId"}  (Session info: chrome=88.0.4324.150)
   at OpenQA.Selenium.Remote.RemoteWebDriver.UnpackAndThrowOnError(Response errorResponse)
   at OpenQA.Selenium.Remote.RemoteWebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.Remote.RemoteWebDriver.FindElement(String mechanism, String value)
   at OpenQA.Selenium.Remote.RemoteWebDriver.FindElementByCssSelector(String cssSelector)
   at OpenQA.Selenium.By.<>c__DisplayClass23_0.<CssSelector>b__0(ISearchContext context)
   at OpenQA.Selenium.By.FindElement(ISearchContext context)
   at OpenQA.Selenium.Remote.RemoteWebDriver.FindElement(By by)
   at Star.Pages.HomePage.ThrowArtificialExceptionForDemo() in D:\S.T.A.R\StarTestSolution\Star.Pages\HomePage.cs:line 64
2021-02-14 06:42:25,728 [INFO] - Test result: Failed:Error
2021-02-14 06:42:25,728 [INFO] - Test duration: 00:00:07.2
```
Optionally you can add explicit logging statements such as the `Test.Logger.Info` call demonstrated here:
```csharp
        public HomePage HomeActionTwo(string homeMsg)
        {
            Assert.AreEqual(homeMsg, Test.DataCache<SimplePOCO>().Announcement);
            Test.Logger.Info("Doing home action two");
            return this;
        }
```
## Repo tags `v3.0` and `Episode-3` go with [episode 3](https://youtu.be/zIkoF8iMMDs) of my YouTube channel. It demonstrates how to take advantage of `Fluent Assertions`. Here is an example of a fluent assertion:
```csharp
        public HomePage VerifyCurrentPageSize()
        {
            DinnersGridRows.Count.Should().Be(Test.DataCache<HomePageModel>().ExpectedPageSize);
            return this;
        }
```
## Repo tags `v2.0` and `Episode-2` go with [episode 2](https://youtu.be/2uZgQpErNdQ) of my YouTube channel. It demonstrates the concept of a `Global Data Cache` and how this is advantageous in web tests. Here is an example of how to use the global data cache. Note the `Test.DataCache<>` lines of code:
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
## Repo tags `v1.0` and `Episode-1` go with [episode 1](https://youtu.be/Q1H9qd0SE6Q) of my YouTube channel. It demonstrates the advantages of fluent style test coding. Here is an example of a fluent style test:
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
