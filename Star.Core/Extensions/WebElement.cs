using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Dinobots.Core.WebDriver;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using Star.Core.Extensions;

namespace Star.Core.Extensions
{
    public static class WebElement
    {
        /// <summary>
        ///     Determines whether the specified element has the 'child' element.
        /// </summary>
        /// <param name="element">The element - this element.</param>
        /// <param name="by">
        ///     The By Locator. For more information see
        ///     <see href="http://selenium.googlecode.com/git/docs/api/dotnet/?topic=html/T_OpenQA_Selenium_By.htm">
        ///         Selenium 'By'
        ///         API Documentation
        ///     </see>
        /// </param>
        /// <returns><c>true</c> if the specified element is not null; otherwise, <c>false</c>.</returns>
        public static bool ElementExists(this IWebElement element, By by)
        {
            try
            {
                element.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Clears the existing text from the element and sets the provided text.
        /// </summary>
        /// <param name="element">The text element to enter text into.</param>
        /// <param name="text">The text to be entered.</param>
        public static void EnterText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        ///     Check if the element is a DropDownList or a text field and perform the appropriate
        ///     action to enter/select the requested information.
        /// </summary>
        /// <param name="element">The element to enter text into or select from list.</param>
        /// <param name="text">The text to be entered.</param>
        public static void SelectOrEnterText(this IWebElement element, string text)
        {
            if (element.GetAttribute("type").Equals("text"))
            {
                element.EnterText(text);
            }
            else
            {
                element.Select().SelectByText(text);
            }
        }

        /// <summary>
        ///     Get the value of the 'value' attribute of the element.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <returns>The value as a string.</returns>
        public static string GetValueAttribute(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        /// <summary>
        ///     Get the value of the 'href' attribute of the element.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <returns>The value as a string.</returns>
        public static string GetHrefAttribute(this IWebElement element)
        {
            return element.GetAttribute("href");
        }

        /// <summary>
        ///     Tests whether or not the element has the specified class name.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <param name="className">The class name to look for.</param>
        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetAttribute("class")
                .Split(' ')
                .Any(c => string.Equals(className, c, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Determines whether or not the element is visible and enabled such that you can click it.
        /// </summary>
        /// <param name="element"></param>
        public static bool IsClickable(this IWebElement element)
        {
            try
            {
                return element != null && element.Displayed && element.Enabled;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether or not the element is both enabled and displayed. It still may not be visible on the screen as
        ///     it could be scrolled off the screen either vertically or horizontally. But it would be visible on the screen simply
        ///     by scrolling the browser window.
        /// </summary>
        /// <param name="element"></param>
        public static bool IsVisible(this IWebElement element)
        {
            return element.Enabled && element.Displayed;
        }

        /// <summary>
        ///     Casts an IWebElement into a SelectElement.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <returns>The same element as a SelectElement</returns>
        public static SelectElement Select(this IWebElement element)
        {
            return new SelectElement(element);
        }

        /// <summary>
        /// Selected an option from an html select element.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <param name="index">The index to select. Positive index works from the beginning of the options list.
        /// A negative index works from the end of the options list.</param>
        public static void SelectByIndex(this IWebElement element, int index)
        {
            var select = element.Select();
            if (index < 0)
            {
                select.SelectByIndex(select.Options.Count + index);
            }
            else
            {
                select.SelectByIndex(index);
            }
        }

        /// <summary>
        ///     Checks/Unchecks the check box based on the state (true/false) passed to this method.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <param name="state">The desired state of the checkbox. True = checked, False = unchecked.</param>
        public static void Check(this IWebElement element, bool state)
        {
            // Click it only if the desired state is different from it's current state.
            if (state ^ element.Selected)
                element.Click();
        }

        /// <summary>
        ///     Checks/Unchecks the check box based on the state (true/false) passed to this method
        ///     using JavaScript which works across all browsers.
        /// </summary>
        /// <param name="element">The element - This web element.</param>
        /// <param name="driver2"></param>
        /// <param name="state">The desired state of the checkbox. True = checked, False = unchecked.</param>
        /// <returns>The checked state of the element at the start.</returns>
        public static bool Check(this IWebElement element, IWebDriver driver2, bool state = true)
        {
            var startValue = element.Selected;

            // Click it only if the desired state is different from it's current state.
            if (!(state ^ element.Selected))
                return startValue;
            // Currently getting the driver from the element only works for elements obtained
            // using FindElement. This does not work for elements obtained via the PageFactory.
            // If the Selenium developers ever "fix" this we can stop passing the driver in as
            // a parameter.
            //var driver = ((IWrapsDriver)element).WrappedDriver;
            driver2.ExecuteJavaScript("arguments[0].checked = arguments[1];", element, state);

            return startValue;
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>IWebElement </returns>
        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }

        /// <summary>
        /// Grab the text from the list of elements and returns a new string list.
        /// </summary>
        /// <param name="elements">The list of elements to operate on.</param>
        /// <returns>New IList/<string/> containing the text from all of the elements.</returns>
        public static IList<string> GetTextList(this IList<IWebElement> elements)
        {
            return elements.Select(x => x.Text.Trim()).ToList();
        }

        /// <summary>
        /// Gets Inner Text
        /// </summary>
        /// <param name="webElement">The element.</param>
        /// <returns>IWebElement </returns>
        public static string GetInnerText(this IWebElement webElement)
        {
            return webElement.GetAttribute("innerText").Trim();
        }

        /// <summary>
        /// Fetch the list of options for an HTML Select element.
        /// </summary>
        /// <param name="webElement">The Select element to fetch the options for.</param>
        /// <returns>An IList&gt;SelectOption&lt; object containing the list of options.</returns>
        public static IList<SelectOption> GetSelectElementOptions(this IWebElement webElement)
        {
            var html = webElement.GetAttribute("innerHTML");
            var htmlSplit = html.Split('\r', '\n');
            return (from optStr in htmlSplit
                    select Regex.Match(optStr, @"^\s*<option value=\""(\d+)\"">(.+)<\/option>$")
                into match
                    where match.Success
                    select new SelectOption { Value = match.Groups[1].Value, Text = match.Groups[2].Value }).ToList();
        }

        #region WaitFor Methods

        /// <summary>
        ///     WaitForAnElement - Waits for the element for specified time or until it exists
        /// </summary>
        /// <param name="element">Current Element</param>
        /// <param name="by">
        ///     The By Locator. For more information see
        ///     <see href="http://selenium.googlecode.com/git/docs/api/dotnet/?topic=html/T_OpenQA_Selenium_By.htm">
        ///         Selenium 'By' API Documentation
        ///     </see>
        /// </param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">if <c>true</c>, the timeout will not throw an exception. (Default is <c>true</c>)</param>
        public static void WaitForElement(this IWebElement element, By by, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ctx => element.FindElement(by));
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
        ///     FindElement - Waits for the element for specified time or until it exists and returns the IWebElement
        /// </summary>
        /// <param name="element">The element - This web Element</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        public static IWebElement FindElement(this IWebElement element, By by, TimeSpan timeout)
        {
            var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };

            return wait.Until(ctx => element.FindElement(by).Displayed ? element.FindElement(by) : null);
        }

        /// <summary>
        ///     FindElements - Waits for the elements for specified time or until they exist and returns the IWebElement
        ///     Collection
        /// </summary>
        /// <param name="element">The element - This web Element</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebElement element, By by, TimeSpan timeout)
        {
            var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };

            return wait.Until(ctx => element.FindElements(by).Count > 0 ? element.FindElements(by) : null);
        }

        /// <summary>
        ///     WaitForElement - Waits for the element for specified time or until it exists
        /// </summary>
        /// <param name="element">Current Element</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="supressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitForElementDisplayed(this IWebElement element, By by, TimeSpan timeout,
            bool supressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };

                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ctx => element.FindElement(by).Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                TestContext.WriteLine("Element Timeout Exception. " + ex.Message);
                if (!supressException)
                    throw;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Exception thrown. Please see Exception details " + ex.Message);
                if (!supressException)
                    throw;
            }
        }

        /// <summary>
        ///     WaitForElementVisible - Waits for the element Visible for specified time or until it is displayed within specified
        ///     time
        /// </summary>
        /// <param name="element">The driver - This web Driver</param>
        /// <param name="by">The by - Locator using By</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitForElementVisible(this IWebElement element, By by, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ctx => element.FindElement(by).Displayed && element.FindElement(by).Enabled);
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
        ///     WaitForAnElement - Waits for the element for specified time or until it exists
        /// </summary>
        /// <param name="element">Current Element</param>
        /// ///
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitForElementDisplayed(this IWebElement element, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ctx => element.Displayed);
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
        ///     WaitForAnElementVisible - Waits for the element Visible for specified time or until it is displayed within
        ///     specified time
        /// </summary>
        /// <param name="element">The driver - This web Driver</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitForElementVisible(this IWebElement element, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.Until(ctx => element.Displayed && element.Enabled);
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
        ///     WaitFor - WebElement Waits until the Condition is true
        /// </summary>
        /// <param name="element">The element - This Web Element</param>
        /// <param name="attributeName">The attribute name - attribute name</param>
        /// <param name="elementPropertyFilter">The searchType - Search Type - contains, starts with, ends with, equals, not equals</param>
        /// <param name="searchFor">string searchFor</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitFor(this IWebElement element, string attributeName,
            ElementPropertyFilter elementPropertyFilter, string searchFor, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                switch (elementPropertyFilter)
                {
                    case ElementPropertyFilter.Contains:
                        wait.Until(ctx => element.GetAttribute(attributeName).Contains(searchFor));
                        break;
                    case ElementPropertyFilter.EndsWith:
                        wait.Until(ctx => element.GetAttribute(attributeName).EndsWith(searchFor));
                        break;
                    case ElementPropertyFilter.Equals:
                        wait.Until(ctx => element.GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.NotEquals:
                        wait.Until(ctx => !element.GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.StartsWith:
                        wait.Until(ctx => element.GetAttribute(attributeName).StartsWith(searchFor));
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

        /// <summary>
        ///     WaitFor - WebElement Waits for Element Child until the Condition is true
        /// </summary>
        /// <param name="element">The element - This Web Element</param>
        /// <param name="by">The by - By Locator</param>
        /// <param name="attributeName">The attribute name - attribute name</param>
        /// <param name="elementPropertyFilter">The searchType - Search Type - contains, starts with, ends with, equals, not equals</param>
        /// <param name="searchFor">string searchFor</param>
        /// <param name="timeout">The amount of time to wait for the element to load before returning.</param>
        /// <param name="logger">The log4net ILog to use when logging errors.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitFor(this IWebElement element, By by, string attributeName,
            ElementPropertyFilter elementPropertyFilter, string searchFor, TimeSpan timeout,
            bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                switch (elementPropertyFilter)
                {
                    case ElementPropertyFilter.Contains:
                        wait.Until(ctx => element.FindElement(by).GetAttribute(attributeName).Contains(searchFor));
                        break;
                    case ElementPropertyFilter.EndsWith:
                        wait.Until(ctx => element.FindElement(by).GetAttribute(attributeName).EndsWith(searchFor));
                        break;
                    case ElementPropertyFilter.Equals:
                        wait.Until(ctx => element.FindElement(by).GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.NotEquals:
                        wait.Until(ctx => !element.FindElement(by).GetAttribute(attributeName).Equals(searchFor));
                        break;
                    case ElementPropertyFilter.StartsWith:
                        wait.Until(ctx => element.FindElement(by).GetAttribute(attributeName).StartsWith(searchFor));
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

        /// <summary>
        ///     Waits for the specified function to be true.
        /// </summary>
        /// <param name="element">The IWebElement to act on.</param>
        /// <param name="condition">Delegate function that must return a boolean.</param>
        /// <param name="timeout">The amount of time to wait for the condition to be true.</param>
        /// <param name="suppressException">Whether or not to rethrow the exception. Default value is true.</param>
        public static void WaitFor(this IWebElement element, Func<IWebElement, bool> condition, TimeSpan timeout, bool suppressException = true)
        {
            try
            {
                var wait = new DefaultWait<IWebElement>(element) { Timeout = timeout };
                wait.Until(condition);
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

        #endregion
    }

    public class SelectOption
    {
        public string Value;
        public string Text;
    }
}
