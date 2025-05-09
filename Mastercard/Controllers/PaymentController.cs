using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GlobalPay.HostedCheckouts.Mastercard.Models;
using Microsoft.Extensions.Options;
using Client.Models;
using Client.Interfaces;
using Client.Common.Models;
using HostedCheckoutRequest = Client.Models.HostedCheckoutRequest;
using GlobalPay.Logging.Helpers;
using Amazon.Runtime.Internal;
using Amazon.CloudWatchLogs;

namespace GlobalPay.HostedCheckouts.Mastercard.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private ILogger<PaymentController> _logger;
        private readonly IHostedCheckOutService _HostedCheckOutService;

        public PaymentController(ILogger<PaymentController> logger, IHostedCheckOutService HostedCheckOutService)
        {
            _HostedCheckOutService = HostedCheckOutService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogOperation("Information", "CreateSession", "Constructor");
            _logger.LogOperation("Exception", "CreateSession", "Constructor",new Exception ());
            return View();
        }

        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] sessionRequest request)
        {
            HostedCheckoutResult resp = new HostedCheckoutResult();
            try
            {
                _logger.LogOperation("Information", "CreateSession", $"CreateSession request : {request}");
                resp = await _HostedCheckOutService.CreateSession(request);
                _logger.LogOperation("Information", "CreateSession", $"CreateSession response : {resp}");
                //return Json(new { resp });
                return Ok(resp);
            }
            catch (Exception ex)
            {

                // ILogging.LogError($"Unexpected error in InitiatePurchase: {ex.Message}", "InitiatePurchase");
                resp.Status = -1;
                resp.message = $"Error occured in microservice, {ex.Message}";
                _logger.LogOperation("Exception", "CreateSession", $"CreateSession request failed : {ex.Message}",ex);
                return StatusCode(500, resp);
            }
        }
        [HttpGet("verify/order/{orderid}")]
        public async Task<IActionResult> Verify(string orderid)
        {

            VerifyTransactionResultV2 response = new VerifyTransactionResultV2();
            try
            {
                //response = await _HostedCheckOutService.Verifyv2(orderid, transactionId);
                _logger.LogOperation("Information", "VerifyTransaction", $"VerifyTransaction request : orderId: {orderid}");
                response = await _HostedCheckOutService.VerifyV2(orderid);
                _logger.LogOperation("Information", "VerifyTransaction", $"VerifyTransaction response : {response}");
                return Json(new { response });
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                // ILogging.LogError($"Unexpected error in InitiatePurchase: {ex.Message}", "InitiatePurchase");
                response.Status = -1;
                response.message = $"Error occured in microservice, {ex.Message}";
                _logger.LogOperation("Exception", "VerifyTransaction", $"VerifyTransaction request failed : {ex.Message}", ex);
                return StatusCode(500, response);
            }
        }

        [HttpGet("verifyOld/order/{orderid}")]
        public async Task<IActionResult> VerifyOld(string orderid)
        {

            VerifyTransactionResult response = new VerifyTransactionResult();
            try
            {
                //response = await _HostedCheckOutService.Verifyv2(orderid, transactionId);
                _logger.LogOperation("Information", "VerifyTransaction", $"VerifyTransaction request : orderId: {orderid}");
                response = await _HostedCheckOutService.Verify(orderid);
                _logger.LogOperation("Information", "VerifyTransaction", $"VerifyTransaction response : {response}");
                return Json(new { response });
            }
            catch (Exception ex)
            {

                // Log unexpected errors
                // ILogging.LogError($"Unexpected error in InitiatePurchase: {ex.Message}", "InitiatePurchase");
                response.Status = -1;
                response.message = $"Error occured in microservice, {ex.Message}";
                _logger.LogOperation("Exception", "VerifyTransaction", $"VerifyTransaction request failed : {ex.Message}", ex);
                return StatusCode(500, response);
            }
        }
        //[HttpGet("callback")]
        //public async Task<IActionResult> Callback()
        //{
        //    var queryParams = HttpContext.Request.Query;

        //    // Log all query parameters (optional)
        //    foreach (var param in queryParams)
        //    {
        //        Console.WriteLine($"Key: {param.Key}, Value: {param.Value}");
        //    }

        //    // Convert to dictionary (optional)
        //    var allParams = queryParams.ToDictionary(p => p.Key, p => p.Value.ToString());

        //    var resultIndicator = queryParams["resultIndicator"].ToString();

        //    if (string.IsNullOrEmpty(resultIndicator))
        //    {
        //        _logger.LogWarning("Missing resultIndicator in callback.");
        //        return BadRequest(new { status = "error", message = "Missing resultIndicator" });
        //    }

        //    // Call your hosted checkout verification logic
        //    var verifyResponse = await _HostedCheckOutService.Verify(resultIndicator, resultIndicator);

        //    // You can also extract other params if needed
        //    var status = queryParams["status"].ToString();
        //    var transactionId = queryParams["transactionId"].ToString();

        //    if (status == "success")
        //    {
        //        // TODO: Store transaction, update DB, etc.
        //        _logger.LogInformation("Transaction successful: {TransactionId}", transactionId);
        //    }

        //    return Ok(new { status = "received", resultIndicator, transactionId });
        //}

        //[HttpPost("notify")]
        //public async Task<IActionResult> NotifyCallback()
        //{
        //    using var reader = new StreamReader(Request.Body);
        //    var body = await reader.ReadToEndAsync();

        //    // Log or save this to a DB
        //    Console.WriteLine("Mastercard Notification Callback Payload:");
        //    Console.WriteLine(body);

        //    // Optionally: Deserialize to a model (if you know the structure)
        //    return Ok(); // Respond with 200 OK so Mastercard knows it succeeded
        //}

        public IActionResult Complete()
        {
            // Payment callback response handling
            return View();
        }
    }
    
}
