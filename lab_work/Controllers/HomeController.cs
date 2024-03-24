using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lab_work.Models;

namespace lab_work.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        if (searchType == "Projects")
        {
            return RedirectToAction("Search", "Project", new { area = "ProjectManagement", searchString });
        }
        else if (searchType == "Tasks")
        {
            //searching tasks
            int defaultProjectId = 2;
            return RedirectToAction("Search", "Task", new { area = "ProjectManagement", projectId = defaultProjectId, searchString });
        }
        return View("Index", "Home");
    }

    public IActionResult NotFound(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }
        return View("Error");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
