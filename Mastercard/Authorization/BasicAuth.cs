using Newtonsoft.Json;

namespace GlobalPay.HostedCheckouts.Mastercard.Authorization
{
    public  class BasicAuth
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
