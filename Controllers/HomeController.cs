using System.Diagnostics;
using System.Net.Mail;
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
        bool f = await fBService.SaveFeedback(obj);
        Console.WriteLine(f);
        try
        {
            using (MailMessage m = new MailMessage())
            {
                m.From = new MailAddress("gargasaha1944@gmail.com");
                if (!string.IsNullOrEmpty(obj.Email))
                {
                    m.To.Add(obj.Email);
                }
                m.Subject = "Thank You for Your Valuable Feedback - Malda College";

                m.Body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>

    <div style='text-align:center; margin-bottom:20px;'>
        <img src='https://www.bcafeedback.somee.com/images/MaldaCollege.jpg'
             alt='Malda College'
             style='max-width:100%; height:auto; width:300px;' />
    </div>

    <h2>Dear {obj.Name},</h2>

    <p>Thank you for taking the time to share your valuable feedback with Malda College, Malda, West Bengal.</p>

    <p>Your insights regarding the BCA department's model exhibition are greatly appreciated. We are committed to continuously improving our academic programs and student services, and your feedback plays a crucial role in this process.</p>

    <p>At Malda College, we strive to provide quality education and foster an inclusive learning environment for all our students.</p>

    <p>We look forward to your continued association with our institution and hope to see you at future exhibitions.</p>

    <p><strong>Best Regards,</strong><br/>
    Malda College<br/>
    Malda, West Bengal, India</p>

</body>
</html>";

                m.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("gargasaha1944@gmail.com", "qwqdezfxxoeasgll");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return RedirectToAction("FeedBack");
    }
    [HttpGet]
    public IActionResult Show()
    {
        ViewData["Mode"] = 1;
        return View();
    }
    [HttpPost]
    public IActionResult ListFeedback(string year)
    {
        ViewData["Mode"] = 2;
        ViewData["Year"] = year;
        return View("Show");
    }
    [HttpGet]
    public async Task<JsonResult> GetFeedbacks(string year)
    {
        // Console.WriteLine(year);
        var feedbacks = await fBService.getFeedbackByYear(year);
        // Console.WriteLine("ran");
        return Json(feedbacks);
    }

    [HttpGet]
    public async Task<JsonResult> GetTopPerforming(string year)
    {
        var topPerforming = await fBService.GetCountGroup(year);
        return Json(topPerforming);
    }

    [HttpPost]
    public async Task<JsonResult> DeleteFeedback(int id)
    {
        bool result = await fBService.DeleteFeedbackById(id);
        return Json(new { success = result });
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
