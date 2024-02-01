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
        DeleteCookie();
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Exchange()
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }
        return View();
    }

    public IActionResult Commodities()
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }
        return View();
    }

    public IActionResult IPO()
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }
        return View();
    }

    [HttpPost]
    public IActionResult User(User user)
    {
        using(var db = new ProjectContext())
        {
            db.Add(user);
            db.SaveChanges();
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {

        using (var db = new ProjectContext())
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                // Invalid username or password, handle accordingly (e.g., show error message)
                return View("Login");
            }

            // Username and password are correct, proceed with the desired action (e.g., redirect to home page)

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("email", email, options);
            Response.Cookies.Append("password", password, options);

            return RedirectToAction("Exchange", "Home");
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public string DeleteCookie()
    {
        // Delete the cookie from the browser.
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("password");
        return "Cookies are Deleted";
    }
}
