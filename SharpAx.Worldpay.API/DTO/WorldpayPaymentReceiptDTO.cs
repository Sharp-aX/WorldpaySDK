using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayPaymentReceiptDTO
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
