namespace EcommerceTests.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating <see cref="DTOs.RegistrationDto"/> instances.
    /// Uses a timestamp suffix to guarantee unique emails on repeated test runs.
    /// </summary>
    public static class RegistrationDtoFactory
    {
        private static string UniqueEmail =>
            $"testuser_{DateTime.Now:yyyyMMddHHmmss}@automationtest.com";

        /// <summary>
        /// Returns a fully valid registration DTO — happy path.
        /// </summary>
        public static DTOs.RegistrationDto GetValidUser() => new()
        {
            FirstName       = "Test",
            LastName        = "Automation",
            Email           = UniqueEmail,
            Password        = "Test@1234!",
            ConfirmPassword = "Test@1234!",
            IsCompany       = false
        };

        /// <summary>
        /// Returns a registration DTO with mismatched passwords.
        /// </summary>
        public static DTOs.RegistrationDto GetMismatchedPasswords() => new()
        {
            FirstName       = "John",
            LastName        = "Doe",
            Email           = UniqueEmail,
            Password        = "Test@1234!",
            ConfirmPassword = "DifferentPass!1",
            IsCompany       = false
        };

        /// <summary>
        /// Returns a registration DTO with an already-registered email.
        /// </summary>
        public static DTOs.RegistrationDto GetDuplicateEmail() => new()
        {
            FirstName       = "Duplicate",
            LastName        = "User",
            Email           = Core.AppSettings.UserEmail, // known-existing account
            Password        = "Test@1234!",
            ConfirmPassword = "Test@1234!",
            IsCompany       = false
        };

        /// <summary>
        /// Returns a registration DTO with an invalid email format.
        /// </summary>
        public static DTOs.RegistrationDto GetInvalidEmailFormat() => new()
        {
            FirstName       = "Bad",
            LastName        = "Email",
            Email           = "notavalidemail",
            Password        = "Test@1234!",
            ConfirmPassword = "Test@1234!",
            IsCompany       = false
        };

        /// <summary>
        /// Returns a registration DTO for a company account.
        /// </summary>
        public static DTOs.RegistrationDto GetCompanyUser() => new()
        {
            FirstName       = "Jane",
            LastName        = "Corp",
            Email           = UniqueEmail,
            Password        = "Test@1234!",
            ConfirmPassword = "Test@1234!",
            IsCompany       = true,
            CompanyName     = "Automation Corp Ltd."
        };
    }
}
