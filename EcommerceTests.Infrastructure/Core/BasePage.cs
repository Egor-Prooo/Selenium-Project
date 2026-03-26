using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EcommerceTests.Infrastructure.Core
{
    /// <summary>
    /// Base class for all Page Object Models.
    /// Provides shared driver access, wait utilities, and JS helpers.
    /// </summary>
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        private const int DefaultTimeoutSeconds = 15;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultTimeoutSeconds));
        }

        /// <summary>
        /// Waits until the DOM readyState is 'complete' and all jQuery AJAX requests finish.
        /// </summary>
        protected void WaitForPageReady()
        {
            // Wait for DOM to fully load
            Wait.Until(d => ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState").Equals("complete"));

            // Wait for jQuery AJAX calls to finish (if jQuery is present on the page)
            try
            {
                Wait.Until(d =>
                {
                    var jqueryDone = ((IJavaScriptExecutor)d)
                        .ExecuteScript("return (typeof jQuery !== 'undefined') ? jQuery.active === 0 : true");
                    return jqueryDone is bool b && b;
                });
            }
            catch (WebDriverTimeoutException)
            {
                // jQuery might not be present on every page — safe to swallow
            }
        }

        /// <summary>
        /// Waits until the element located by <paramref name="locator"/> is visible and returns it.
        /// </summary>
        protected IWebElement WaitUntilVisible(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Waits until the element located by <paramref name="locator"/> is clickable and returns it.
        /// </summary>
        protected IWebElement WaitUntilClickable(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Scrolls the given element into view using JavaScript.
        /// </summary>
        protected void ScrollIntoView(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// Clicks an element using JavaScript — useful when a regular click is intercepted.
        /// </summary>
        protected void JsClick(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Returns the current page URL.
        /// </summary>
        public string GetCurrentUrl() => Driver.Url;
    }
}
