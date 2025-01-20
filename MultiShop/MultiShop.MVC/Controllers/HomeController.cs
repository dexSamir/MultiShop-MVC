using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MultiShop.MVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

