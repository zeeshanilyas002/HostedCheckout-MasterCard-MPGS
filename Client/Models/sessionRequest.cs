using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class sessionRequest
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public string orderId { get; set; }
        public string returnUrl { get; set; }
        public string apiOperation { get; set; } = "PURCHASE";
        public MerchantInfo merchant { get; set; }

    }


    public class MerchantInfo
    {
        public string name { get; set; }
        public MerchantAddress address { get; set; }
    }

    public class MerchantAddress
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
    }
}
