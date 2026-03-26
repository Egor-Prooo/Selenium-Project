namespace EcommerceTests.Infrastructure.DTOs
{
    /// <summary>
    /// Data Transfer Object representing login credentials.
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not LoginDto other) return false;
            return Email == other.Email && Password == other.Password;
        }

        public override int GetHashCode() => HashCode.Combine(Email, Password);

        public override string ToString() => $"Email: {Email}";
    }
}
