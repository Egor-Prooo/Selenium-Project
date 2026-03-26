using EcommerceTests.Infrastructure.Core;

namespace EcommerceTests.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating <see cref="DTOs.LoginDto"/> instances.
    /// All credential data comes from environment variables via <see cref="AppSettings"/>.
    /// </summary>
    public static class LoginDtoFactory
    {
        /// <summary>
        /// Returns a valid LoginDto for the registered test account.
        /// </summary>
        public static DTOs.LoginDto GetValidUser() => new()
        {
            Email    = AppSettings.UserEmail,
            Password = AppSettings.UserPassword
        };

        /// <summary>
        /// Returns a LoginDto with a valid email but a wrong password.
        /// </summary>
        public static DTOs.LoginDto GetInvalidPassword() => new()
        {
            Email    = AppSettings.UserEmail,
            Password = "WrongPassword!99"
        };

        /// <summary>
        /// Returns a LoginDto with a completely unregistered email.
        /// </summary>
        public static DTOs.LoginDto GetUnregisteredUser() => new()
        {
            Email    = "notregistered@example.com",
            Password = "SomePass123!"
        };

        /// <summary>
        /// Returns a LoginDto with an empty email field.
        /// </summary>
        public static DTOs.LoginDto GetEmptyEmail() => new()
        {
            Email    = string.Empty,
            Password = AppSettings.UserPassword
        };
    }
}
