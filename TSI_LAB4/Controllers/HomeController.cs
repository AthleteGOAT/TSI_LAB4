using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TSI_LAB4.Models;

namespace TSI_LAB4.Controllers;

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

    public IActionResult RSA()
    {
        return View();
    }

    public IActionResult DES()
    {
        return View();
    } 
    public IActionResult SignatureDigital()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}

