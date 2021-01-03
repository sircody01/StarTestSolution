using System;
using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Star.Core.WebDriver;
using UAParser;

namespace Star.Core
{
    [TestFixture]
    public abstract class BaseWebTest : WebDriverInit, IWebTest
    {
        #region Properties

        public IWebDriver TestWebDriver { get; private set; }
        public string Scheme { get; protected set; }
        public string Host { get; protected set; }
        public string Country { get; protected set; }
        public object TestStartTime { get; private set; }

        private object _targetBrowser;

        #endregion

        #region Setup & Cleanup

        protected void BaseTestInitialize(string uri, WebDriverType driverToUse)
        {
            TestWebDriver = InitializeWebDriver(driverToUse);
            TestWebDriver.Manage().Timeouts().ImplicitWait = ApplicationSettings.ElementLoadTimeSpan;
            TestStartTime = DateTime.Now;
            _targetBrowser = GetBrowserInformation(TestWebDriver);
            TestWebDriver.Navigate().GoToUrl(uri);
            VerifyWebDriverIsConnected(uri, driverToUse);

            // Set a cookie which can be used by web analytics to filter out web traffic coming from our test automation.
            // We have to set the cookie AFTER navigating to the home page because Selenium can only set cookies in the domain of the currently loaded page.
            TestWebDriver.Manage()
                .Cookies.AddCookie(new Cookie("StarTestAutomation", "Yes", "aspnetawesome.com", "/", DateTime.Now.AddMinutes(120)));
            if (ApplicationSettings.ScreenResolution == Size.Empty)
            {
                TestWebDriver.Manage().Window.Maximize();
            }
            else
            {
                TestWebDriver.Manage().Window.Position = new Point(0, 0);
                TestWebDriver.Manage().Window.Size = ApplicationSettings.ScreenResolution;
            }
        }

        [TearDown]
        public void TestCleanup()
        {
            var didTestFail =
                TestContext.CurrentContext.Result.Outcome == ResultState.Failure ||
                TestContext.CurrentContext.Result.Outcome == ResultState.Error ||
                TestContext.CurrentContext.Result.Outcome == ResultState.Inconclusive;
            try
            {
                if (TestWebDriver != null)
                {
                    TestWebDriver.Close();
                    TestWebDriver.Quit();
                    TestWebDriver = null;
                }
            }
            catch (WebDriverException ex)
            {
                TestContext.Out.WriteLine(ex.Message);

                //Hide WebDriver exceptions here after the test has finished.
                TestWebDriver = null;
            }
            finally
            {
                // This must be done AFTER closing the TestWebDriver
                // If the test failed, capture important test assets created on Sauce Labs
                if (didTestFail)
                {
                }
            }
        }

        #endregion

        #region Private Methods

        private void VerifyWebDriverIsConnected(string uri, WebDriverType driverToUse)
        {
            try
            {
                // Read the URL from the TestWebDriver to verify that the instance is still responding.
                TestContext.WriteLine($"URL at start of test: {TestWebDriver.Url}");
            }
            catch (WebDriverException ex)
            {
                TestContext.WriteLine("***** WARNING *****");
                TestContext.WriteLine(ex.ToString());
                TestContext.WriteLine("Lost contact with the WebDriver. This test will not continue.");
                TestContext.WriteLine(string.Empty);
                TestContext.WriteLine("***** END WARNING *****");
                Assert.Inconclusive("The test did not execute correctly. Please see the test output and retry.");
            }
        }

        private static Browser GetBrowserInformation(IWebDriver driver)
        {
            var parser = Parser.GetDefault();
            var uaString = driver.ExecuteJavaScript<string>("return navigator.userAgent");
            var parsedUa = parser.Parse(uaString);
            var ua = parsedUa.UA;

            return new Browser
            {
                Name = ua.Family,
                Version = $"{ua.Major}.{ua.Minor}"
            };
        }

        #endregion
    }
}
