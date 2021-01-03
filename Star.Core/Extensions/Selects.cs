using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Dinobots.Core.Extensions
{
    /// <summary>
    /// Extension methods for Selects
    /// </summary>
    public static class Selects
    {
        /// <summary>
        /// Selects all.  </summary>
        /// <param name="sel">The sel.</param>
        /// <param name="driver">The Selenium webdriver to use.</param>
        public static void SelectAll(this SelectElement sel, IWebDriver driver)
        {
            if (driver.ToString().Contains("Chrome"))
            {
                var actions = new Actions(driver);
                var ctrlactions = actions.KeyDown(Keys.Control);
                ctrlactions = sel.Options.Where(item => !item.Selected).Aggregate(ctrlactions, (current, item) => current.Click(item));
                ctrlactions.KeyUp(Keys.Control).Build().Perform();
            }
            else
            {
                foreach (var item in sel.Options)
                {
                    if (!item.Selected)
                        item.Click();
                }
            }
        }

        public static void MultiSelectByText(this SelectElement sel, string[] list, IWebDriver driver)
        {
            if (driver.ToString().Contains("Chrome"))
            {
                var actions = new Actions(driver);
                var ctrlactions = actions.KeyDown(Keys.Control);
                ctrlactions = list.Aggregate(ctrlactions, (current, text) => current.Click(sel.Options.First(a => a.Text == text)));
                ctrlactions.KeyUp(Keys.Control).Build().Perform();
            }
            else
            {
                foreach (var text in list)
                {
                    sel.SelectByText(text);
                }
            }
        }

        /// <summary>
        /// Selects the by partial text.
        /// </summary>
        /// <param name="sel">The select element.</param>
        /// <param name="partialText">The partial text.</param>
        public static void SelectByPartialText(this SelectElement sel, string partialText)
        {
            var option = sel.WrappedElement.FindElement(By.XPath("descendant::option[contains(text(),'" + partialText + "')]"));
            sel.SelectByValue(option.GetAttribute("value"));
        }
    }
}
