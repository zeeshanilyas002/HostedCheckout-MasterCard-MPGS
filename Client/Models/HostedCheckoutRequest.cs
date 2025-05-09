using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Models
{
    public class HostedCheckoutRequest
    {
        [JsonPropertyName("apiOperation")]
        public string ApiOperation { get; set; }
        [JsonPropertyName("checkoutMode")]
        public string CheckoutMode { get; set; }
        [JsonPropertyName("interaction")]

        public Interaction Interaction { get; set; }
        [JsonPropertyName("order")]
        public Order order { get; set; }
       

    }
    
    public class Interaction
    {
        [JsonPropertyName("operation")]

        public string Operation { get; set; }
        [JsonPropertyName("merchant")]

        public Merchant Merchant { get; set; }
        [JsonPropertyName("returnUrl")]

        public string ReturnUrl { get; set; }
    }

    public class Merchant
    {
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("url")]

        public string Url { get; set; }
        
    }

    public class Order
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

}
