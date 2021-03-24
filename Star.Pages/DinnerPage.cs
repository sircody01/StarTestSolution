using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using Star.Core;
using Star.Core.Extensions;
using Star.Models;

namespace Star.Pages
{
    public partial class DinnerPage : ProDinnerPage
    {
        #region Element definitions

        private IWebElement CreateButton => WebDriver.FindElement(By.CssSelector("body > main > div.bar > button"));
        private IWebElement NameField => WebDriver.FindElement(By.CssSelector("#createdinnerGridName-awed"));
        private IWebElement Country => WebDriver.FindElement(By.CssSelector("#createdinnerGrid > div > div > form > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div.einput > div > div > button"));
        private IWebElement CountrySearch => WebDriver.FindElement(By.CssSelector("#createdinnerGridCountryIdSearch-awed"));
        private IWebElement CountryOkButton => WebDriver.FindElement(By.CssSelector("body > div:nth-child(33) > div > div.o-pbtns > button.awe-btn.awe-okbtn.o-pbtn"));
        private IWebElement Chef => WebDriver.FindElement(By.CssSelector("#createdinnerGridChefId-awed"));
        private IWebElement ChefSearch => WebDriver.FindElement(By.CssSelector("#createdinnerGridChefId-dropmenu > div.o-srcc.o-ldngp > input"));
        private IWebElement ChefFirstRow => WebDriver.FindElement(By.CssSelector("#createdinnerGridChefId-dropmenu > div.o-itsc > ul > li"));
        private IWebElement AddressField => WebDriver.FindElement(By.CssSelector("#createdinnerGridAddress-awed"));
        private IWebElement StartDate => WebDriver.FindElement(By.CssSelector("#createdinnerGridStart"));
        private IWebElement Time => WebDriver.FindElement(By.CssSelector("#createdinnerGridTime-awed"));
        private IWebElement Duration => WebDriver.FindElement(By.CssSelector("#createdinnerGridDuration-awed"));
        private IWebElement MealsInput => WebDriver.FindElement(By.CssSelector("#createdinnerGrid > div > div > form > div.finput > div > div > div.o-dd > div"));
        private IWebElement OkButton => WebDriver.FindElement(By.CssSelector("body > div.o-pwrap > div > div.o-pbtns > button.awe-btn.awe-okbtn.o-pbtn"));
        private IWebElement DinnersTableFirstRow => WebDriver.FindElement(By.CssSelector("#dinnerGrid div.awe-content.awe-tablc tr"));
        private IWebElement TableCount => WebDriver.FindElement(By.CssSelector("#dinnerGrid > div.awe-footer > div.o-gpginf"));

        #endregion

        #region Constructors

        public DinnerPage(ref IWebTest test)
            : base(ref test)
        {

        }

        #endregion

        #region Action Steps

        public DinnerPage DinnerActionOne(string dinnerMsg)
        {
            Test.Logger.Info("Doing dinner action one");
            Test.DataCache<SimplePOCO>().Announcement = dinnerMsg;
            return this;
        }

        public DinnerPage DinnerActionTwo(string dinnerMsg)
        {
            Test.Logger.Info("Doing dinner action two");
            Assert.AreEqual(dinnerMsg, Test.DataCache<SimplePOCO>().Announcement);
            return this;
        }

        public DinnerPage CreateDinner(DinnerModel dinnerData)
        {
            // Open the create dinner input form
            CreateButton.Click();

            // Fill in the create dinner form
            NameField.SendKeys(dinnerData.Name);

            Country.Click();
            CountrySearch.SendKeys(dinnerData.Country + Keys.Enter);
            // Need just a little time for the filter to act
            Thread.Sleep(300);
            // We must to a fresh find of the first row, else we get a stale element exception thrown
            WebDriver.FindElement(By.CssSelector("#createdinnerGridCountryIdGrid1 > div.awe-mcontent > div.awe-content.awe-tablc > div > table > tbody > tr")).Click();
            CountryOkButton.Click();

            Chef.Click();
            ChefSearch.SendKeys(dinnerData.Chef);
            ChefFirstRow.Click();

            AddressField.SendKeys(dinnerData.Address);

            StartDate.SendKeys(dinnerData.Start.ToString("d"));

            Time.SendKeys(dinnerData.Time);

            Duration.SendKeys(dinnerData.Duration.ToString());

            MealsInput.Click();
            foreach (var mealName in dinnerData.Meals)
            {
                WebDriver.FindElement(By.XPath($"//*[@id='createdinnerGridMeals-dropmenu']/div[2]/ul/li/div[text()=' {mealName}']")).Click();
            }

            // Save current table row counts
            var rowCounts = TableCount.GetInnerText();
            OkButton.Click();
            // There is a delay between clicking the OK button and the grid being updated in the UI
            // Wait for the UI grid to be updated by waiting for the row count shown at the bottom of the grid to be incremented
            TableCount.WaitFor(e => e.GetInnerText() != rowCounts, ElementLoadTimeSpan);

            return this;
        }

        #endregion
    }
}
