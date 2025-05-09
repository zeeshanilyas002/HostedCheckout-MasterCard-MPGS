using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Client.Models;
using Client.Interfaces;
using Client.Common.Models;
using System.Text.Json;
using System.Net;
using Newtonsoft.Json;
using System.Numerics;
using Microsoft.Extensions.Logging;
using GlobalPay.Logging.Helpers;
//using Newtonsoft.Json;

namespace Client.Repositories
{
    public class HostedCheckOutService : IHostedCheckOutService
    {
        private readonly IHttpClientFactory _clientFactory;
        private ILogger<HostedCheckOutService> _logger;
        private readonly MPGSCreds _MPGSCreds;
        public HostedCheckOutService(ILogger<HostedCheckOutService> logger, IHttpClientFactory clientFactory, IOptions<MPGSCreds> mpgsCreds)
        {
            _MPGSCreds = mpgsCreds.Value;
            _clientFactory = clientFactory;
            _logger = logger;
        }
        
        public async Task<HostedCheckoutResult> CreateSession(sessionRequest request)
        {
            try
            {
                Console.WriteLine($"orderId : {request.orderId}");
                HostedCheckoutRequest hostedCheckoutRequest = new HostedCheckoutRequest
                {
                    ApiOperation = string.IsNullOrEmpty(request.apiOperation)? "INITIATE_CHECKOUT" : request.apiOperation,//CREATE_CHECKOUT_SESSION
                    CheckoutMode = "WEBSITE",
                    Interaction = new Interaction
                    {
                        Operation = "PURCHASE",//PURCHASE
                        ReturnUrl = request.returnUrl,
                        Merchant = new Merchant
                        {
                            Name = request.merchant.name,
                            Url = "https://localhost:7167/api/payment/notify"
                        }
                    },
                    order = new Order
                    {
                        Currency = "GHS",
                        Amount = request.amount,
                        id = request.orderId,
                        Description = "Test Description"
                    }
                };
                var client = _clientFactory.CreateClient();
                string merchantid = _MPGSCreds.merchantId;

                string userName = $"merchant.{_MPGSCreds.merchantId}";
                string password = $"{_MPGSCreds.APIKey}";

                // Version is hardcoded for now only on sandbox we will get it from aws secret manager on live
                _MPGSCreds.apiVersion = int.Parse(_MPGSCreds.apiVersion) < 70 ? "72" : _MPGSCreds.apiVersion;
                //_MPGSCreds.apiBaseUrl = "";
                var url = $"{_MPGSCreds.apiBaseUrl}/api/rest/version/{_MPGSCreds.apiVersion}/merchant/{merchantid}/session";

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                
                var serializeContent = System.Text.Json.JsonSerializer.Serialize(hostedCheckoutRequest);
                _logger.LogOperation("Information", "CreateSession", $"CreateSession request : {hostedCheckoutRequest}, version : {_MPGSCreds.apiVersion}");

                var content = new StringContent(serializeContent, Encoding.UTF8, "application/json");

                var resp = await client.PostAsync(url, content);
                var result = await resp.Content.ReadAsStringAsync();
                //_logger.LogOperation("Information", "CreateSession", $"CreateSession response : {result}");
                if (resp.IsSuccessStatusCode)
                {
                    HostedCheckoutResponse hostedCheckoutResponse = System.Text.Json.JsonSerializer.Deserialize<HostedCheckoutResponse>(result);
                    return new HostedCheckoutResult
                    {
                        Status = 0,
                        StatusCode = resp.StatusCode,
                        CheckoutSuccessResponse = hostedCheckoutResponse,
                    };
                    
                }
                else
                {
                    HostedCheckoutErrorResponse hostedCheckoutErrorResponse = System.Text.Json.JsonSerializer.Deserialize<HostedCheckoutErrorResponse>(result);
                    return new HostedCheckoutResult
                    {
                        Status = 0,
                        StatusCode = resp.StatusCode,
                        ErrorResponse = hostedCheckoutErrorResponse,
                    };
                    
                }

            }
            catch (Exception ex)
            {
                _logger.LogOperation("Exception", "CreateSession", $"CreateSession request failed : {ex.Message}", ex);
                return new HostedCheckoutResult
                {
                    Status = -1,
                    message = $"Error occurred : {ex.Message}",
                };
            }
        }

