using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Common.Models
{
    //public class ClientResponse
    //{
    //    public HttpStatusCode StatusCode { get; set; }

    //    public string? Data { get; set; }

    //    public int Status { get; set; }

    //    [JsonIgnore]
    //    public string CardResponse { get; set; }
    //    public string? message { get; set; }
    //}
    //public class ClientResponsev2<T>
    //{
    //    public HttpStatusCode StatusCode { get; set; }

    //    public T Data { get; set; }

    //    public int Status { get; set; }

    //    [JsonIgnore]
    //    public string CardResponse { get; set; }
    //    public string? message { get; set; }
    //}

    public class HostedCheckoutResult 
    {
        public HttpStatusCode StatusCode { get; set; }
        public int Status { get; set; }
        public string? message { get; set; }
        public HostedCheckoutResponse CheckoutSuccessResponse { get; set; }
        public HostedCheckoutErrorResponse ErrorResponse { get; set; }
        public bool IsSuccess => ErrorResponse == null;
    }

}
