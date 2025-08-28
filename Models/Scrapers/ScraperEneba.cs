using AngleSharp.Html.Parser;

namespace keyseeker.Models
{
    public class EnebaScraper : ISiteScraper
    {
        private const string BaseUrl = "https://www.eneba.com/store/all?drms[]=steam&page=1&text=";
        private static readonly HttpClient _httpClient = new HttpClient();

        static EnebaScraper()
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

            foreach (var item in doc.QuerySelectorAll(".pFaGHa.WpvaUk.t8TnVR.ssdbYG"))
            {
                var title = item.QuerySelector(".YLosEL")?.TextContent?.Trim();
                var price = item.QuerySelector(".L5ErLT")?.TextContent?.Trim();
                var gameUrl = item.QuerySelector("a")?.GetAttribute("href");

                if (!string.IsNullOrEmpty(title))
                {
                    results.Add(new Game
                    {
                        Store = "Eneba",
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
