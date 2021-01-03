using OpenQA.Selenium;
using Star.Core;
using Star.Core.Page;
using Star.Core.Page.PageParts;

namespace Star.Pages.PartialPages
{
    public class AboutPartialPage : PagePart
    {
        public AboutPartialPage(IPage page, ref IWebTest test, IWebElement parentElement)
            : base(page, ref test, ref parentElement)
        {

        }
    }
}
