namespace Star.Core.Exceptions
{
    internal class ErrorMessages
    {
        public const string InvalidConfigurationSelected =
            "The configuration you've selected is not supported configuration. Valid values include Chrome, Firefox, InternetExplorer, Edge, RemoteWebDriver.";

        public const string PageNotLoaded = "The Page failed to load properly. See Exception details.";
    }
}
