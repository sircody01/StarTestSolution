using NUnit.Framework;
using OpenQA.Selenium;
using Star.Core.WebDriver;

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

        #endregion

        protected void BaseTestInitialize(string uri, WebDriverType driverToUse)
        {
            TestWebDriver = InitializeWebDriver(driverToUse);
        }
    }
}
