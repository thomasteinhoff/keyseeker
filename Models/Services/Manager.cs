namespace keyseeker.Models
{
    public class ScraperManager
    {
        private readonly IEnumerable<ISiteScraper> _scrapers;

        public ScraperManager(IEnumerable<ISiteScraper> scrapers)
        {
            _scrapers = scrapers;
        }

        public async Task<List<Game>> SearchAllAsync(string query)
        {
            var tasks = _scrapers.Select(s => s.ScrapeAsync(query));
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(r => r).ToList();
        }
    }
}
