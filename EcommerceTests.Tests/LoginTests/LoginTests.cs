using NUnit.Framework;
using EcommerceTests.Infrastructure.Pages;
using EcommerceTests.Infrastructure.Factories;
using EcommerceTests.Infrastructure.Core;

namespace EcommerceTests.Tests.LoginTests
{
    /// <summary>
    /// Tests for the Login functionality of the nopCommerce demo store.
    /// URL: https://demo.nopcommerce.com/login
    /// </summary>
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private HeaderPage _headerPage = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _loginPage  = new LoginPage(Driver);
            _headerPage = new HeaderPage(Driver);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 1 – Happy Path
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /login.
        ///   2. Enter valid credentials from LoginDtoFactory.GetValidUser().
        ///   3. Click the Login button.
        /// Expected results:
        ///   - The user is redirected away from /login.
        ///   - The header shows a 'Log out' link, confirming the session is active.
        /// </summary>
        [Test]
        public void Login_WithValidCredentials_RedirectsToHomePage()
        {
            // Arrange
            var loginDto = LoginDtoFactory.GetValidUser();

            // Act
            _loginPage.NavigateTo();
            _loginPage.Login(loginDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_headerPage.IsUserLoggedIn(),
                    Is.True,
                    "User should be logged in after providing valid credentials.");

                Assert.That(_loginPage.GetCurrentUrl(),
                    Does.Not.Contain("/login"),
                    "URL should no longer be the login page after a successful login.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 2 – Wrong password
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /login.
        ///   2. Enter a valid email with an incorrect password.
        ///   3. Click the Login button.
        /// Expected results:
        ///   - The page stays on /login.
        ///   - An error message is displayed.
        ///   - The user is NOT logged in.
        /// </summary>
        [Test]
        public void Login_WithInvalidPassword_ShowsErrorMessage()
        {
            // Arrange
            var loginDto = LoginDtoFactory.GetInvalidPassword();

            // Act
            _loginPage.NavigateTo();
            _loginPage.Login(loginDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorDisplayed(),
                    Is.True,
                    "An error message should be displayed when the password is wrong.");

                Assert.That(_headerPage.IsUserLoggedIn(),
                    Is.False,
                    "User should NOT be logged in after entering a wrong password.");

                Assert.That(_loginPage.GetCurrentUrl(),
                    Does.Contain("/login"),
                    "The URL should still point to the login page after a failed attempt.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 3 – Unregistered email
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /login.
        ///   2. Enter an email that has never been registered.
        ///   3. Click the Login button.
        /// Expected results:
        ///   - An error message is visible.
        ///   - The user remains on the login page.
        /// </summary>
        [Test]
        public void Login_WithUnregisteredEmail_ShowsErrorMessage()
        {
            // Arrange
            var loginDto = LoginDtoFactory.GetUnregisteredUser();

            // Act
            _loginPage.NavigateTo();
            _loginPage.Login(loginDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorDisplayed(),
                    Is.True,
                    "An error should appear when logging in with an unregistered email.");

                Assert.That(_loginPage.GetCurrentUrl(),
                    Does.Contain("/login"),
                    "URL should remain on the login page after an unsuccessful attempt.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 4 – Empty email field
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /login.
        ///   2. Leave the email field empty, enter any password.
        ///   3. Click the Login button.
        /// Expected results:
        ///   - A validation error is displayed for the email field.
        ///   - The user is not logged in.
        /// </summary>
        [Test]
        public void Login_WithEmptyEmail_ShowsValidationError()
        {
            // Arrange
            var loginDto = LoginDtoFactory.GetEmptyEmail();

            // Act
            _loginPage.NavigateTo();
            _loginPage.Login(loginDto);

            // Assert
            Assert.That(_loginPage.IsErrorDisplayed(),
                Is.True,
                "A validation error should be shown when the email field is empty.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 5 – DTO comparison
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Create two LoginDto objects via the factory using the same preset.
        ///   2. Compare them using Equals().
        /// Expected results:
        ///   - Both DTOs are equal (same email and password fields).
        ///   This validates the DTO Equals() contract used across all assertions.
        /// </summary>
        [Test]
        public void LoginDto_CreatedByFactory_AreEqualForSamePreset()
        {
            // Arrange
            var dto1 = LoginDtoFactory.GetValidUser();
            var dto2 = LoginDtoFactory.GetValidUser();

            // Assert
            Assert.That(dto1, Is.EqualTo(dto2),
                "Two LoginDtos created from the same factory method should be equal.");
        }
    }
}