        public async Task<HostedCheckoutResponse> CreateSessionv1(sessionRequest request)
        {
            try
            {
                HostedCheckoutRequest hostedCheckoutRequest = new HostedCheckoutRequest
                {
                    ApiOperation = "INITIATE_CHECKOUT",//CREATE_CHECKOUT_SESSION
                    CheckoutMode = "WEBSITE",
                    Interaction = new Interaction
                    {
                        Operation = "PURCHASE",//PURCHASE
                        ReturnUrl = request.returnUrl,
                        Merchant = new Merchant
                        {
                            Name = request.merchant.name,
                            Url = "https://localhost:7167/api/payment/notify"
                        },
                    },
                    order = new Order
                    {
                        Currency = "GHS",
                        Amount = request.amount,
                        id = request.orderId,
                        Description = "Test Description"
                    }
                };

                var client = _clientFactory.CreateClient();
                string merchantid = _MPGSCreds.merchantId;

                string userName = $"merchant.{_MPGSCreds.merchantId}";
                string password = $"{_MPGSCreds.APIKey}";

                var url = $"{_MPGSCreds.apiBaseUrl}/api/rest/version/{_MPGSCreds.apiVersion}/merchant/{merchantid}/session";

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                 var serializeContent = System.Text.Json.JsonSerializer.Serialize(hostedCheckoutRequest);
                _logger.LogOperation("Information", "CreateSession", $"CreateSession request : {hostedCheckoutRequest}, version : {_MPGSCreds.apiVersion}");

                var content = new StringContent(serializeContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                var hostedCheckoutResponse = System.Text.Json.JsonSerializer.Deserialize<HostedCheckoutResponse>(result);

                //var json = JsonDocument.Parse(result);

                //var sessionId = json.RootElement.GetProperty("session").GetProperty("id").GetString();
                return hostedCheckoutResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<VerifyTransactionResult> Verify(string orderid)
        {
            try
            {

                var client = _clientFactory.CreateClient();
                string merchantid = _MPGSCreds.merchantId;

                string userName = $"merchant.{_MPGSCreds.merchantId}";
                string password = $"{_MPGSCreds.APIKey}";

                var url = $"{_MPGSCreds.apiBaseUrl}/api/rest/version/{_MPGSCreds.apiVersion}/merchant/{merchantid}/order/{orderid}";
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode ==HttpStatusCode.OK)
                {
                    // var transactionResponse = JsonSerializer.Deserialize<RetrieveTransactionResponse>(result);
                    var transactionResponse = System.Text.Json.JsonSerializer.Deserialize<RetrieveTransactionResponse>(result);
                    return new VerifyTransactionResult
                    {
                        Status = 1,
                        StatusCode = response.StatusCode,
                        TransactionSuccessResponse = transactionResponse,
                        };

                    //return new VerifyTransactionResult
                    //{
                    //    Status = 1,
                    //    StatusCode = resp.StatusCode,
                    //    Data = Newtonsoft.Json.JsonConvert.SerializeObject(transactionResponse)
                    //};
                }
                else
                {
                    // var error = JsonSerializer.Deserialize<HostedCheckoutErrorResponse>(result);
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<HostedCheckoutErrorResponse>(result);
                    return new VerifyTransactionResult
                    {
                        Status = 0,
                        StatusCode = response.StatusCode,
                        ErrorResponse = errorResponse,
                    };
                    //return new ClientResponse
                    //{
                    //    Status = 0,
                    //    StatusCode = resp.StatusCode,
                    //    Data = Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse)
                    //};
                }

            }
            catch (Exception ex)
            {

                return new VerifyTransactionResult
                {
                    Status = -1,
                    message = $"Error occurred : {ex.Message}",
                };
            }
        }
        public async Task<VerifyTransactionResultV2> VerifyV2(string orderid)
        {
            try
            {

                var client = _clientFactory.CreateClient();
                string merchantid = _MPGSCreds.merchantId;

                string userName = $"merchant.{_MPGSCreds.merchantId}";
                string password = $"{_MPGSCreds.APIKey}";
               // _MPGSCreds.apiVersion = int.Parse(_MPGSCreds.apiVersion) < 70 ? "72" : _MPGSCreds.apiVersion;

                var url = $"{_MPGSCreds.apiBaseUrl}/api/rest/version/{_MPGSCreds.apiVersion}/merchant/{merchantid}/order/{orderid}";
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // var transactionResponse = JsonSerializer.Deserialize<RetrieveTransactionResponse>(result);
                    var transactionResponse = System.Text.Json.JsonSerializer.Deserialize<verifyTransactionResp>(result);
                    return new VerifyTransactionResultV2
                    {
                        Status = 1,
                        StatusCode = response.StatusCode,
                        verifyTransactionResp = transactionResponse,                        
                    };
                }
                else
                {
                    // var error = JsonSerializer.Deserialize<HostedCheckoutErrorResponse>(result);
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<verifyTransactionResp>(result);
                    return new VerifyTransactionResultV2
                    {
                        Status = 0,
                        StatusCode = response.StatusCode,
                        verifyTransactionResp = errorResponse,
                    };
                    //return new ClientResponse
                    //{
                    //    Status = 0,
                    //    StatusCode = resp.StatusCode,
                    //    Data = Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse)
                    //};
                }

            }
            catch (Exception ex)
            {
                return new VerifyTransactionResultV2
                {
                    Status = -1,
                    message = $"Error occurred : {ex.Message}",
                };
            }
        }
    }
}
