using Client.Interfaces;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPay.HostedCheckouts.Mastercard.Controllers
{
    public class CheckoutTest : Controller
    {
        private readonly IHostedCheckOutService _HostedCheckOutService;

        public CheckoutTest(IHostedCheckOutService HostedCheckOutService)
        {
            _HostedCheckOutService = HostedCheckOutService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] sessionRequest request)
        {
            HostedCheckoutResponse resp = await _HostedCheckOutService.CreateSessionv1(request);
            return Json(new { resp.session });
        }
        public IActionResult Complete()
        {
            // Payment callback response handling
            return View();
        }
    }
}
