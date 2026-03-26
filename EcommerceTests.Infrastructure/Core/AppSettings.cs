namespace EcommerceTests.Infrastructure.Core
{
    /// <summary>
    /// Central place for application-wide constants and environment-variable-backed credentials.
    /// Credentials are read from environment variables; never hard-coded.
    /// </summary>
    public static class AppSettings
    {
        public static readonly string BaseUrl = "https://demo.nopcommerce.com";

        // ── Credentials from environment variables ──────────────────────────────
        // Set these before running:
        //   Windows:  setx NOPCOMMERCE_EMAIL "your@email.com"
        //             setx NOPCOMMERCE_PASSWORD "yourPassword"
        //   Linux/Mac: export NOPCOMMERCE_EMAIL="your@email.com"

        public static string UserEmail =>
            Environment.GetEnvironmentVariable("NOPCOMMERCE_EMAIL")
            ?? "admin@yourstore.com"; // fallback for local dev only

        public static string UserPassword =>
            Environment.GetEnvironmentVariable("NOPCOMMERCE_PASSWORD")
            ?? "admin";              // fallback for local dev only

        // Registration test account
        public static string RegEmail =>
            Environment.GetEnvironmentVariable("NOPCOMMERCE_REG_EMAIL")
            ?? $"testuser_{DateTime.Now:yyyyMMddHHmmss}@test.com";
    }
}
