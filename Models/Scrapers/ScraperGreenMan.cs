using AngleSharp.Html.Parser;

namespace keyseeker.Models
{
    public class GreenManScraper : ISiteScraper
    {
        private const string BaseUrl = "https://www.greenmangaming.com/pt/search/?query=";
        private static readonly HttpClient _httpClient = new HttpClient();

        static GreenManScraper()
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/120.0 Safari/537.36"
            );
        }

        public async Task<List<Game>> ScrapeAsync(string query)
        {
            var results = new List<Game>();

            if (string.IsNullOrWhiteSpace(query))
                return results;

            var encodedQuery = query.Trim().Replace(" ", "+");
            var url = $"{BaseUrl}{encodedQuery}";

            string html;
            try
            {
                html = await _httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException)
            {
                return results;
            }

            var parser = new HtmlParser();
            var doc = parser.ParseDocument(html);

            foreach (var item in doc.QuerySelectorAll(".ais-Hits-item"))
            {
                var title = item.QuerySelector(".prod-name")?.TextContent?.Trim();
                var price = item.QuerySelector(".current-price")?.TextContent?.Trim();
                var gameUrl = item.QuerySelector("a")?.GetAttribute("href");

                if (!string.IsNullOrEmpty(title))
                {
                    results.Add(new Game
                    {
                        Store = "Green Man Gaming",
                        Title = title,
                        Price = price,
                        Url = gameUrl
                    });
                }
            }

            return results;
        }
    }
}
