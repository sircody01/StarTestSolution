using FluentAssertions;
using FluentAssertions.Execution;
using OpenQA.Selenium;
using Star.Core.Extensions;
using Star.Models;

namespace Star.Pages
{
    public partial class DinnerPage : ProDinnerPage
    {
        #region Validations

        public DinnerPage VerifyDinnerAdded(DinnerModel dinnerData)
        {
            var firstRow = DinnersTableFirstRow;
            var cells = firstRow.FindElements(By.CssSelector("td"));
            using (new AssertionScope())
            {
                cells[0].Should().Be(dinnerData.Name);
                cells[1].Should().Be(dinnerData.Chef);
                cells[2].Should().Be(dinnerData.Country);
                dinnerData.Meals.TrueForAll(s => cells[3].GetInnerText().Contains(s));
                cells[4].Should().Be(dinnerData.Address);
            }
            return this;
        }

        public override bool IsActive()
        {
            WebDriver.Url.Should().Be("https://prodinner.aspnetawesome.com/Dinner", "Did not land on the dinner page");
            return true;
        }

        #endregion
    }
}
