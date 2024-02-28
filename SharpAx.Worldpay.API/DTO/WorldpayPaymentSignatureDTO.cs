using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayPaymentSignatureDTO
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("data")]
        public WorldpayPaymentSignatureDataDTO Data { get; set; }
    }

    public class WorldpayPaymentSignatureDataDTO
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("isSignatureVerified", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSignatureVerified { get; set; }
    }
}
