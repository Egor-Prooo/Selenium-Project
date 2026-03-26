namespace EcommerceTests.Infrastructure.DTOs
{
    /// <summary>
    /// Data Transfer Object representing a new-user registration form.
    /// </summary>
    public class RegistrationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsCompany { get; set; } = false;
        public string? CompanyName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not RegistrationDto other) return false;
            return FirstName == other.FirstName
                && LastName == other.LastName
                && Email == other.Email
                && IsCompany == other.IsCompany;
        }

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName, Email);

        public override string ToString() =>
            $"Name: {FirstName} {LastName}, Email: {Email}";
    }
}
