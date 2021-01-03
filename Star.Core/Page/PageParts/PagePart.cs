using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;

namespace Star.Core.Page.PageParts
{
    public interface IStarPagePart : IPage
    {
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class PagePart : IStarPagePart
    {
        #region Constructors

        protected PagePart(IPage page, ref IWebTest test, ref IWebElement parentElement)
        {
            ParentPage = page;
            WebDriver = page.WebDriver;
            Test = test;
            ParentElement = parentElement;
        }

        #endregion

        #region Class variables

        protected IWebTest Test;

        #endregion

        #region Properties

        public IWebDriver WebDriver { get; }
        private IWebElement ParentElement { get; }
        protected IPage ParentPage { get; }

        #endregion

        #region Public Methods

        public virtual bool IsActive()
        {
            throw new System.NotImplementedException();
        }

        #endregion    }
    }
}
