using NUnit.Framework;
using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;

namespace EcommerceTests.Tests
{
    /// <summary>
    /// Base class for all test classes.
    /// Handles WebDriver initialization and teardown so every test starts clean.
    /// Also provides a shared <see cref="Driver"/> reference and a helper to log in
    /// quickly when login is a prerequisite rather than the subject under test.
    /// </summary>
    public abstract class BaseTest
    {
        protected IWebDriver Driver = null!;

        /// <summary>
        /// Initializes a fresh ChromeDriver before each test method.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            Driver = DriverFactory.InitDriver();
            Driver.Navigate().GoToUrl(AppSettings.BaseUrl);
            WaitForPageReady();
        }

        /// <summary>
        /// Quits and disposes the WebDriver after each test method.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            DriverFactory.QuitDriver();
        }

        // ── Shared helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Waits until the browser reports document.readyState === 'complete'
        /// and jQuery.active === 0.
        /// </summary>
        protected void WaitForPageReady()
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(
                Driver, TimeSpan.FromSeconds(15));

            wait.Until(d => ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState").Equals("complete"));

            try
            {
                wait.Until(d =>
                {
                    var result = ((IJavaScriptExecutor)d)
                        .ExecuteScript(
                            "return (typeof jQuery !== 'undefined') ? jQuery.active === 0 : true");
                    return result is bool b && b;
                });
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException) { /* jQuery may not exist */ }
        }

        /// <summary>
        /// Performs a silent login so tests that depend on being authenticated
        /// do not repeat login logic.
        /// </summary>
        protected void LoginAsRegisteredUser()
        {
            var loginPage = new Infrastructure.Pages.LoginPage(Driver);
            loginPage.NavigateTo();
            loginPage.Login(Infrastructure.Factories.LoginDtoFactory.GetValidUser());
            WaitForPageReady();
        }
    }
}
