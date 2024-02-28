using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayAvsVerificationDataDTO
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("isAvsConfirmed")]
        public bool IsAvsConfirmed { get; set; }
    }

    public class WorldpayAvsVerificationDTO
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("data")]
        public WorldpayAvsVerificationDataDTO Data { get; set; }
    }
}
