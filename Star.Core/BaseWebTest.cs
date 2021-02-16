using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using log4net;
using log4net.Config;
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
        private class ThreadSafeTestProperties
        {
            public IWebDriver Driver { get; set; }
            public int PostSharpIndent { get; set; }
            public ILog PostSharpLogger { get; set; }
            public ILog Logger { get; set; }
            public DateTime TestStartTime { get; set; }
        }

        #endregion

        #region Class Variables

        // This is a dictionary of test data cache dictionaries. There is one dictionary per test actively running.
        // The outer, or parent dictionary uses NUnit's unique test ID as the key.
        // The dictionary for each test uses the POCO's type as the key.
        private static ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> _runningTestsDataDictionaries;
        private static string _testResultsFolder;

        #endregion

        #region Properties

        public string Scheme { get; protected set; }
        public string Host { get; protected set; }
        public string Country { get; protected set; }

        /// <summary>
        /// Captures when this specific test started. This will be used in test results to log when the test executed and how long it took to execute.
        /// </summary>
        public DateTime TestStartTime
        {
            get
            {
                var d = DataCache<ThreadSafeTestProperties>();
                return d.TestStartTime;
            }
            private set { DataCache<ThreadSafeTestProperties>().TestStartTime = value; }
        }

        /// <summary>
        /// The Selenium webdriver used by the test. There's one webdriver, one browser per running test.
        /// </summary>
        public IWebDriver TestWebDriver
        {
            get
            {
                var d = DataCache<ThreadSafeTestProperties>();
                return d.Driver;
            }
            private set
            {
                DataCache<ThreadSafeTestProperties>().Driver = value;
            }
        }

        /// <summary>
        /// The log4net logger that may optionally be used throughout the test code.
        /// </summary>
        public ILog Logger
        {
            get
            {
                var d = DataCache<ThreadSafeTestProperties>();
                return d.Logger;
            }
            private set
            {
                DataCache<ThreadSafeTestProperties>().Logger = value;
            }
        }

        /// <summary>
        /// The log4net logger used exclusively by PostSharp logging.
        /// </summary>
        public ILog PostSharpLogger
        {
            get
            {
                var d = DataCache<ThreadSafeTestProperties>();
                return d.PostSharpLogger;
            }
            private set
            {
                DataCache<ThreadSafeTestProperties>().PostSharpLogger = value;
            }
        }

        /// <summary>
        /// Log output indent level used by PostSharp logging.
        /// </summary>
        public int PostSharpIndent
        {
            get
            {
                var d = DataCache<ThreadSafeTestProperties>();
                return d.PostSharpIndent;
            }
            set
            {
                DataCache<ThreadSafeTestProperties>().PostSharpIndent = value;
            }
        }

        private Browser _targetBrowser;

        #endregion

        #region Setup & Cleanup

        [OneTimeSetUp]
        public void Init()
        {
            _runningTestsDataDictionaries = new ConcurrentDictionary<string, ConcurrentDictionary<Type, object>>();
            _testResultsFolder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestResults", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }

        protected void BaseTestInitialize(string uri, WebDriverType driverToUse)
        {
            // Create a new log file named specifically for this test. We'll get one log file per executed test.
            var fileName = $"{TestContext.CurrentContext.Test.FullName}.log".Replace("\"", "'").Replace('/', '-').Replace(" - row:", " - row");
            var _logFilePath = Path.Combine(_testResultsFolder, fileName);
            var _loggerRepoString = fileName + "Repository";
            var loggerRepository = LogManager.CreateRepository(_loggerRepoString);

            ThreadContext.Properties["logPath"] = _logFilePath;
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));
            PostSharpLogger = LogManager.GetLogger(_loggerRepoString, "Star.Pages.Logging.LogMethodAspect");
            Logger = LogManager.GetLogger(_loggerRepoString, "Star.Tests.Logging");

            Logger.InfoFormat("Log file stored at: {0}", _logFilePath);

            try
            {
                TestWebDriver = InitializeWebDriver(driverToUse);
                TestWebDriver.Manage().Timeouts().ImplicitWait = ApplicationSettings.ElementLoadTimeSpan;
                TestStartTime = DateTime.Now;
                _targetBrowser = GetBrowserInformation(TestWebDriver);
                TestWebDriver.Navigate().GoToUrl(uri);
                VerifyWebDriverIsConnected();
                Logger.Info($"Running tests on {_targetBrowser.Name} version {_targetBrowser.Version}");
                Logger.Info($"Completed Selenium WebDriver initialization. URL at start of test: {TestWebDriver.Url}.");

            }
            catch (Exception ex)
            {
                Logger.Info(ex.GetType().FullName + " " + ex.Message);
                Logger.Info(ex.StackTrace);
                TestCleanup();
                throw;
            }

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
            Logger.Info($"Test result: {TestContext.CurrentContext.Result.Outcome}");
            Logger.InfoFormat("Test duration: {0:hh\\:mm\\:ss\\.f}", DateTime.Now - TestStartTime);
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
                Logger.Error(ex.Message);

                //Hide WebDriver exceptions here after the test has finished.
                TestWebDriver = null;
            }
            finally
            {
                // This must be done AFTER closing the TestWebDriver
                // If the test failed, capture important test assets created on Sauce Labs
                if (didTestFail)
                {
                    Logger.Error($"URL of failed test: {TestWebDriver.Url}");
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
                Logger.Info($"URL at start of test: {TestWebDriver.Url}");
            }
            catch (WebDriverException ex)
            {
                Logger.Info("***** WARNING *****");
                Logger.Info(ex.ToString());
                Logger.Info("Lost contact with the WebDriver. This test will not continue.");
                Logger.Info(string.Empty);
                Logger.Info("***** END WARNING *****");
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
