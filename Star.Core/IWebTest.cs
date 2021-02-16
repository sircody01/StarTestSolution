using log4net;
using OpenQA.Selenium;

namespace Star.Core
{
    public interface IWebTest
    {
        IWebDriver TestWebDriver { get; }
        string Scheme { get; }
        string Host { get; }
        string Country { get; }
        public int PostSharpIndent { get; set; }
        public ILog PostSharpLogger { get; }
        public ILog Logger { get; }
        T DataCache<T>() where T : new();
    }
}
