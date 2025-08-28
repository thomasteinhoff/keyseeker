using AngleSharp.Html.Parser;

namespace keyseeker.Models
{
    public class InstantGamingScraper : ISiteScraper
    {
        private const string BaseUrl = "https://www.instant-gaming.com/en/search/?platform%5B%5D=&type%5B%5D=steam&query=";
        private static readonly HttpClient _httpClient = new HttpClient();

        static InstantGamingScraper()
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

            foreach (var item in doc.QuerySelectorAll(".item"))
            {
                var title = item.QuerySelector(".title")?.TextContent?.Trim();
                var price = item.QuerySelector(".price")?.TextContent?.Trim();
                var gameUrl = item.QuerySelector("a")?.GetAttribute("href");

                if (!string.IsNullOrEmpty(title))
                {
                    results.Add(new Game
                    {
                        Store = "Instant Gaming",
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
