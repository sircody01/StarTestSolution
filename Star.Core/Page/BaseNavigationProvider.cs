using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Star.Core.Exceptions;
using Star.Core.Extensions;

namespace Star.Core.Page
{
    public class BaseNavigationProvider
    {
        #region Class Variables

        // This is passed by ref. It cannot be made a property.
        protected IWebTest Test;

        #endregion

        #region Enum's

        public enum HandleAlertMethod
        {
            Accept,
            Dismiss
        }

        #endregion

        #region Properties

        public IWebTest ActiveTest => Test;

        public IWebDriver WebDriver { get; }

        public void WaitForElement(By by)
        {
            WebDriver.WaitForElement(by, ApplicationSettings.ElementLoadTimeSpan);
        }

        protected static TimeSpan ElementLoadTimeSpan => ApplicationSettings.ElementLoadTimeSpan;

        protected static TimeSpan PageLoadTimeSpan => ApplicationSettings.PageLoadTimeSpan;

        #endregion

        #region Constructors

        protected BaseNavigationProvider(ref IWebTest test)
        {
            Test = test;
            WebDriver = test.TestWebDriver;
            WaitForPageLoad();
        }

        #endregion

        #region Protected methods

        protected void WaitForPageLoad()
        {
            try
            {
                WebDriver.WaitForPageLoad(PageLoadTimeSpan);
            }
            catch (Exception ex)
            {
                if (ex is WebDriverTimeoutException)
                    TestContext.WriteLine(@"The specified wait time has expired.");

                TestContext.Out.WriteLine(string.Format(@"{0}:{1}", ErrorMessages.PageNotLoaded,
                    ex.Message));
                throw;
            }
        }

        protected void WaitForElementClickable(IWebElement element)
        {
            var wait = new WebDriverWait(WebDriver, ElementLoadTimeSpan);
            wait.Until(driver => element.IsClickable());
        }

        protected string HandleAlert(int timeoutInSeconds = 30, HandleAlertMethod handleMethod = HandleAlertMethod.Accept)
        {
            string msg;
            var wait = new WebDriverWait(Test.TestWebDriver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                var alert = wait.Until(d =>
                {
                    try
                    {
                        //Attempt to switch to an alert
                        return Test.TestWebDriver.SwitchTo().Alert();
                    }
                    catch (NoAlertPresentException)
                    {
                        //Alert not present yet
                        return null;
                    }
                });
                msg = alert.Text;
                switch (handleMethod)
                {
                    case HandleAlertMethod.Accept:
                        alert.Accept();
                        break;
                    case HandleAlertMethod.Dismiss:
                        alert.Dismiss();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(handleMethod), handleMethod, null);
                }
            }

            catch (WebDriverTimeoutException e)
            {
                msg = e.Message;
            }

            return msg;
        }
        #endregion
    }
}
