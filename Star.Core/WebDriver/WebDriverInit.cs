using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Star.Core.WebDriver
{
    public class WebDriverInit
    {
        #region Constants


        #endregion

        #region Public methods

        public static WebDriverType GetDriverType(string browserName)
        {
            if (browserName == null) return WebDriverType.Undefined;
            switch (browserName)
            {
                case "Chrome":
                    return WebDriverType.Chrome;
                case "InternetExplorer":
                case "IE":
                    return WebDriverType.InternetExplorer;
                case "Firefox":
                    return WebDriverType.Firefox;
                case "Edge":
                    return WebDriverType.Edge;

                default:
                    return WebDriverType.Chrome;
            }
        }

        #endregion

        #region Class variables


        #endregion

        #region Protected methods

        protected IWebDriver InitializeWebDriver(WebDriverType driverToUse)
        {
            IWebDriver driver;

            var optionsFactory = new BrowserOptionsFactory();
            var browserOptions = new[]
            {
                BrowserOptionFlag.NoSandbox,
                BrowserOptionFlag.IgnoreSecurityDomains,
                BrowserOptionFlag.DisableNativeEvents
            };

            switch (driverToUse)
            {
                case WebDriverType.Chrome:
                    var chromeOptions = optionsFactory.GetChromeOptions(browserOptions);
                    chromeOptions.UseSpecCompliantProtocol = true;
                    var chromeDriver = new ChromeDriver(ApplicationSettings.AssemblyDirectory, chromeOptions,
                        ApplicationSettings.SeleniumCommandTimeOutTimeSpan);
                    driver = chromeDriver;
                    break;
                case WebDriverType.Firefox:
                    // If we don't launch the FirefoxDriver this way, it runs VERY slowly in a .net core application
                    FirefoxDriverService geckoService = FirefoxDriverService.CreateDefaultService();
                    geckoService.Host = "::1";
                    var firefoxProfile = optionsFactory.GetFirefoxProfile(browserOptions);
                    var firefoxDriver = new FirefoxDriver(geckoService, firefoxProfile,
                        ApplicationSettings.SeleniumCommandTimeOutTimeSpan);
                    driver = firefoxDriver;
                    break;
                case WebDriverType.InternetExplorer:
                    var ieOptions = optionsFactory.GetInternetExplorerOptions(browserOptions);
                    var ieDriver = new InternetExplorerDriver(ApplicationSettings.AssemblyDirectory, ieOptions,
                        ApplicationSettings.SeleniumCommandTimeOutTimeSpan);
                    driver = ieDriver;
                    break;
                case WebDriverType.Edge:
                    // Trying to launch the EdgeDriver any other way results in "unknown error"
                    var service = EdgeDriverService.CreateDefaultService(ApplicationSettings.AssemblyDirectory, "msedgedriver.exe");
                    var edgeOptions = optionsFactory.GetEdgeOptions(browserOptions);
                    var edgeDriver = new EdgeDriver(service, edgeOptions);
                    driver = edgeDriver;
                    break;
                default:
                    throw new NotImplementedException("Unknow WebDriver type: " + driverToUse);
            }
            return driver;
        }

        #endregion
    }
}
