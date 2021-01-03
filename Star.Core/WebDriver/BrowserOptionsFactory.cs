using System.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Star.Core.WebDriver
{
    public class BrowserOptionsFactory
    {
        public ChromeOptions GetChromeOptions(params BrowserOptionFlag[] flags)
        {
            var opts = new ChromeOptions();

            if (flags.Contains(BrowserOptionFlag.DisableImages))
            {
                opts.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
            }
            if (flags.Contains(BrowserOptionFlag.NoSandbox))
                opts.AddArgument("no-sandbox");

            return opts;
        }

        public FirefoxOptions GetFirefoxProfile(params BrowserOptionFlag[] flags)
        {
            var profile = new FirefoxOptions();

            if (flags.Contains(BrowserOptionFlag.DisableImages))
            {
                // Set arbitrarily high number to allow override of image loading (which is frozen) http://stackoverflow.com/a/34161948
                profile.SetPreference("browser.migration.version", 9001);
                profile.SetPreference("permissions.default.image", 2);
            }

            return profile;
        }

        public InternetExplorerOptions GetInternetExplorerOptions(params BrowserOptionFlag[] flags)
        {
            var opts = new InternetExplorerOptions();
            if (flags.Contains(BrowserOptionFlag.IgnoreSecurityDomains))
            {
                opts.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            }
            if (flags.Contains(BrowserOptionFlag.DisableNativeEvents))
            {
                opts.EnableNativeEvents = false;
            }

            return opts;
        }

        public EdgeOptions GetEdgeOptions(params BrowserOptionFlag[] flags)
        {
            var opts = new EdgeOptions
            {
                //UseInPrivateBrowsing = true
            };

            return opts;
        }
    }
}
