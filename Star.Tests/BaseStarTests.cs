using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Star.Core;

namespace Star.Tests
{
    public class BaseStarTests : BaseWebTest
    {
        protected Pages.ProDinnerApp ProDinner;

        [SetUp]
        public void TestInitialize()
        {
            BaseTestInitialize($"https://prodinner.aspnetawesome.com/", ApplicationSettings.StarDriverType);
            // Suppress the "Accept cookies" message
            TestWebDriver.Manage().Cookies.AddCookie(new Cookie("CookieMsg", "", "aspnetawesome.com", "/", DateTime.Now.AddMinutes(120)));
            TestWebDriver.Navigate().Refresh();

            Host = new Uri(TestWebDriver.Url).Host;
            ProDinner = new Pages.ProDinnerApp(this);
        }
    }
}
