using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Represents the top navigation header present on every page.
    /// Provides navigation to major sections and the shopping cart.
    /// </summary>
    public class HeaderPage : BasePage
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly By _myAccountLink        = By.XPath("//a[@class='ico-account']");
        private readonly By _logoutLink           = By.XPath("//a[@class='ico-logout']");
        private readonly By _loginLink            = By.XPath("//a[@class='ico-login']");
        private readonly By _registerLink         = By.XPath("//a[@class='ico-register']");
        private readonly By _shoppingCartLink     = By.XPath("//a[@class='ico-cart']");
        private readonly By _cartQtyBadge         = By.XPath("//span[@class='cart-qty']");
        private readonly By _searchBox            = By.XPath("//input[@id='small-searchterms']");
        private readonly By _searchButton         = By.XPath("//button[@class='button-1 search-box-button']");

        public HeaderPage(IWebDriver driver) : base(driver) { }

        /// <summary>Clicks the 'My Account' link in the header.</summary>
        public void ClickMyAccount()
        {
            WaitUntilClickable(_myAccountLink).Click();
            WaitForPageReady();
        }

        /// <summary>Clicks the 'Log out' link and waits for the home page to load.</summary>
        public void Logout()
        {
            WaitUntilClickable(_logoutLink).Click();
            WaitForPageReady();
        }

        /// <summary>Returns true if the logout link is visible (user is logged in).</summary>
        public bool IsUserLoggedIn()
        {
            var elements = Driver.FindElements(_logoutLink);
            return elements.Count > 0 && elements[0].Displayed;
        }

        /// <summary>Clicks the 'Log in' link in the header.</summary>
        public void ClickLogin()
        {
            WaitUntilClickable(_loginLink).Click();
            WaitForPageReady();
        }

        /// <summary>Clicks the 'Register' link in the header.</summary>
        public void ClickRegister()
        {
            WaitUntilClickable(_registerLink).Click();
            WaitForPageReady();
        }

        /// <summary>Clicks the shopping cart icon and navigates to the cart page.</summary>
        public void ClickShoppingCart()
        {
            WaitUntilClickable(_shoppingCartLink).Click();
            WaitForPageReady();
        }

        /// <summary>Returns the quantity displayed on the cart badge.</summary>
        public string GetCartQuantity()
        {
            var badge = Driver.FindElements(_cartQtyBadge);
            return badge.Count > 0 ? badge[0].Text.Trim('(', ')') : "0";
        }

        /// <summary>Performs a quick search using the header search box.</summary>
        public SearchResultsPage SearchFor(string keyword)
        {
            var box = WaitUntilVisible(_searchBox);
            box.Clear();
            box.SendKeys(keyword);
            WaitUntilClickable(_searchButton).Click();
            WaitForPageReady();
            return new SearchResultsPage(Driver);
        }
    }
}
