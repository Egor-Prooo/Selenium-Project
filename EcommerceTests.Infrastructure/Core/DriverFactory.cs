using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EcommerceTests.Infrastructure.Core
{
    /// <summary>
    /// Manages creation and lifetime of the WebDriver instance.
    /// </summary>
    public static class DriverFactory
    {
        private static IWebDriver? _driver;

        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                    throw new InvalidOperationException("Driver has not been initialized. Call InitDriver() first.");
                return _driver;
            }
        }

        /// <summary>
        /// Initializes a headless-capable ChromeDriver with standard options.
        /// </summary>
        public static IWebDriver InitDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            // Uncomment for headless CI runs:
            // options.AddArgument("--headless");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return _driver;
        }

        /// <summary>
        /// Quits the driver and releases all resources.
        /// </summary>
        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
