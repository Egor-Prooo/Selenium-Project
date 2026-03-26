using NUnit.Framework;
using EcommerceTests.Infrastructure.Pages;
using EcommerceTests.Infrastructure.Factories;
using EcommerceTests.Infrastructure.DTOs;

namespace EcommerceTests.Tests.CartTests
{
    /// <summary>
    /// Tests for the Shopping Cart functionality of the nopCommerce demo store.
    /// Covers adding, removing, updating products and the checkout flow.
    /// </summary>
    [TestFixture]
    public class CartTests : BaseTest
    {
        private ProductPage _productPage = null!;
        private CartPage    _cartPage    = null!;
        private HeaderPage  _headerPage  = null!;

        // Known product slugs on demo.nopcommerce.com
        private const string SimpleComputerSlug = "build-your-own-computer";
        private const string BookSlug            = "fahrenheit-451-by-ray-bradbury";

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _productPage = new ProductPage(Driver);
            _cartPage    = new CartPage(Driver);
            _headerPage  = new HeaderPage(Driver);

            // Ensure we start each cart test with an empty cart
            _cartPage.NavigateTo();
            // If items are present from a previous test, they will be cleaned in TearDown
        }

        [TearDown]
        public override void TearDown()
        {
            // Best-effort cart cleanup before quitting
            try
            {
                _cartPage.NavigateTo();
                var products = _cartPage.GetCartProducts();
                foreach (var p in products)
                    _cartPage.RemoveProduct(p.Name);
            }
            catch { /* ignore cleanup errors */ }

            base.TearDown();
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 1 – Add a product and verify it appears in the cart
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to the Simple Computer product page.
        ///   2. Click 'Add to cart'.
        ///   3. Navigate to /cart.
        /// Expected results:
        ///   - The cart contains exactly one item.
        ///   - The product DTO retrieved from the cart matches the expected DTO.
        /// </summary>
        [Test]
        public void AddProduct_ToCart_ProductAppearsInCart()
        {
            // Arrange
            var expected = ProductDtoFactory.GetSimpleProduct();

            // Act
            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();
            _cartPage.NavigateTo();

            var cartProducts = _cartPage.GetCartProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(cartProducts.Count,
                    Is.EqualTo(1),
                    "Cart should contain exactly one product after adding one item.");

                Assert.That(cartProducts[0].Name,
                    Is.EqualTo(expected.Name),
                    "The product name in the cart should match the expected DTO.");

                Assert.That(cartProducts[0].Price,
                    Is.EqualTo(expected.Price),
                    "The product price in the cart should match the expected DTO.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 2 – Remove a product from the cart
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Add a product to the cart.
        ///   2. Navigate to /cart.
        ///   3. Click the remove button for that product.
        /// Expected results:
        ///   - The cart is empty after removal.
        ///   - The empty cart message is displayed.
        /// </summary>
        [Test]
        public void RemoveProduct_FromCart_CartBecomesEmpty()
        {
            // Arrange — add item first
            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();
            _cartPage.NavigateTo();

            var expected = ProductDtoFactory.GetSimpleProduct();

            // Act
            _cartPage.RemoveProduct(expected.Name);

            // Assert
            Assert.That(_cartPage.IsCartEmpty(),
                Is.True,
                "Cart should be empty after removing the only product.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 3 – Update product quantity
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Add a product to the cart.
        ///   2. Navigate to /cart.
        ///   3. Update quantity to 2 and click 'Update shopping cart'.
        /// Expected results:
        ///   - The quantity field reflects the new value.
        ///   - The product DTO with quantity=2 matches the cart row.
        /// </summary>
        [Test]
        public void UpdateQuantity_InCart_ReflectsNewQuantity()
        {
            // Arrange
            var expectedDto = ProductDtoFactory.GetProductWithQuantityTwo();
            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();
            _cartPage.NavigateTo();

            // Act
            _cartPage.UpdateQuantity(expectedDto.Name, 2);

            var cartProducts = _cartPage.GetCartProducts();
            var actualProduct = cartProducts.FirstOrDefault(p => p.Name == expectedDto.Name);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualProduct,
                    Is.Not.Null,
                    "The product should still be present in the cart after updating quantity.");

                Assert.That(actualProduct!.Quantity,
                    Is.EqualTo(expectedDto.Quantity),
                    "The quantity in the cart should equal the value we set (2).");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 4 – Multiple products in cart
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Add 'Simple Computer' to the cart.
        ///   2. Add 'Fahrenheit 451' book to the cart.
        ///   3. Navigate to /cart.
        /// Expected results:
        ///   - The cart contains 2 distinct line items.
        ///   - Both product DTOs are present in the cart products list.
        /// </summary>
        [Test]
        public void AddMultipleProducts_ToCart_AllProductsPresent()
        {
            // Arrange
            var product1 = ProductDtoFactory.GetSimpleProduct();
            var product2 = ProductDtoFactory.GetSecondProduct();

            // Act
            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();
            _productPage.NavigateTo(BookSlug);
            _productPage.AddToCart();
            _cartPage.NavigateTo();

            var cartProducts = _cartPage.GetCartProducts();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(cartProducts.Count,
                    Is.EqualTo(2),
                    "Cart should contain exactly 2 items after adding two distinct products.");

                Assert.That(cartProducts.Any(p => p.Name.Contains(product1.Name, StringComparison.OrdinalIgnoreCase)),
                    Is.True,
                    $"Cart should contain '{product1.Name}'.");

                Assert.That(cartProducts.Any(p => p.Name.Contains(product2.Name, StringComparison.OrdinalIgnoreCase)),
                    Is.True,
                    $"Cart should contain '{product2.Name}'.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 5 – Proceed to checkout (purchase flow)
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Log in as a registered user.
        ///   2. Add a product to the cart.
        ///   3. Navigate to /cart.
        ///   4. Accept the terms of service and click Checkout.
        /// Expected results:
        ///   - The URL changes to /onepagecheckout or /checkout.
        ///   - The checkout page is reached successfully.
        /// </summary>
        [Test]
        public void ProceedToCheckout_WithItemInCart_NavigatesToCheckoutPage()
        {
            // Arrange — login is required for checkout
            LoginAsRegisteredUser();

            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();
            _cartPage.NavigateTo();

            // Act
            _cartPage.ProceedToCheckout();

            // Assert
            Assert.That(_cartPage.GetCurrentUrl(),
                Does.Contain("checkout").IgnoreCase,
                "After clicking Checkout the URL should contain 'checkout'.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 6 – Cart badge count updates
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Add a product to the cart.
        ///   2. Inspect the header cart badge.
        /// Expected results:
        ///   - The cart badge shows a quantity of at least 1.
        /// </summary>
        [Test]
        public void AddProduct_CartBadge_ShowsCorrectQuantity()
        {
            // Act
            _productPage.NavigateTo(SimpleComputerSlug);
            _productPage.AddToCart();

            var badgeQty = _headerPage.GetCartQuantity();

            // Assert
            Assert.That(int.Parse(badgeQty),
                Is.GreaterThanOrEqualTo(1),
                "The cart badge in the header should show at least 1 item after adding a product.");
        }
    }
}
