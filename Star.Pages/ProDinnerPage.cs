using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Star.Core;
using Star.Core.Extensions;
using Star.Core.Page;
using Star.Pages.Navigation;

namespace Star.Pages
{
    public abstract class ProDinnerPage : ProDinnerNavigationProvider, IPage
    {
        #region Enum's

        public enum BrowserFindKey
        {
            Title,
            Url,
            WindowHandleId
        }

        #endregion

        #region Common page elements

        private IWebElement CurrentHtml => WebDriver.FindElement(By.XPath("//*"));

        #endregion

        #region Class variables

        private Stack<string> _browserWindowStack;

        #endregion

        #region Constructors

        protected ProDinnerPage(ref IWebTest test)
            : base(ref test)
        {
        }

        #endregion

        #region Properties

        private List<string> DatesOnPage
        {
            get
            {
                const string datePattern = "[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}";
                var outerHtml = Regex.Replace(CurrentHtml.GetAttribute("outerHTML"), @"Built On:(.*?)" + datePattern, "");
                var dateMatches = Regex.Matches(outerHtml, datePattern);
                return (from object date in dateMatches select date.ToString()).ToList();
            }
        }

        #endregion

        #region Validations

        public abstract bool IsActive();

        #endregion

        #region Common page methods

        internal ProDinnerPage GoBack()
        {
            WebDriver.Navigate().Back();
            return this;
        }

        /// <summary>
        ///     Connect the Selenium WebDriver to a different browser window.
        /// </summary>
        /// <param name="key">The type of search to perform (Title, Url, WindowHandleId)</param>
        /// <param name="keyValue">The value to search for.</param>
        internal void SwitchBrowser(BrowserFindKey key, string keyValue)
        {
            var foundWindow = false;

            if (null == _browserWindowStack)
            {
                _browserWindowStack = new Stack<string>();
            }

            // Now search for the correct window to switch to.
            for (var i = 0; foundWindow == false && i < WebDriver.WindowHandles.Count; i++)
            {
                WebDriver.SwitchToWindow(i);

                switch (key)
                {
                    case BrowserFindKey.Title:
                        if (WebDriver.Title.ToLower().Contains(keyValue.ToLower()))
                            foundWindow = true;
                        break;

                    case BrowserFindKey.Url:
                        if (WebDriver.Url.ToLower().Contains(keyValue.ToLower()))
                            foundWindow = true;
                        break;

                    case BrowserFindKey.WindowHandleId:
                        if (WebDriver.WindowHandles[i] == keyValue)
                            foundWindow = true;
                        break;
                }
            }
            if (!foundWindow)
            {
                throw new ArgumentException("Cannot find the window using method '" + key + "' having value '" +
                                            keyValue + "'.");
            }
            _browserWindowStack.Push(WebDriver.CurrentWindowHandle);
        }

        #endregion
    }
}
