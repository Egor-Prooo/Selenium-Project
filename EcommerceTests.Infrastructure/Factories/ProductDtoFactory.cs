namespace EcommerceTests.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating <see cref="DTOs.ProductDto"/> instances that represent
    /// products available on the nopCommerce demo store.
    /// </summary>
    public static class ProductDtoFactory
    {
        /// <summary>
        /// A well-known inexpensive product used for cart/purchase flow tests.
        /// </summary>
        public static DTOs.ProductDto GetSimpleProduct() => new()
        {
            Name     = "Simple Computer",
            Price    = 800.00m,
            Quantity = 1
        };

        /// <summary>
        /// A second product used for multi-item cart tests.
        /// </summary>
        public static DTOs.ProductDto GetSecondProduct() => new()
        {
            Name     = "Fahrenheit 451 by Ray Bradbury",
            Price    = 27.00m,
            Quantity = 1
        };

        /// <summary>
        /// Returns a product DTO with quantity set to 2.
        /// </summary>
        public static DTOs.ProductDto GetProductWithQuantityTwo() => new()
        {
            Name     = "Simple Computer",
            Price    = 800.00m,
            Quantity = 2
        };
    }
}
