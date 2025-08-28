using keyseeker.Models;
using Microsoft.AspNetCore.Mvc;

[Route("Scraper")]
public class ScraperController : Controller
{
    private readonly ScraperManager _scraperManager;

    public ScraperController(ScraperManager scraperManager)
    {
        _scraperManager = scraperManager;
    }

    [HttpGet("Search")]
    public async Task<ActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var results = await _scraperManager.SearchAllAsync(query);
        return View(results);
    }
}
