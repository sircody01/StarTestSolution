using OpenQA.Selenium;

namespace Star.Core
{
    public interface IWebTest
    {
        IWebDriver TestWebDriver { get; }
        string Scheme { get; }
        string Host { get; }
        string Country { get; }
        T DataCache<T>() where T : new();
    }
}
