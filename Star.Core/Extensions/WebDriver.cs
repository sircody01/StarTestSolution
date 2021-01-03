using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Dinobots.Core.WebDriver;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using Star.Core.Exceptions;

namespace Star.Core.Extensions
{
    public static class WebDriver
    {
        /// <summary>
        /// Waits For An element to be loaded in the DOM. This function will wait for the element for the specified TimeSpan.
        /// </summary>
        /// <param name="driver">Extension method type, IWebDriver</param>
        /// <param name="by">The By Locator. For more information see <see href="http://selenium.googlecode.com/git/docs/api/dotnet/?topic=html/T_OpenQA_Selenium_By.htm">Selenium 'By' API Documentation</see></param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">if <c>true</c>, the timeout will not throw an exception. (Default is <c>true</c>)</param>
        public static void WaitForElement(this IWebDriver driver, By by, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(drv => drv.FindElement(by));
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (NoSuchWindowException ex)
            {
                TestContext.WriteLine("No such window Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// Waits for page to load.
        /// </summary>
        /// <param name="driver">Extension method type, IWebDriver</param>
        /// <param name="timeout">The amount of time to wait for the page to load before returning.</param>
        /// <returns><c>true</c> if page loads within the specified time <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">driver;Driver must support javascript execution</exception>
        /// <exception cref="WebDriverTimeoutException">The specified wait time has expired. See Selenium Documentation for <see href="http://selenium.googlecode.com/git/docs/api/dotnet/?topic=html/T_OpenQA_Selenium_WebDriverTimeoutException.htm">OpenQA.Selenium.WebDriverTimeoutException</see></exception>
        public static void WaitForPageLoad(this IWebDriver driver, TimeSpan timeout)
        {
            var timer = new Stopwatch();
            timer.Start();
            while (timer.Elapsed < timeout)
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(
                    driver1 => IsPageReady(driver as IJavaScriptExecutor)
                );
                // If we get to here, the wait finished before timing out and it's time to return to the caller.
                break;
            }
        }

        private static bool IsPageReady(IJavaScriptExecutor driver)
        {
            var isReadyStateComplete = false;
            var isjQueryActive = false;
            var isAngularRequestsPending = false;

            try
            {
                var isDocumentDefined = (bool)driver.ExecuteScript("return (typeof document != 'undefined')");
                if (isDocumentDefined)
                {
                    isReadyStateComplete = driver.ExecuteScript("return document.readyState").Equals("complete");
                }

                if (!isReadyStateComplete)
                    return false;
                var isjQueryDefined = (bool)driver.ExecuteScript("return (typeof window.jQuery != 'undefined')");
                if (isjQueryDefined)
                {
                    isjQueryActive = (bool)driver.ExecuteScript("return jQuery.active != 0;");
                }

                if (isjQueryDefined && isjQueryActive)
                    return false;
                var isAngularDefined = (bool)driver.ExecuteScript(
                    "return (typeof window.angular != 'undefined')");
                if (isAngularDefined)
                {
                    var isAngularInjectorDefined = (bool)driver.ExecuteScript(
                        "return (typeof angular.element(document).injector() != 'undefined')");
                    isAngularRequestsPending = isAngularInjectorDefined
                        ? (bool)driver.ExecuteScript(
                            "return (angular.element(document).injector().get('$http').pendingRequests.length != 0)")
                        : (bool)driver.ExecuteScript(
                            "return (angular.element(document.querySelector('[ng-app]')).injector().get('$http').pendingRequests.length != 0)");
                }

                return !isAngularDefined || !isAngularRequestsPending;
            }
            catch (InvalidOperationException)
            {
                // Sometimes ExecuteScript errors out with System.InvalidOperationException: JavaScript error (UnexpectedJavaScriptError)
                // Just ignore the error and continue waiting
                return false;
            }
            catch (WebDriverException)
            {
                // Sometimes in IE11 ExecuteScript errors out with OpenQA.Selenium.WebDriverException: Error executing JavaScript
                // Just ignore the error and continue waiting
                return false;
            }
        }

        public static void WaitForNewWindow(this IWebDriver driver, TimeSpan timeout)
        {
            var startCount = driver.WindowHandles.Count;
            var wait = new WebDriverWait(driver, timeout);
            wait.Until(d => d.WindowHandles.Count > startCount);
        }

        /// <summary>
        /// Determines whether the element specified in the By locator exists in the DOM.
        /// </summary>
        /// <param name="driver">Extension method type, IWebDriver</param>
        /// <param name="by">The By locator. For more information see <see href="http://selenium.googlecode.com/git/docs/api/dotnet/?topic=html/T_OpenQA_Selenium_By.htm">Selenium 'By' API Documentation</see></param>
        /// <returns><c>true</c> if the specified driver has element; otherwise, <c>false</c>.</returns>
        public static bool ElementExists(this IWebDriver driver, By by)
        {
            var remote = driver as RemoteWebDriver;
            var browser = remote?.Capabilities["browserName"] as string;
            var oldTimeout = browser == "MicrosoftEdge"
                ? ApplicationSettings.ElementLoadTimeSpan
                : driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
            var count = driver.FindElements(by).Count;
            driver.Manage().Timeouts().ImplicitWait = oldTimeout;
            return count > 0;
        }

        /// <summary>
        /// FindElement  - Wait for the element for specified time or until it exists and returns the IWebElement
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        public static IWebElement FindElement(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            return wait.Until(drv => drv.FindElement(by).Displayed ? drv.FindElement(by) : null);
        }

        /// <summary>
        /// FindElements  - Waits for the elements for specified time or until they exist and returns the IWebElement Collection
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the elements to load before returning.</param>
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            return wait.Until(drv => drv.FindElements(by).Count > 0 ? drv.FindElements(by) : null);
        }

        /// <summary>
        /// WaitForAnElementDisplayed  - Wait for the element to be displayed for specified time or until it is displayed within the specified time
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForElementDisplayed(this IWebDriver driver, By by, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(drv => drv.FindElement(by).Displayed);
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// WaitForAnElementVisible  - Wait for the element to be displayed within specified time
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForElementVisible(this IWebDriver driver, By by, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(drv => drv.FindElement(by).Displayed && drv.FindElement(by).Enabled);
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// WaitForAnElementClickable  - Wait for the element to be clickable within specified time
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="locator">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForElementClickable(this IWebDriver driver, By locator, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(d => d.IsClickable(locator));
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// WaitForAnElementExists  - Wait for the element to be existing within specified time
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="locator">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForElementExists(this IWebDriver driver, By locator, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(d => d.ElementExists(locator));
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// WaitForElementToBeHidden  - Wait for the element to not be visible within specified time
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForElementToBeHidden(this IWebDriver driver, By by, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(drv => !drv.FindElement(by).Displayed && drv.FindElement(by).Enabled);
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (StaleElementReferenceException ex)
            {
                TestContext.WriteLine("Stale Element Reference Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// Wait for the Select option to be selected.
        /// </summary>
        /// <param name="driver">The driver - This web Driver</param>
        /// <param name="selectElement">The HTML Select element</param>
        /// <param name="optionValue">The value of the select option to wait to be selected</param>
        /// <param name="timeout">The amount of time to wait for the option to be selected before returning</param>
        /// <param name="logger">The log4net ILog to use when logging errors</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitForSelectOptionToBeSelected(this IWebDriver driver, IWebElement selectElement,
            string optionValue, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(d =>
                {
                    var selected =
                        d.ExecuteJavaScript<ReadOnlyCollection<object>>("return $(arguments[0]).val()", selectElement);
                    return selected != null && selected.Count > 0 && selected.Contains(optionValue);
                });
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        /// <summary>
        /// Wait for the attribute of the target element to meet the specified condition.
        /// </summary>
        /// <param name="driver">The driver - This Web Driver</param>
        /// <param name="locator">The by - By Locator</param>
        /// <param name="attributeName">The attribute name to test</param>
        /// <param name="elementPropertyFilter">The searchType - Search Type - contains, starts with, ends with, equals, not equals</param>
        /// <param name="searchFor">string searchFor</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net logger to use in the method.</param>
        /// <param name="suppressException">The suppressException - bool Type</param>
        public static void WaitFor(this IWebDriver driver, By locator, string attributeName, ElementPropertyFilter elementPropertyFilter, string searchFor, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                switch (elementPropertyFilter)
                {
                    case ElementPropertyFilter.Contains:
                        wait.Until(drv => drv.FindElement(locator).GetAttribute(attributeName).Contains(searchFor));
                        break;
                    case ElementPropertyFilter.EndsWith:
                        wait.Until(drv => drv.FindElement(locator).GetAttribute(attributeName).EndsWith(searchFor));
                        break;
                    case ElementPropertyFilter.Equals:
                        wait.Until(drv => drv.FindElement(locator).GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.NotEquals:
                        wait.Until(drv => !drv.FindElement(locator).GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.StartsWith:
                        wait.Until(drv => drv.FindElement(locator).GetAttribute(attributeName).StartsWith(searchFor));
                        break;
                }
            }
            catch (NoSuchElementException ex)
            {
                TestContext.WriteLine("Element Not Found. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!suppressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!suppressException)
                    throw;
            }
        }

        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, bool> condition, TimeSpan timeout, bool shouldRethrow = false)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(condition);
            }
            catch (Exception)
            {
                if (shouldRethrow)
                    throw;
            }
        }

        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, IWebElement> condition, TimeSpan timeout, bool shouldRethrow = false)
        {
            try
            {
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(condition);
            }
            catch (Exception)
            {
                if (shouldRethrow)
                    throw;
            }
        }

        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, bool> condition, bool shouldRethrow = false)
        {
            driver.WaitFor(condition, ApplicationSettings.ElementLoadTimeSpan, shouldRethrow);
        }

        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, IWebElement> condition, bool shouldRethrow = false)
        {
            driver.WaitFor(condition, ApplicationSettings.ElementLoadTimeSpan, shouldRethrow);
        }

        /// <summary>
        /// Will scoll the browsers viewport up to the specified coordinate.
        /// </summary>
        /// <param name="driver">The driver - This Web Driver</param>
        /// <param name="coordinates">The coordinate of the pixel to move to the upper left corner.
        /// Default is 0, 250</param>
        public static void ScrollUp(this IWebDriver driver, string coordinates = "0,250")
        {
            driver.ExecuteJavaScript("scroll(" + coordinates + ")");
        }

        public static Screenshot CaptureScreenshot(this IWebDriver driver)
        {
            if (!(driver is ITakesScreenshot camera))
                throw new ScreenshotException("Screenshot capture not possible because camera was not found");
            return camera.GetScreenshot();
        }

        /// <summary>
        /// Determines whether or not a modal dialog is present.
        /// If it is the handle to the dialog is returned.
        /// </summary>
        /// <param name="driver">The driver - This Web Driver</param>
        public static IAlert IsAlertPresent(this IWebDriver driver)
        {
            try
            {
                return driver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether or not the specified element is visible and enabled such that you can click it.
        /// </summary>
        /// <param name="driver">The driver - This Web Driver</param>
        /// <param name="locator">The By object to use to find the element to test</param>
        public static bool IsClickable(this IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                return element != null && element.Enabled;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static bool IsFieldVisibile(this IWebDriver driver, string controlId)
        {
            var element = driver.FindElement(By.Id(controlId));
            return element.Displayed;
        }

        public static IEnumerable<string> FindCssHttpLinkElements(this IWebDriver driver)
        {
            return driver.GetElementAttributes(By.XPath("(//link[contains(@href,'http:')])"), "href");
        }

        public static IEnumerable<string> FindImageHttpLinkElements(this IWebDriver driver)
        {
            return driver.GetElementAttributes(By.XPath("(//img[contains(@src,'http:')])"), "src");
        }

        public static IEnumerable<string> FindHttpLinkElements(this IWebDriver driver)
        {
            return driver.GetElementAttributes(By.XPath("(//a[contains(@href,'http:')])"), "href");
        }

        private static IEnumerable<IWebElement> GetElements(this IWebDriver driver, By locator)
        {
            var list = new List<IWebElement>();
            var links = new ReadOnlyCollection<IWebElement>(list);
            var doesElementExist = driver.ElementExists(locator);

            if (doesElementExist)
                links = driver.FindElements(locator, ApplicationSettings.ElementLoadTimeSpan);

            return links.ToList();
        }

        private static IEnumerable<string> GetElementAttributes(this IWebDriver driver, By locator, string elementAttribute)
        {
            var links = driver.GetElements(locator);
            return links.Select(element => element.GetAttribute(elementAttribute)).ToList();
        }

        public static void SwitchToFrame(this IWebDriver driver, IWebElement frame)
        {
            driver.SwitchTo().Frame(frame);
        }

        public static void SwitchToFrame(this IWebDriver driver, By findBy)
        {
            driver.SwitchTo().Frame(driver.FindElement(findBy, TimeSpan.FromSeconds(15)));
        }

        public static void SwitchToWindow(this IWebDriver driver, int index)
        {
            driver.SwitchTo().Window(driver.WindowHandles[index]);
        }

        public static void SwitchToDefaultContent(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        ///     Provide a way to click on links and buttons through JavaScript.
        ///     IE does not always "Click" on elements, this method should be use when
        ///     this issue happens on normal Click() selenium method.
        ///     on a link or button. All other browsers do not have the problem with clicking.
        /// </summary>
        /// <param name="driver">The IWebDriver</param>
        /// <param name="element"> The webElement that click() does not work properly on IE</param>
        /// <remarks>https://github.com/seleniumhq/selenium-google-code-issue-archive/issues/4403</remarks>
        public static void JavaScriptClick(this IWebDriver driver, IWebElement element)
        {
            driver.ExecuteJavaScript("arguments[0].click()", element);
        }

        /// <summary>
        ///     IE Modal windows are difficult to connect to. An ordinary click will never return until
        ///     the modal window is closed. Even trying to call JavaScriptClick gets blocked on the
        ///     modal window.
        ///
        ///     The only solution is to use an advanced JavaScript method to make the click in a separate
        ///     JavaScript thread, then connect to the new window when it opens.
        /// </summary>
        /// <param name="driver">The IWebDriver.</param>
        /// <param name="element">The element to click that opens the modal window.</param>
        /// <param name="timeout">The amount of time to wait for the window to open.</param>
        public static void OpenAndConnectModalWindow(this IWebDriver driver, IWebElement element, TimeSpan timeout)
        {
            // To connect to the modal window we'll spawn a separate JavaScript thread to do the actual element click.
            driver.ExecuteJavaScript("var el=arguments[0]; setTimeout(function() { el.click(); }, 100);", element);
            if (ApplicationSettings.StarDriverType.ToString().Equals("InternetExplorer"))
            {
                driver.WaitForNewWindow(timeout);
                driver.SwitchToWindow(1);
            }
        }

        public static IWebElement ScrollIntoView(this IWebDriver driver, IWebElement element)
        {
            driver.ExecuteJavaScript("arguments[0].scrollIntoView();", element);
            return element;
        }

        /// <summary>
        /// Scroll the browser window to the specified X, Y position.
        /// </summary>
        /// <param name="driver">The IWebDriver to use.</param>
        /// <param name="pos">The X, Y position to scroll the window to.</param>
        public static void ScrollWindowTo(this IWebDriver driver, Point pos)
        {
            driver.ExecuteJavaScript("window.scrollTo(arguments[0], arguments[1])", pos.X, pos.Y);
        }

        /// <summary>
        /// Scroll the upper left corner of the target element to the specified X, Y position.
        /// </summary>
        /// <param name="driver">The IWebDriver to use.</param>
        /// <param name="element">The target element to scroll.</param>
        /// <param name="desiredPoint">The point to scroll the target element to.</param>
        public static void ScrollElementTo(this IWebDriver driver, IWebElement element, Point desiredPoint)
        {
            var currentPageOffset = new Point();
            var newPageOffset = new Point();

            var currentLocation = element.Location;
            currentPageOffset.Y = (int)driver.ExecuteJavaScript<long>("return window.pageYOffset;");
            currentPageOffset.X = (int)driver.ExecuteJavaScript<long>("return window.pageXOffset;");
            newPageOffset.Y = currentLocation.Y + currentPageOffset.Y - desiredPoint.Y;
            newPageOffset.X = currentLocation.X + currentPageOffset.X - desiredPoint.X;
            driver.ExecuteJavaScript("window.scrollTo(" + newPageOffset.X + ", " + newPageOffset.Y + ");");
        }

        public static long ApiGet(this IWebDriver webDriver, string url)
        {
            const string script = "var xhr = new XMLHttpRequest();" + "xhr.open('GET', arguments[0], false);" +
                                  "xhr.send(null);" + "return xhr.status";
            var apiStatus = webDriver.ExecuteJavaScript<long>(script, url);
            return apiStatus;
        }

        public static long ApiPost(this IWebDriver webDriver, string url)
        {
            const string script = "var xhr = new XMLHttpRequest();" + "xhr.open('POST', arguments[0], false);" +
                                  "xhr.send(null);" + "return xhr.status";
            var apiStatus = webDriver.ExecuteJavaScript<long>(script, url);
            return apiStatus;
        }
    }
}
