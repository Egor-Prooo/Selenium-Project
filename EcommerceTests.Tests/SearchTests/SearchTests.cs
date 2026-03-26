using NUnit.Framework;
using EcommerceTests.Infrastructure.Pages;
using EcommerceTests.Infrastructure.Factories;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Tests.SearchTests
{
    /// <summary>
    /// Tests for the Search functionality of the nopCommerce demo store.
    /// Covers basic search, advanced search filters, no-results scenarios,
    /// and data-driven keyword tests.
    /// URL: https://demo.nopcommerce.com/search
    /// </summary>
    [TestFixture]
    public class SearchTests : BaseTest
    {
        private SearchResultsPage _searchPage = null!;
        private HeaderPage _headerPage = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _searchPage  = new SearchResultsPage(Driver);
            _headerPage  = new HeaderPage(Driver);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 1 – Basic keyword search returns results
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Enter keyword 'apple' and submit.
        /// Expected results:
        ///   - At least one product result is displayed.
        ///   - Each result DTO has a non-empty name.
        /// </summary>
        [Test]
        public void Search_WithValidKeyword_ReturnsResults()
        {
            // Arrange
            var dto = SearchOptionsDtoFactory.GetBasicSearch("apple");

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            var results = _searchPage.GetResultProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results.Count,
                    Is.GreaterThan(0),
                    "Searching for 'apple' should return at least one result.");

                Assert.That(results.All(p => !string.IsNullOrEmpty(p.Name)),
                    Is.True,
                    "Every result DTO should have a non-empty product name.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 2 – No results search
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Enter a keyword guaranteed to match nothing.
        /// Expected results:
        ///   - No product results are shown.
        ///   - A 'no result' message is displayed.
        /// </summary>
        [Test]
        public void Search_WithNonExistentKeyword_ShowsNoResultsMessage()
        {
            // Arrange
            var dto = SearchOptionsDtoFactory.GetNoResultsSearch();

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_searchPage.HasNoResults(),
                    Is.True,
                    "A 'no results' message should appear for a keyword that matches nothing.");

                Assert.That(_searchPage.GetResultCount(),
                    Is.EqualTo(0),
                    "Result count should be zero for a keyword that matches nothing.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 3 – Category-scoped search
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Search for 'computer' scoped to the Computers category.
        /// Expected results:
        ///   - Results are returned.
        ///   - All result DTOs have a non-zero price (they are real products).
        /// </summary>
        [Test]
        public void Search_WithCategoryFilter_ReturnsFilteredResults()
        {
            // Arrange
            var dto = SearchOptionsDtoFactory.GetComputersSearch();

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            var results = _searchPage.GetResultProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results.Count,
                    Is.GreaterThan(0),
                    "A category-scoped search for 'computer' should return results.");

                Assert.That(results.All(p => p.Price > 0),
                    Is.True,
                    "All returned product DTOs should have a positive price.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 4 – Search with description scanning enabled
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Search for 'Intel' with 'Search in descriptions' checked.
        /// Expected results:
        ///   - Results are returned (Intel appears in product descriptions).
        /// </summary>
        [Test]
        public void Search_WithSearchInDescriptions_ReturnsResults()
        {
            // Arrange
            var dto = SearchOptionsDtoFactory.GetSearchInDescriptions("Intel");

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            var results = _searchPage.GetResultProducts();

            // Assert
            Assert.That(results.Count,
                Is.GreaterThan(0),
                "Searching in product descriptions for 'Intel' should yield at least one result.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 5 – Price range filter
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Search for 'notebook' with a price range of $0 – $1000.
        /// Expected results:
        ///   - All returned product DTOs have a price within [0, 1000].
        /// </summary>
        [Test]
        public void Search_WithPriceRangeFilter_ReturnsProductsWithinRange()
        {
            // Arrange
            const decimal priceFrom = 0m;
            const decimal priceTo   = 1000m;
            var dto = SearchOptionsDtoFactory.GetSearchWithPriceRange("notebook", priceFrom, priceTo);

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            var results = _searchPage.GetResultProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results.Count,
                    Is.GreaterThan(0),
                    "Searching for 'notebook' with a price filter should return results.");

                Assert.That(results.All(p => p.Price >= priceFrom && p.Price <= priceTo),
                    Is.True,
                    $"All result DTOs should have a price between {priceFrom} and {priceTo}.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 6 – Header quick-search navigates to results page
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. From the header search box, search for 'phone'.
        /// Expected results:
        ///   - The URL changes to /search.
        ///   - Results are displayed.
        /// </summary>
        [Test]
        public void QuickSearch_FromHeader_NavigatesToSearchResultsPage()
        {
            // Act
            _headerPage.SearchFor("phone");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_searchPage.GetCurrentUrl(),
                    Does.Contain("/search"),
                    "The URL should navigate to the /search page after a header quick search.");

                Assert.That(_searchPage.GetResultCount(),
                    Is.GreaterThan(0),
                    "Searching for 'phone' from the header should return at least one result.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 7 – Data-Driven: multiple keywords all return results
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /search.
        ///   2. Search for the given keyword from the TestCase data.
        /// Expected results:
        ///   - At least one result is returned for each keyword.
        /// </summary>
        [TestCase("laptop",   TestName = "DataDriven_Search_Laptop")]
        [TestCase("book",     TestName = "DataDriven_Search_Book")]
        [TestCase("camera",   TestName = "DataDriven_Search_Camera")]
        [TestCase("jewelry",  TestName = "DataDriven_Search_Jewelry")]
        public void Search_DataDriven_ValidKeywords_ReturnResults(string keyword)
        {
            // Arrange
            var dto = SearchOptionsDtoFactory.GetBasicSearch(keyword);

            // Act
            _searchPage.NavigateTo();
            _searchPage.PerformSearch(dto);

            var results = _searchPage.GetResultProducts();

            // Assert
            Assert.That(results.Count,
                Is.GreaterThan(0),
                $"Searching for '{keyword}' should return at least one product result.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 8 – DTO equality: two identical search option DTOs are equal
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Create two SearchOptionsDto objects with identical parameters.
        ///   2. Compare them using Equals().
        /// Expected results:
        ///   - Both DTOs are equal, validating the DTO contract.
        /// </summary>
        [Test]
        public void SearchOptionsDto_WithSameParameters_AreEqual()
        {
            // Arrange
            var dto1 = SearchOptionsDtoFactory.GetComputersSearch();
            var dto2 = SearchOptionsDtoFactory.GetComputersSearch();

            // Assert
            Assert.That(dto1, Is.EqualTo(dto2),
                "Two SearchOptionsDto objects built from the same factory method should be equal.");
        }
    }
}
