using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FeedBack.Models;

namespace FeedBack.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult FeedBack()
    {
        return View();
    }
    [HttpPost]
    public IActionResult FeedBack(Models.FB obj)
    {
        Console.WriteLine(obj.Name+" "+obj.Email+" "+obj.Fb+" "+obj.Emojivalue);
        
        return RedirectToAction("FeedBack");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
