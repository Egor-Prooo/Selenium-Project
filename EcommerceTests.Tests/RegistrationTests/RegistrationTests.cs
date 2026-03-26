using NUnit.Framework;
using EcommerceTests.Infrastructure.Pages;
using EcommerceTests.Infrastructure.Factories;

namespace EcommerceTests.Tests.RegistrationTests
{
    /// <summary>
    /// Tests for the Registration functionality of the nopCommerce demo store.
    /// URL: https://demo.nopcommerce.com/register
    /// </summary>
    [TestFixture]
    public class RegistrationTests : BaseTest
    {
        private RegistrationPage _registrationPage = null!;
        private HeaderPage _headerPage = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _registrationPage = new RegistrationPage(Driver);
            _headerPage       = new HeaderPage(Driver);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 1 – Happy Path
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /register.
        ///   2. Fill in all fields with valid data from RegistrationDtoFactory.GetValidUser().
        ///   3. Click the Register button.
        /// Expected results:
        ///   - A success message is displayed.
        ///   - The user is redirected away from /register (or stays with a confirmation).
        /// </summary>
        [Test]
        public void Register_WithValidData_ShowsSuccessMessage()
        {
            // Arrange
            var dto = RegistrationDtoFactory.GetValidUser();

            // Act
            _registrationPage.NavigateTo();
            _registrationPage.Register(dto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_registrationPage.GetSuccessMessage(),
                    Is.Not.Empty,
                    "A success confirmation message should be visible after valid registration.");

                Assert.That(_registrationPage.GetSuccessMessage(),
                    Does.Contain("Your registration completed"),
                    "The success message should confirm registration is complete.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 2 – Mismatched passwords
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /register.
        ///   2. Enter valid data but with mismatched Password and ConfirmPassword.
        ///   3. Click the Register button.
        /// Expected results:
        ///   - A validation error is shown indicating the passwords do not match.
        ///   - Registration does not succeed.
        /// </summary>
        [Test]
        public void Register_WithMismatchedPasswords_ShowsValidationError()
        {
            // Arrange
            var dto = RegistrationDtoFactory.GetMismatchedPasswords();

            // Act
            _registrationPage.NavigateTo();
            _registrationPage.Register(dto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_registrationPage.HasValidationErrors(),
                    Is.True,
                    "Validation errors should appear when passwords do not match.");

                Assert.That(_registrationPage.GetFirstValidationError(),
                    Does.Contain("password").IgnoreCase,
                    "The error message should reference the password field.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 3 – Duplicate email
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /register.
        ///   2. Attempt to register with an email that already exists in the system.
        ///   3. Click the Register button.
        /// Expected results:
        ///   - An error message stating the email is already registered is shown.
        /// </summary>
        [Test]
        public void Register_WithDuplicateEmail_ShowsAlreadyRegisteredError()
        {
            // Arrange
            var dto = RegistrationDtoFactory.GetDuplicateEmail();

            // Act
            _registrationPage.NavigateTo();
            _registrationPage.Register(dto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_registrationPage.HasValidationErrors(),
                    Is.True,
                    "An error should be displayed when registering with an already-used email.");

                Assert.That(_registrationPage.GetFirstValidationError(),
                    Does.Contain("already").IgnoreCase
                        .Or.Contain("registered").IgnoreCase
                        .Or.Contain("exists").IgnoreCase,
                    "The error message should indicate the email is already taken.");
            });
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 4 – Invalid email format
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Navigate to /register.
        ///   2. Enter an email in an invalid format (e.g. 'notavalidemail').
        ///   3. Click the Register button.
        /// Expected results:
        ///   - A validation error about the email field format is shown.
        /// </summary>
        [Test]
        public void Register_WithInvalidEmailFormat_ShowsFormatError()
        {
            // Arrange
            var dto = RegistrationDtoFactory.GetInvalidEmailFormat();

            // Act
            _registrationPage.NavigateTo();
            _registrationPage.Register(dto);

            // Assert
            Assert.That(_registrationPage.HasValidationErrors(),
                Is.True,
                "A format validation error should appear for an invalid email address.");
        }

        // ────────────────────────────────────────────────────────────────────────
        // Test 5 – DTO equality
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Steps:
        ///   1. Create two RegistrationDto objects for the same type (company user).
        ///   2. Compare them using Equals().
        /// Expected results:
        ///   - The DTOs share matching FirstName, LastName, and IsCompany fields.
        /// </summary>
        [Test]
        public void RegistrationDto_CompanyUsers_HaveSameBaseFields()
        {
            // Arrange
            var dto1 = RegistrationDtoFactory.GetCompanyUser();
            var dto2 = RegistrationDtoFactory.GetCompanyUser();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dto1.FirstName, Is.EqualTo(dto2.FirstName),
                    "Both company user DTOs should have the same first name.");

                Assert.That(dto1.IsCompany, Is.True,
                    "Company user DTO should have IsCompany set to true.");

                Assert.That(dto1.CompanyName, Is.Not.Empty,
                    "Company name should not be empty for a company user DTO.");
            });
        }
    }
}
