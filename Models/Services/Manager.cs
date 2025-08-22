using keyseeker.Models;

namespace MyProject.Models
{
    public class Manager
    {
        private readonly List<ISiteScraper> _scrapers;

        public Manager(List<ISiteScraper> scrapers)
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
