namespace EcommerceTests.Infrastructure.DTOs
{
    /// <summary>
    /// Data Transfer Object representing the options available on the advanced search page.
    /// </summary>
    public class SearchOptionsDto
    {
        public string Keyword { get; set; } = string.Empty;
        public bool SearchInDescriptions { get; set; } = false;
        public string? CategoryName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SearchOptionsDto other) return false;
            return Keyword == other.Keyword
                && SearchInDescriptions == other.SearchInDescriptions
                && CategoryName == other.CategoryName
                && PriceFrom == other.PriceFrom
                && PriceTo == other.PriceTo;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Keyword, SearchInDescriptions, CategoryName);

        public override string ToString() =>
            $"Keyword: '{Keyword}', InDescriptions: {SearchInDescriptions}, Category: {CategoryName ?? "All"}";
    }
}
