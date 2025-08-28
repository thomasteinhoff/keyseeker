using Microsoft.AspNetCore.Mvc;
using keyseeker.Models;

public class HomeController : Controller
{
    private readonly ScraperManager _manager;

    public HomeController(ScraperManager manager)
    {
        _manager = manager;
    }

    public async Task<IActionResult> Index(string? query)
    {
        List<Game> results = new();

        if (!string.IsNullOrWhiteSpace(query))
        {
            results = await _manager.SearchAllAsync(query);
        }

        return View(results);
    }
}
