using Microsoft.AspNetCore.Mvc;

namespace MultiShop.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{
    // GET: DashboardController
    public ActionResult Index()
    {
        return View();
    }

}
