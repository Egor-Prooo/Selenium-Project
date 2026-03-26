using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Page Object Model for the Login page (/login).
    /// </summary>
    public class LoginPage : BasePage
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly By _emailInput         = By.XPath("//input[@id='Email']");
        private readonly By _passwordInput      = By.XPath("//input[@id='Password']");
        private readonly By _loginButton        = By.XPath("//button[@class='button-1 login-button']");
        private readonly By _errorMessage       = By.XPath("//div[contains(@class,'message-error')]//li");
        private readonly By _validationSummary  = By.XPath("//div[@class='validation-summary-errors']");
        private readonly By _forgotPasswordLink = By.XPath("//a[contains(@href,'passwordrecovery')]");
        private readonly By _rememberMeCheckbox = By.XPath("//input[@id='RememberMe']");
        private readonly By _registerLink       = By.XPath("//div[@class='new-wrapper']//a[contains(@href,'register')]");

        public LoginPage(IWebDriver driver) : base(driver) { }

        /// <summary>Navigates directly to the login page.</summary>
        public LoginPage NavigateTo()
        {
            Driver.Navigate().GoToUrl($"{AppSettings.BaseUrl}/login");
            WaitForPageReady();
            return this;
        }

        /// <summary>
        /// Fills in the login form and submits it using the provided DTO.
        /// </summary>
        public void Login(LoginDto dto)
        {
            var emailField = WaitUntilVisible(_emailInput);
            emailField.Clear();
            emailField.SendKeys(dto.Email);

            var passwordField = WaitUntilVisible(_passwordInput);
            passwordField.Clear();
            passwordField.SendKeys(dto.Password);

            WaitUntilClickable(_loginButton).Click();
            WaitForPageReady();
        }

        /// <summary>Returns the text of the first visible error/validation message.</summary>
        public string GetErrorMessage()
        {
            var errors = Driver.FindElements(_errorMessage);
            if (errors.Count > 0) return errors[0].Text.Trim();

            var summary = Driver.FindElements(_validationSummary);
            return summary.Count > 0 ? summary[0].Text.Trim() : string.Empty;
        }

        /// <summary>Returns true if an error message is displayed on the page.</summary>
        public bool IsErrorDisplayed()
            => Driver.FindElements(_errorMessage).Count > 0
            || Driver.FindElements(_validationSummary).Count > 0;

        /// <summary>Clicks the 'Forgot Password' link.</summary>
        public void ClickForgotPassword()
        {
            WaitUntilClickable(_forgotPasswordLink).Click();
            WaitForPageReady();
        }
    }
}
