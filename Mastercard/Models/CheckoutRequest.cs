namespace GlobalPay.HostedCheckouts.Mastercard.Models
{
    public class HostedCheckoutRequest
    {
        public string apiOperation { get; set; } = "INITIATE_CHECKOUT";
        public string checkoutMode { get; set; } = "WEBSITE";
        public Interaction interaction { get; set; }
        public Order order { get; set; }
    }

    public class Interaction
    {
        public string operation { get; set; } // "PURCHASE" or "AUTHORIZE"
        public Merchant merchant { get; set; }
        public string returnUrl { get; set; }
    }

    public class Merchant
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Order
    {
        public string currency { get; set; }
        public string amount { get; set; }
        public string id { get; set; }
        public string description { get; set; }
    }


}
