namespace EcommerceTests.Infrastructure.DTOs
{
    /// <summary>
    /// Data Transfer Object representing a product shown in listings or the cart.
    /// </summary>
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;

        /// <summary>Convenience: Price × Quantity</summary>
        public decimal TotalPrice => Price * Quantity;

        public override bool Equals(object? obj)
        {
            if (obj is not ProductDto other) return false;
            return Name == other.Name
                && Price == other.Price
                && Quantity == other.Quantity;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Price, Quantity);

        public override string ToString() =>
            $"Product: {Name}, Price: {Price:C}, Qty: {Quantity}";
    }
}
