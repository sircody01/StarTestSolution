using System;
using System.Collections.Concurrent;
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
        #region Embedded private classes

        /// <summary>
        /// This is a private POCO used for storing each tests Selenium WebDriver in the global data cache.
        /// </summary>
        private class CachedTestsWebDriver
        {
            public IWebDriver Driver { get; set; }
        }

        #endregion

        #region Class Variables

        // This is a dictionary of test data cache dictionaries. There is one dictionary per test actively running.
        // The outer, or parent dictionary uses NUnit's unique test ID as the key.
        // The dictionary for each test uses the POCO's type as the key.
        private static ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> _runningTestsDataDictionaries;

        #endregion

        #region Properties

        /// <summary>
        /// The Selenium webdriver used by the rest of the test. There's one webdriver, one browser per running test.
        /// </summary>
        public IWebDriver TestWebDriver
        {
            get
            {
                var d = DataCache<CachedTestsWebDriver>();
                return d.Driver;
            }
            private set
            {
                DataCache<CachedTestsWebDriver>().Driver = value;
            }
        }
        public string Scheme { get; protected set; }
        public string Host { get; protected set; }
        public string Country { get; protected set; }
        public DateTime TestStartTime { get; private set; }

        private object _targetBrowser;

        #endregion

        #region Setup & Cleanup

        [OneTimeSetUp]
        public void Init()
        {
            _runningTestsDataDictionaries = new ConcurrentDictionary<string, ConcurrentDictionary<Type, object>>();
        }

        protected void BaseTestInitialize(string uri, WebDriverType driverToUse)
        {
            TestWebDriver = InitializeWebDriver(driverToUse);
            TestWebDriver.Manage().Timeouts().ImplicitWait = ApplicationSettings.ElementLoadTimeSpan;
            TestStartTime = DateTime.Now;
            _targetBrowser = GetBrowserInformation(TestWebDriver);
            TestWebDriver.Navigate().GoToUrl(uri);
            VerifyWebDriverIsConnected();

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

        #region Public Methods

        public T DataCache<T>() where T : new()
        {
            // Try to get the tests data cache ditionary
            if (_runningTestsDataDictionaries.TryGetValue(TestContext.CurrentContext.Test.ID, out ConcurrentDictionary<Type, object> testsDataCache))
            {
                // We succeeded in getting the tests data cache dictionary. Now try to get the POCO the test is asking for by the object type.
                if (testsDataCache.TryGetValue(typeof(T), out object o))
                {
                    // We succeeded in getting the POCO the test is looking for. Return it to the test.
                    return (T)o;
                }
            }
            else
            {
                // There is no data cache dictionary for the currently executing test so let's create one and add it.
                testsDataCache = new ConcurrentDictionary<Type, object>();
                _runningTestsDataDictionaries.TryAdd(TestContext.CurrentContext.Test.ID, testsDataCache);
            }

            // The POCO the test is looking for does not exist so let's create one, add it to the tests data dictionary and return it to the test.
            T poco = new T();
            testsDataCache.TryAdd(typeof(T), poco);
            return poco;
        }

        #endregion

        #region Private Methods

        private void VerifyWebDriverIsConnected()
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
