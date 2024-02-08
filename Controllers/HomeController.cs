using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_project.Models;
using System.ComponentModel;

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

        // var randomStockTickers = new List<IPO>();

        // // Create an instance of ProjectContext
        // using (var context = new ProjectContext())
        // {
        //     // Retrieve values from the Exchanges table
        //     var exchanges = context.IPO.ToList();

        //     foreach (var exchange in exchanges)
        //     {
        //         var ticker = exchange.Ticker;
        //         var value = exchange.Value;
        //         var capital = exchange.Capital;
        //         var circulating = exchange.Circulating;

        //         var exchangeModel = new IPO
        //         {
        //             Ticker = ticker,
        //             Value = value,
        //             Capital = capital,
        //             Circulating = circulating
        //         };

        //         randomStockTickers.Add(exchangeModel);
        //     }
        // }

        List<Holdings> stockData = GetData();

        // Pass the randomStockTickers list to the view
        return View(stockData);

    }

    [HttpPost]
    public IActionResult Exchange(IFormCollection form)
    {
        Console.WriteLine(form);

        if (form["action"] == "buy")
        {
            // Logic for buy action
            Console.WriteLine("Buy button CLicked");

            return RedirectToAction("BuySuccess");
        }
        else if (form["action"] == "sell")
        {
            // Logic for sell action
            Console.WriteLine("sell Button Clicked");

            return RedirectToAction("SellSuccess");
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

    public IActionResult buyAssets()
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }
        return View();
    }

    // [HttpPost]
    // public IActionResult buyAssets(string ticker, string quantity, double price, string date)
    // {

    //     // Buy Logic
    //     Console.WriteLine(ticker + " " + quantity + " " + price + " " + date);
    //     return View();
    // }

    public IActionResult sellAssets()
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
    public IActionResult sellAssets(string ticker, string quantity, double price, string date)
    {
        string email = Request.Cookies["email"]; // Replace with the actual email value
        int sellQuantity = int.Parse(quantity); // Parse the quantity to an integer

        using (var db = new ProjectContext())
        {
            // Find the rows in the "Holdings" table matching the email and ticker
            var holdings = db.Holdings.Where(h => h.Email == email && h.Ticker == ticker).ToList();

            if (holdings.Count == 0)
            {
                return RedirectToAction("NotFound", "Home"); // No rows found, return false
            }

            double remainingQuantity = sellQuantity;

            foreach (var holding in holdings)
            {
                if (remainingQuantity < holding.Quantity)
                {
                    // Deduct the sellQuantity from the current holding
                    holding.Quantity -= remainingQuantity;
                    db.Entry(holding).Property(x => x.Quantity).IsModified = true;
                    db.SaveChanges();

                    // Update the Circulating value in the IPO table
                    var ipoRow = db.IPO.FirstOrDefault(i => i.Ticker == ticker);
                    if (ipoRow != null)
                    {
                        ipoRow.Circulating += Convert.ToInt32(remainingQuantity);
                        db.Entry(ipoRow).Property(x => x.Circulating).IsModified = true;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Exchange", "Home");; // Save changes and return true
                }
                else if (remainingQuantity == holding.Quantity)
                {
                    // Delete the row from the database
                    db.Holdings.Remove(holding);
                    db.SaveChanges();

                    // Update the Circulating value in the IPO table
                    var ipoRow = db.IPO.FirstOrDefault(i => i.Ticker == ticker);
                    if (ipoRow != null)
                    {
                        ipoRow.Circulating += Convert.ToInt32(remainingQuantity);
                        db.Entry(ipoRow).Property(x => x.Circulating).IsModified = true;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Exchange", "Home"); // Save changes and return true
                }
                else // remainingQuantity > holding.Quantity
                {
                    // Subtract the holding.Quantity from the remainingQuantity
                    remainingQuantity -= holding.Quantity;
                    db.Holdings.Remove(holding);
                }
            }

            return RedirectToAction("False", "Home"); // Sell quantity exceeds the total holding quantity, return false
        }
    }

    public IActionResult IPO()
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }


        // using (var db = new ProjectContext())
        // {
        //     // Create sample data
        //     var exchangeData = new List<Exchange>
        //     {
        //         new Exchange { AssetName = "Apple", Ticker = "AAPL", Shares = 100000, Circulating = 100000, Capital = 3000000 },
        //         new Exchange { AssetName = "Nvidia", Ticker = "NVDI", Shares = 5000000, Circulating = 2000000, Capital = 100000000 },
        //         // Add more sample data if needed
        //     };

        //     // Add sample data to the context
        //     db.Exchange.AddRange(exchangeData);

        //     // Save changes to the database
        //     db.SaveChanges();
        // }


        return View();
    }

    [HttpPost]
    public IActionResult IPO(IPO ipo)
    {
       Console.WriteLine("eneterd");

        using(var db = new ProjectContext())
        {
            db.Add(ipo);
            db.SaveChanges();
        }

        return View();
    }

    public IActionResult CardProcessor(string ticker, int quantity, decimal price, DateTime date)
    {
        string storedEmail = Request.Cookies["email"];
        string storedPassword = Request.Cookies["password"];

        if (string.IsNullOrEmpty(storedEmail) && string.IsNullOrEmpty(storedPassword))
        {
            return RedirectToAction("Login", "Home");
        }

        var transactionData = new TransactionData
        {
            Ticker = ticker,
            Quantity = quantity,
            Price = price,
            Date = date
        };

        return View(transactionData);

    }


    [HttpPost]
    public IActionResult CardProcessor(string ticker, string date, double price, int quantity)
    {
        var holding = new Holdings
        {
            Email = Request.Cookies["email"],
            AssetName = ticker,
            Ticker = ticker,
            OpenPrice = price,
            Date = date,
            Quantity = quantity
        };

        // Save the holding to the "Holdings" table using the ProjectContext
        using (var db = new ProjectContext())
        {
            // Find the corresponding row in the "IPO" table based on the ticker
            var ipoRow = db.IPO.FirstOrDefault(i => i.Ticker == ticker);

            if (ipoRow != null)
            {
                // Deduct the circulating value
                ipoRow.Circulating -= quantity;

                // Check if the circulating value becomes less than zero
                if (ipoRow.Circulating < 0)
                {
                    return RedirectToAction("False", "Home");
                }

                // Save the changes to the database
                db.Entry(ipoRow).Property(x => x.Circulating).IsModified = true;

                // Save the changes to the database
                db.Holdings.Add(holding);
                db.SaveChanges();

                return RedirectToAction("Exchange", "Home");
            }
            else
            {
                return RedirectToAction("DoesNotExist", "Home");
            }
        }

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


    public List<Holdings> GetData()
    {
        var randomStockTickers = new List<Holdings>();

        // Create an instance of ProjectContext
        using (var context = new ProjectContext())
        {
            // Retrieve values from the IPO table
            var exchanges = context.Holdings.ToList();

            foreach (var exchange in exchanges)
            {
                var ticker = exchange.Ticker;
                var OpenPrice = exchange.OpenPrice;
                var Quantity = exchange.Quantity;
                var Date = exchange.Date;

                var exchangeModel = new Holdings
                {
                    Ticker = ticker,
                    OpenPrice = OpenPrice,
                    Quantity = Quantity,
                    Date = Date
                };

                randomStockTickers.Add(exchangeModel);
            }
        }

        return randomStockTickers;
    }
}
