using FluentAssertions;
using NUnit.Framework;
using Star.Models;

namespace Star.Pages
{
    public partial class HomePage
    {
        #region Validations

        public HomePage VerifyCurrentPageSize()
        {
            DinnersGridRows.Count.Should().Be(Test.DataCache<HomePageModel>().ExpectedPageSize);
            return this;
        }

        #endregion

        #region Base class validation overrides

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/", "Did not land on the home page");
            return true;
        }

        #endregion
    }
}
