using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Page Object Model for the Shopping Cart page (/cart).
    /// </summary>
    public class CartPage : BasePage
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly By _cartRows            = By.XPath("//tr[contains(@class,'cart-item-row')]");
        private readonly By _emptyCartMessage    = By.XPath("//div[@class='no-data' and contains(.,'empty')]");
        private readonly By _termsCheckbox       = By.XPath("//input[@id='termsofservice']");
        private readonly By _checkoutButton      = By.XPath("//button[@id='checkout']");
        private readonly By _updateCartButton    = By.XPath("//button[@name='updatecart']");
        private readonly By _continueShoppingBtn = By.XPath("//button[@class='button-2 continue-shopping-button']");

        // Within a cart row
        private const string RowProductName  = ".//td[contains(@class,'product')]//a";
        private const string RowPrice        = ".//td[contains(@class,'unit-price')]//span";
        private const string RowQtyInput     = ".//input[contains(@class,'qty-input')]";
        private const string RowSubtotal     = ".//td[contains(@class,'subtotal')]//span";
        private const string RowRemoveButton = ".//td[contains(@class,'remove-from-cart')]//button";

        public CartPage(IWebDriver driver) : base(driver) { }

        /// <summary>Navigates directly to the cart page.</summary>
        public CartPage NavigateTo()
        {
            Driver.Navigate().GoToUrl($"{AppSettings.BaseUrl}/cart");
            WaitForPageReady();
            return this;
        }

        /// <summary>
        /// Returns a list of <see cref="ProductDto"/> objects representing every item in the cart.
        /// </summary>
        public List<ProductDto> GetCartProducts()
        {
            var products = new List<ProductDto>();
            var rows = Driver.FindElements(_cartRows);

            foreach (var row in rows)
            {
                var nameEl  = row.FindElements(By.XPath(RowProductName));
                var priceEl = row.FindElements(By.XPath(RowPrice));
                var qtyEl   = row.FindElements(By.XPath(RowQtyInput));

                if (nameEl.Count == 0) continue;

                var name  = nameEl[0].Text.Trim();
                var price = priceEl.Count > 0
                    ? decimal.Parse(priceEl[0].Text.Replace("$", "").Replace(",", "").Trim())
                    : 0m;
                var qty = qtyEl.Count > 0
                    ? int.Parse(qtyEl[0].GetAttribute("value") ?? "1")
                    : 1;

                products.Add(new ProductDto { Name = name, Price = price, Quantity = qty });
            }

            return products;
        }

        /// <summary>Returns true if the cart is empty.</summary>
        public bool IsCartEmpty()
        {
            return Driver.FindElements(_emptyCartMessage).Count > 0;
        }

        /// <summary>Removes the first product whose name matches <paramref name="productName"/>.</summary>
        public void RemoveProduct(string productName)
        {
            var rows = Driver.FindElements(_cartRows);
            foreach (var row in rows)
            {
                var nameEl = row.FindElements(By.XPath(RowProductName));
                if (nameEl.Count > 0 && nameEl[0].Text.Trim().Contains(productName))
                {
                    var removeBtn = row.FindElement(By.XPath(RowRemoveButton));
                    JsClick(removeBtn);
                    WaitForPageReady();
                    return;
                }
            }
            throw new NoSuchElementException($"Product '{productName}' not found in cart.");
        }

        /// <summary>Updates the quantity of a cart item and clicks the Update Cart button.</summary>
        public void UpdateQuantity(string productName, int qty)
        {
            var rows = Driver.FindElements(_cartRows);
            foreach (var row in rows)
            {
                var nameEl = row.FindElements(By.XPath(RowProductName));
                if (nameEl.Count > 0 && nameEl[0].Text.Trim().Contains(productName))
                {
                    var qtyInput = row.FindElement(By.XPath(RowQtyInput));
                    qtyInput.Clear();
                    qtyInput.SendKeys(qty.ToString());

                    WaitUntilClickable(_updateCartButton).Click();
                    WaitForPageReady();
                    return;
                }
            }
            throw new NoSuchElementException($"Product '{productName}' not found in cart.");
        }

        /// <summary>
        /// Accepts the terms of service and clicks the Checkout button.
        /// </summary>
        public void ProceedToCheckout()
        {
            var terms = WaitUntilClickable(_termsCheckbox);
            if (!terms.Selected)
                terms.Click();

            WaitUntilClickable(_checkoutButton).Click();
            WaitForPageReady();
        }

        /// <summary>Returns the number of distinct line items in the cart.</summary>
        public int GetCartItemCount()
        {
            return Driver.FindElements(_cartRows).Count;
        }

        /// <summary>Returns the subtotal text for a named product.</summary>
        public string GetSubtotalForProduct(string productName)
        {
            var rows = Driver.FindElements(_cartRows);
            foreach (var row in rows)
            {
                var nameEl = row.FindElements(By.XPath(RowProductName));
                if (nameEl.Count > 0 && nameEl[0].Text.Trim().Contains(productName))
                {
                    var subtotalEl = row.FindElements(By.XPath(RowSubtotal));
                    return subtotalEl.Count > 0 ? subtotalEl[0].Text.Trim() : string.Empty;
                }
            }
            return string.Empty;
        }
    }
}
