using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class HomePage : ProDinnerPage
    {
        #region Element definitions

        private IWebElement PageSize => WebDriver.FindElements(By.CssSelector("#dinnersGridPageSize-awed")).FirstOrDefault();

        private IList<IWebElement> DinnersGridRows => WebDriver.FindElements(By.CssSelector("#dinnersGrid div.awe-row"));

        #endregion

        #region Constructors

        public HomePage(ref IWebTest test)
            : base(ref test)
        {

        }

        #endregion

        #region Public action methods

        public HomePage HomeActionOne(string homeMsg)
        {
            Test.DataCache<SimplePOCO>().Announcement = homeMsg;
            Test.Logger.Info("Doing home action one");
            InternalMethodOne();
            return this;
        }

        public HomePage HomeActionTwo(string homeMsg)
        {
            Assert.AreEqual(homeMsg, Test.DataCache<SimplePOCO>().Announcement);
            Test.Logger.Info("Doing home action two");
            return this;
        }

        public HomePage ChangePageSize(int newSize)
        {
            PageSize.Click();
            var oldRowsCount = DinnersGridRows.Count;
            WebDriver.FindElement(By.XPath($"//*[@id='dinnersGridPageSize-dropmenu']//li[contains(text(), '{newSize}') ]")).Click();
            // The number of grid rows displayed does not instantly change. We have to wait for it.
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(1));
            wait.Until(driver => DinnersGridRows.Count != oldRowsCount);
            Test.DataCache<HomePageModel>().ExpectedPageSize = newSize;
            return this;
        }

        public HomePage ThrowArtificialExceptionForDemo()
        {
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var e = WebDriver.FindElement(By.CssSelector("#InvalidId"));
            return this;
        }

        #endregion

        #region Internal Methods

        public void InternalMethodOne()
        {
            InternalMethodTwo();
        }

        public void InternalMethodTwo()
        {

        }

        #endregion
    }
}
