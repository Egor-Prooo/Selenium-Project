using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Page Object Model for an individual Product Details page.
    /// </summary>
    public class ProductPage : BasePage
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly By _productTitle        = By.XPath("//h1[@class='product-name']");
        private readonly By _productPrice        = By.XPath("//span[@id='price-value-1'] | //span[contains(@id,'price-value')]");
        private readonly By _addToCartButton     = By.XPath("//button[contains(@id,'add-to-cart-button')]");
        private readonly By _qtyInput            = By.XPath("//input[@id='product_enteredQuantity_1'] | //input[contains(@id,'product_enteredQuantity')]");
        private readonly By _successNotification = By.XPath("//p[@class='content' and contains(text(),'cart')]");
        private readonly By _notificationBar     = By.XPath("//div[@id='bar-notification']");
        private readonly By _closeNotification   = By.XPath("//span[@class='close']");

        public ProductPage(IWebDriver driver) : base(driver) { }

        /// <summary>Navigates to a product page by its URL slug.</summary>
        public ProductPage NavigateTo(string slug)
        {
            Driver.Navigate().GoToUrl($"{AppSettings.BaseUrl}/{slug}");
            WaitForPageReady();
            return this;
        }

        /// <summary>Returns the product name displayed on the page.</summary>
        public string GetProductName()
        {
            return WaitUntilVisible(_productTitle).Text.Trim();
        }

        /// <summary>Returns the product price as a decimal.</summary>
        public decimal GetPrice()
        {
            var priceText = WaitUntilVisible(_productPrice).Text
                .Replace("$", "")
                .Replace(",", "")
                .Trim();
            return decimal.Parse(priceText);
        }

        /// <summary>Sets the quantity field to the given value.</summary>
        public void SetQuantity(int qty)
        {
            var qtyField = Driver.FindElements(_qtyInput);
            if (qtyField.Count > 0)
            {
                qtyField[0].Clear();
                qtyField[0].SendKeys(qty.ToString());
            }
        }

        /// <summary>Clicks the 'Add to cart' button and waits for the notification bar.</summary>
        public void AddToCart()
        {
            var btn = WaitUntilClickable(_addToCartButton);
            ScrollIntoView(btn);
            btn.Click();
            // Wait for the notification bar to appear
            WaitUntilVisible(_notificationBar);
            WaitForPageReady();
        }

        /// <summary>Returns the text of the notification bar after adding to cart.</summary>
        public string GetNotificationText()
        {
            return WaitUntilVisible(_notificationBar).Text.Trim();
        }

        /// <summary>Closes the notification bar if it is visible.</summary>
        public void CloseNotification()
        {
            var closeBtn = Driver.FindElements(_closeNotification);
            if (closeBtn.Count > 0 && closeBtn[0].Displayed)
                closeBtn[0].Click();
        }
    }
}
