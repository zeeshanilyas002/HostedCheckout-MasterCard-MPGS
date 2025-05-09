using GlobalPay.HostedCheckouts.Mastercard.Models;
using GlobalPay.Logging.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GlobalPay.HostedCheckouts.Mastercard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation("Exception", "CreateSession", "Constructor", new Exception());

            _logger.LogOperation("Information", "HomeController", "Constructor");
        }

        public IActionResult Index()
        {           
            return View();
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
}
