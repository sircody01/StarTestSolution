﻿using NUnit.Framework;
using Star.Core;
using Star.Models;

namespace Star.Pages
{
    public partial class ChefPage : ProDinnerPage
    {
        public ChefPage(ref IWebTest test)
            : base(ref test)
        {

        }

        public ChefPage ChefActionOne(string chefMsg)
        {
            Test.DataCache<SimplePOCO>().Announcement = chefMsg;
            Test.Logger.Info("Doing chef action one");
            return this;
        }

        public ChefPage ChefActionTwo(string chefMsg)
        {
            Assert.AreEqual(chefMsg, Test.DataCache<SimplePOCO>().Announcement);
            Test.Logger.Info("Doing chef action two");
            return this;
        }

        public override bool IsActive()
        {
            Assert.AreEqual(WebDriver.Url, "https://prodinner.aspnetawesome.com/Chef", "Did not land on the chef page");
            return true;
        }
    }
}
