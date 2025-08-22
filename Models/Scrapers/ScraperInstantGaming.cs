using AngleSharp.Html.Parser;

namespace keyseeker.Models
{
    public class InstantGamingScraper : ISiteScraper
    {
        private const string BaseUrl = "https://www.instant-gaming.com/en/search/?q=";

        public async Task<List<Game>> ScrapeAsync(string query)
        {
            var results = new List<Game>();

            string address = BaseUrl + query.Replace(" ", "+");
            if (string.IsNullOrWhiteSpace(address))
                return results;

            var encoded = Uri.EscapeDataString(address);
            var url = $"{BaseUrl}{encoded}";

            using var http = new HttpClient();
            var html = await http.GetStringAsync(url);

            var parser = new HtmlParser();
            var doc = await parser.ParseDocumentAsync(html);

            foreach (var item in doc.QuerySelectorAll(".item"))
            {
                var title = item.QuerySelector(".title")?.TextContent.Trim();
                var price = item.QuerySelector(".price")?.TextContent.Trim();
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
