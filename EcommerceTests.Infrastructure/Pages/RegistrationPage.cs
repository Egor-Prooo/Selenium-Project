using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Page Object Model for the Registration page (/register).
    /// </summary>
    public class RegistrationPage : BasePage
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly By _firstNameInput      = By.XPath("//input[@id='FirstName']");
        private readonly By _lastNameInput       = By.XPath("//input[@id='LastName']");
        private readonly By _emailInput          = By.XPath("//input[@id='Email']");
        private readonly By _companyCheckbox     = By.XPath("//input[@id='Company']");
        private readonly By _companyNameInput    = By.XPath("//input[@id='Company']");
        private readonly By _passwordInput       = By.XPath("//input[@id='Password']");
        private readonly By _confirmPasswordInput= By.XPath("//input[@id='ConfirmPassword']");
        private readonly By _registerButton      = By.XPath("//button[@id='register-button']");
        private readonly By _successMessage      = By.XPath("//div[@class='result']");
        private readonly By _validationErrors    = By.XPath("//span[contains(@class,'field-validation-error')]");
        private readonly By _summaryErrors       = By.XPath("//div[contains(@class,'message-error')]//li");
        private readonly By _genderMaleRadio     = By.XPath("//input[@id='gender-male']");

        public RegistrationPage(IWebDriver driver) : base(driver) { }

        /// <summary>Navigates directly to the registration page.</summary>
        public RegistrationPage NavigateTo()
        {
            Driver.Navigate().GoToUrl($"{AppSettings.BaseUrl}/register");
            WaitForPageReady();
            return this;
        }

        /// <summary>
        /// Fills in all registration form fields using the provided DTO and submits.
        /// </summary>
        public void Register(RegistrationDto dto)
        {
            WaitUntilVisible(_firstNameInput).Clear();
            Driver.FindElement(_firstNameInput).SendKeys(dto.FirstName);

            Driver.FindElement(_lastNameInput).Clear();
            Driver.FindElement(_lastNameInput).SendKeys(dto.LastName);

            Driver.FindElement(_emailInput).Clear();
            Driver.FindElement(_emailInput).SendKeys(dto.Email);

            Driver.FindElement(_passwordInput).Clear();
            Driver.FindElement(_passwordInput).SendKeys(dto.Password);

            Driver.FindElement(_confirmPasswordInput).Clear();
            Driver.FindElement(_confirmPasswordInput).SendKeys(dto.ConfirmPassword);

            WaitUntilClickable(_registerButton).Click();
            WaitForPageReady();
        }

        /// <summary>Returns the success message text after successful registration.</summary>
        public string GetSuccessMessage()
        {
            var elements = Driver.FindElements(_successMessage);
            return elements.Count > 0 ? elements[0].Text.Trim() : string.Empty;
        }

        /// <summary>Returns the first inline field validation error text.</summary>
        public string GetFirstValidationError()
        {
            var errors = Driver.FindElements(_validationErrors);
            if (errors.Count > 0) return errors[0].Text.Trim();

            var summary = Driver.FindElements(_summaryErrors);
            return summary.Count > 0 ? summary[0].Text.Trim() : string.Empty;
        }

        /// <summary>Returns true if any validation error is displayed.</summary>
        public bool HasValidationErrors()
            => Driver.FindElements(_validationErrors).Any(e => e.Displayed)
            || Driver.FindElements(_summaryErrors).Any(e => e.Displayed);

        /// <summary>Returns a list of all visible error messages.</summary>
        public List<string> GetAllValidationErrors()
        {
            var errors = new List<string>();
            errors.AddRange(Driver.FindElements(_validationErrors)
                .Where(e => e.Displayed).Select(e => e.Text.Trim()));
            errors.AddRange(Driver.FindElements(_summaryErrors)
                .Where(e => e.Displayed).Select(e => e.Text.Trim()));
            return errors;
        }
    }
}
