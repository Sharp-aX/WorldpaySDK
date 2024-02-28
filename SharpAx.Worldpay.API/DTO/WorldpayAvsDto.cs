using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayAvsResultDto
    {
        [JsonProperty("isCvvMatch")]
        public string IsCvvMatch { get; set; }

        [JsonProperty("isAddressLineMatch")]
        public string IsAddressLineMatch { get; set; }

        [JsonProperty("isPostalCodeMatch")]
        public string IsPostalCodeMatch { get; set; }
    }

    public class WorldpayAvsDataDto
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("avsResults")]
        public WorldpayAvsResultDto AvsResults { get; set; }
    }

    public class WorldpayAvsDto
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("data")]
        public WorldpayAvsDataDto Data { get; set; }
    }
}
