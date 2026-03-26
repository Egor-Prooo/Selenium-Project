using OpenQA.Selenium;
using EcommerceTests.Infrastructure.Core;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Infrastructure.Pages
{
    /// <summary>
    /// Page Object Model for the Search Results page (/search).
    /// Covers both the quick header search and the advanced search form.
    /// </summary>
    public class SearchResultsPage : BasePage
    {
        // ── Advanced search form locators ────────────────────────────────────────
        private readonly By _searchKeywordInput      = By.XPath("//input[@id='q']");
        private readonly By _advancedSearchCheckbox  = By.XPath("//input[@id='advs']");
        private readonly By _searchInDescCheckbox    = By.XPath("//input[@id='isc']");
        private readonly By _categoryDropdown        = By.XPath("//select[@id='cid']");
        private readonly By _priceFromInput          = By.XPath("//input[@id='pf']");
        private readonly By _priceToInput            = By.XPath("//input[@id='pt']");
        private readonly By _searchButton            = By.XPath("//button[@class='button-1 search-button']");

        // ── Results area locators ────────────────────────────────────────────────
        private readonly By _productItems      = By.XPath("//div[contains(@class,'product-item')]");
        private readonly By _noResultsMessage  = By.XPath("//div[@class='search-results']//div[@class='no-result']");
        private readonly By _searchTitle       = By.XPath("//div[@class='page-title']/h1");

        // Within a result item
        private const string ItemName  = ".//h2[@class='product-title']/a";
        private const string ItemPrice = ".//span[@class='price actual-price']";

        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        /// <summary>Navigates directly to the search page.</summary>
        public SearchResultsPage NavigateTo()
        {
            Driver.Navigate().GoToUrl($"{AppSettings.BaseUrl}/search");
            WaitForPageReady();
            return this;
        }

        /// <summary>
        /// Performs an advanced search using all fields provided in the <see cref="SearchOptionsDto"/>.
        /// </summary>
        public void PerformSearch(SearchOptionsDto dto)
        {
            // Set keyword
            var keywordInput = WaitUntilVisible(_searchKeywordInput);
            keywordInput.Clear();
            keywordInput.SendKeys(dto.Keyword);

            // Expand advanced search if needed
            if (dto.SearchInDescriptions || dto.CategoryName != null
                || dto.PriceFrom.HasValue || dto.PriceTo.HasValue)
            {
                var advCheckbox = Driver.FindElement(_advancedSearchCheckbox);
                if (!advCheckbox.Selected)
                    advCheckbox.Click();

                if (dto.SearchInDescriptions)
                {
                    var descCheckbox = Driver.FindElement(_searchInDescCheckbox);
                    if (!descCheckbox.Selected)
                        descCheckbox.Click();
                }

                if (!string.IsNullOrEmpty(dto.CategoryName))
                {
                    var catDrop = new OpenQA.Selenium.Support.UI.SelectElement(
                        Driver.FindElement(_categoryDropdown));
                    catDrop.SelectByText(dto.CategoryName);
                }

                if (dto.PriceFrom.HasValue)
                {
                    var pfInput = Driver.FindElement(_priceFromInput);
                    pfInput.Clear();
                    pfInput.SendKeys(dto.PriceFrom.Value.ToString("F0"));
                }

                if (dto.PriceTo.HasValue)
                {
                    var ptInput = Driver.FindElement(_priceToInput);
                    ptInput.Clear();
                    ptInput.SendKeys(dto.PriceTo.Value.ToString("F0"));
                }
            }

            WaitUntilClickable(_searchButton).Click();
            WaitForPageReady();
        }

        /// <summary>
        /// Returns a list of <see cref="ProductDto"/> objects for every visible result item.
        /// </summary>
        public List<ProductDto> GetResultProducts()
        {
            var products = new List<ProductDto>();
            var items = Driver.FindElements(_productItems);

            foreach (var item in items)
            {
                var nameEls  = item.FindElements(By.XPath(ItemName));
                var priceEls = item.FindElements(By.XPath(ItemPrice));

                if (nameEls.Count == 0) continue;

                var name  = nameEls[0].Text.Trim();
                var price = priceEls.Count > 0
                    ? decimal.Parse(priceEls[0].Text.Replace("$", "").Replace(",", "").Trim())
                    : 0m;

                products.Add(new ProductDto { Name = name, Price = price });
            }

            return products;
        }

        /// <summary>Returns the number of results displayed.</summary>
        public int GetResultCount()
        {
            return Driver.FindElements(_productItems).Count;
        }

        /// <summary>Returns true when the "no results" message is displayed.</summary>
        public bool HasNoResults()
        {
            return Driver.FindElements(_noResultsMessage).Count > 0;
        }

        /// <summary>Returns the no-results message text.</summary>
        public string GetNoResultsMessage()
        {
            var els = Driver.FindElements(_noResultsMessage);
            return els.Count > 0 ? els[0].Text.Trim() : string.Empty;
        }

        /// <summary>Returns true if every result item's name contains the keyword.</summary>
        public bool AllResultsContainKeyword(string keyword)
        {
            var products = GetResultProducts();
            return products.All(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
