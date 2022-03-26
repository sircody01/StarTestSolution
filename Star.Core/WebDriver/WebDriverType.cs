namespace Star.Core.WebDriver
{
    /// <summary>
    /// The Browser types that are supported.
    /// </summary>
    public enum WebDriverType
    {
        /// <summary>
        /// When the driver type specified in appsettings is unrecognized
        /// </summary>
        Undefined,

        /// <summary>
        /// Mozilla Firefox
        /// </summary>
        Firefox,

        /// <summary>
        /// Microsoft Internet Explorer
        /// </summary>
        InternetExplorer,

        /// <summary>
        /// Google Chrome
        /// </summary>
        Chrome,

        /// <summary>
        /// The Microsoft Edge Browser
        /// </summary>
        Edge
    }
}
