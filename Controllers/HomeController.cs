using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FeedBack.Models;
using FeedBack.Services;

namespace FeedBack.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private FBService fBService;
    public HomeController(ILogger<HomeController> logger)
    {
        fBService = new FBService(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build());
        _logger = logger;
    }
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
    public async Task<IActionResult> FeedBack(Models.FB obj)
    {
        bool f=await fBService.SaveFeedback(obj);
        Console.WriteLine(f);
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
