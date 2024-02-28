using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayReferralMethodVerificationDTO
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }
        [JsonProperty("data")]
        public WorldpayReferralMethodVerificationDataDTO Data { get; set; }
    }

    public class WorldpayReferralMethodVerificationDataDTO
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("authorisationCode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AuthorisationCode { get; set; }
    }
}
