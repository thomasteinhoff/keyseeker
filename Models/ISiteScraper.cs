namespace keyseeker.Models
{
    public interface ISiteScraper
    {
        Task<List<Game>> ScrapeAsync(string query);
    }
}