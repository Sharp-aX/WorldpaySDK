using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayReferralMethodDTO
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("data")]
        public WorldpayReferralMethodDataDTO Data { get; set; }
    }

    public class WorldpayReferralMethodDataDTO
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("referralContacts")]
        public List<WorldpayReferralMethodContactDTO> ReferralContacts { get; set; }

        [JsonProperty("merchant")]
        public WorldpayReferralMethodMerchantDTO Merchant { get; set; }
    }

    public class WorldpayReferralMethodMerchantDTO
    {
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
    }

    public class WorldpayReferralMethodContactDTO
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
