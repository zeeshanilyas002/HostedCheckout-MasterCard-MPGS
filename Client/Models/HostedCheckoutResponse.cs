using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class HostedCheckoutResponse
    {
        public string merchant { get; set; }
        public string result { get; set; }
        public Session session { get; set; }
        public error error { get; set; }
        public string successIndicator { get; set; }
    }
    public class error
    {
        public string explanation { get; set; }
        public string cause { get; set; }
    }
    public class Session
    {
        public string id { get; set; }
        public string updateStatus { get; set; }
        public string version { get; set; }
    }






    //verify

    public class VerifyTransactionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public int Status { get; set; }
        public string? message { get; set; }
        public RetrieveTransactionResponse TransactionSuccessResponse { get; set; }
        public HostedCheckoutErrorResponse ErrorResponse { get; set; }
        public bool IsSuccess => ErrorResponse == null;
    }
    public class VerifyTransactionResultV2
    {
        public HttpStatusCode StatusCode { get; set; }
        public int Status { get; set; }
        public string? message { get; set; }
        public verifyTransactionResp verifyTransactionResp { get; set; } = new verifyTransactionResp();
    }
    public class HostedCheckoutErrorResponse
    {
        public ErrorDetail error { get; set; }
        public string result { get; set; }
    }

    public class ErrorDetail
    {
        public string cause { get; set; }
        public string explanation { get; set; }
    }

    public class RetrieveTransactionResponse
    {
        public string Merchant { get; set; }
        public OrderDetails Order { get; set; }
        public ResponseDetails Response { get; set; }
        public TransactionDetails Transaction { get; set; }
    }

    public class OrderDetails
    {
        public decimal Amount { get; set; }
        public DateTime CreationTime { get; set; }
        public string Currency { get; set; }
        public string Id { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public decimal MerchantAmount { get; set; }
        public string MerchantCurrency { get; set; }
        public decimal TotalAuthorizedAmount { get; set; }
        public decimal TotalCapturedAmount { get; set; }
        public decimal TotalDisbursedAmount { get; set; }
        public decimal TotalRefundedAmount { get; set; }
    }

    public class ResponseDetails
    {
        public string GatewayCode { get; set; }
        public string Result { get; set; }
    }

    public class TransactionDetails
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public AcquirerDetails Acquirer { get; set; }
    }

    public class AcquirerDetails
    {
        // You can expand this with actual fields when known
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
