using Microsoft.AspNetCore.Mvc;
using keyseeker.Models;

namespace keyseeker.Controllers
{
    public class ScraperController : Controller
    {
        private readonly ISiteScraper _scraper;

        public ScraperController(ISiteScraper scraper)
        {
            _scraper = scraper;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            Console.WriteLine($"[DEBUG] Query received: {query}");

            if (string.IsNullOrWhiteSpace(query))
                return View(new List<Game>());

            var results = await _scraper.ScrapeAsync(query);

            Console.WriteLine($"[DEBUG] Scraper returned {results.Count} results");
            
            return Json(results);

        }
    }
}
