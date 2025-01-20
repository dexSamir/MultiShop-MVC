using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.BL.Extension;
using MultiShop.BL.VM.Category;
using MultiShop.Core.Entities;
using MultiShop.DAL.Context;

namespace MultiShop.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{

    public ActionResult Index()
    {
        return View();
    }
}
