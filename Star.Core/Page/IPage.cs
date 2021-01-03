using OpenQA.Selenium;

namespace Star.Core.Page
{
    /// <summary>
    ///     Interface IPage will be used by all Web Page class definitions to assure consistency and reusability.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        ///     Validates that the driver is active on the page you've identified.
        ///     Some examples would be to see the webDriver.url contains "Login.aspx" in the case that you are building a page
        ///     class for a login page.
        ///     The constructor of the BasePage uses this to determine whether or not you are on the page and will give feedback to
        ///     the test class that it is invalid what you are trying to do.
        /// </summary>
        /// <returns><c>true</c> if the page is active, <c>false</c> if it is not.</returns>
        bool IsActive();

        IWebDriver WebDriver { get; }
    }
}
