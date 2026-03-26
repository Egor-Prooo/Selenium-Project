namespace EcommerceTests.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating <see cref="DTOs.SearchOptionsDto"/> instances.
    /// </summary>
    public static class SearchOptionsDtoFactory
    {
        /// <summary>Returns a basic keyword search with no filters.</summary>
        public static DTOs.SearchOptionsDto GetBasicSearch(string keyword) => new()
        {
            Keyword = keyword
        };

        /// <summary>Returns a search scoped to the Computers category.</summary>
        public static DTOs.SearchOptionsDto GetComputersSearch() => new()
        {
            Keyword      = "computer",
            CategoryName = "Computers"
        };

        /// <summary>Returns a search with description scanning enabled.</summary>
        public static DTOs.SearchOptionsDto GetSearchInDescriptions(string keyword) => new()
        {
            Keyword              = keyword,
            SearchInDescriptions = true
        };

        /// <summary>Returns a search filtered by a price range.</summary>
        public static DTOs.SearchOptionsDto GetSearchWithPriceRange(
            string keyword, decimal from, decimal to) => new()
        {
            Keyword    = keyword,
            PriceFrom  = from,
            PriceTo    = to
        };

        /// <summary>Returns a search that is expected to yield no results.</summary>
        public static DTOs.SearchOptionsDto GetNoResultsSearch() => new()
        {
            Keyword = "zzznoresultsexpected999"
        };

        /// <summary>Returns a search with a single character (edge case).</summary>
        public static DTOs.SearchOptionsDto GetSingleCharSearch() => new()
        {
            Keyword = "a"
        };
    }
}
