﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Star.Core.WebDriver;

namespace Star.Core
{
    public static class ApplicationSettings
    {
        #region Private Dependent Class

        private class AppSettings
        {
            public int ElementLoadTimeout { get; set; }
            public int PageLoadTimeout { get; set; }
            public int SeleniumCommandTimeout { get; set; }
            public string StarDriverType { get; set; }
            public string ScreenResolution { get; set; }
            public string TargetEnvironment { get; set; }
            public string TargetCountry { get; set; }
            public string TargetLanguage { get; set; }
            public string MongoDBConnectionString { get; set; }
            public string TestResultsDb { get; set; }
            public string TestResultsCollection { get; set; }
        }

        #endregion

        #region Constants

        public const int MethodTimeout = 900000; // A 15 minute timeout (900,000 milliseconds) used by most tests

        #endregion

        #region Class variables

        private static readonly AppSettings Settings;

        #endregion

        #region Constructors

        static ApplicationSettings()
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            Settings = configuration.GetSection("AppSettings").Get<AppSettings>();
        }

        #endregion

        #region Properties

        public static string TargetEnvironment => Settings.TargetEnvironment;
        public static string TargetCountry => Settings.TargetCountry;
        public static string TargetLanguage => Settings.TargetLanguage;
        public static string MachineName => Environment.MachineName;
        public static TimeSpan ElementLoadTimeSpan => TimeSpan.FromSeconds(Settings.ElementLoadTimeout);
        public static TimeSpan PageLoadTimeSpan => TimeSpan.FromSeconds(Settings.PageLoadTimeout);
        public static TimeSpan SeleniumCommandTimeOutTimeSpan => TimeSpan.FromSeconds(Settings.SeleniumCommandTimeout);
        public static WebDriverType StarDriverType => Enum.Parse<WebDriverType>(Settings.StarDriverType);
        public static string MongoDBConnectionString => Settings.MongoDBConnectionString;
        public static string TestResultsDb => Settings.TestResultsDb;
        public static string TestResultsCollection => Settings.TestResultsCollection;

        public static Size ScreenResolution
        {
            get
            {
                try
                {
                    var a = Settings.ScreenResolution.Split(new char[] { 'x' });
                    return new Size()
                    {
                        Width = int.Parse(a[0]),
                        Height = int.Parse(a[1])
                    };
                }
                catch (Exception) { }
                return Size.Empty;
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string OperatingSystemName()
        {
            var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            // The .Get() takes a few seconds to execute. I can't find a way to speed it up, or a better alternative that works for Windows 10 and above.
            var osName =
                searcher.Get().Cast<ManagementObject>().Select(o => o.GetPropertyValue("Caption")).FirstOrDefault();
            return osName?.ToString() ?? "Unknown";
        }

        #endregion
    }
}
