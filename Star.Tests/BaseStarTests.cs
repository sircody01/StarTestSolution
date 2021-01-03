using System;
using NUnit.Framework;
using Star.Core;

namespace Star.Tests
{
    public class BaseStarTests : BaseWebTest
    {
        protected Pages.ProDinnerApp ProDinner;

        [SetUp]
        public void TestInitialize()
        {
            BaseTestInitialize($"https://www.myapp.com/{ApplicationSettings.TargetRegion}", ApplicationSettings.StarDriverType);
            Host = new Uri(TestWebDriver.Url).Host;
            ProDinner = new Pages.ProDinnerApp(this);
        }
    }
}
