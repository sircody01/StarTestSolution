using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Star.Core.WebDriver
{
    public static class DesiredCapabilitiesFactory
    {
        public static DriverOptions GetCapability(WebDriverType driverType)
        {
            switch (driverType)
            {
                case WebDriverType.Chrome:
                    return new ChromeOptions();
                case WebDriverType.Firefox:
                    return new FirefoxOptions()
                    {
                        AcceptInsecureCertificates = true
                    };
                case WebDriverType.Edge:
                    return new EdgeOptions();
                case WebDriverType.InternetExplorer:
                    return new InternetExplorerOptions
                    {
                        BrowserVersion = "11",
                        EnableNativeEvents = false,
                        PlatformName = "Windows",
                        ForceShellWindowsApi = true,
                        EnsureCleanSession = true,
                        BrowserCommandLineArguments = "-private"
                    };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
