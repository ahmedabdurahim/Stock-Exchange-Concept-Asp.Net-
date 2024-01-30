using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_project.Models;

namespace ASP.NET_project.Controllers;

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

    public IActionResult User()
    {
        return View();
    }

    [HttpPost]
    public IActionResult User(User user)
    {
        Console.WriteLine("Hello");
        using(var db = new ProjectContext())
        {
            db.Add(user);
            db.SaveChanges();
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
