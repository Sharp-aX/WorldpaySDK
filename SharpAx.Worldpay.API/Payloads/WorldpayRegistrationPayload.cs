using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Payloads
{
    internal class WorldpayRegistrationPayload
    {
        [JsonProperty("pointOfSaleId")]
        public string PointOfSaleId { get; set; }
        [JsonProperty("pointOfSaleReference")]
        public string PointOfSaleReference { get; set; }
        [JsonProperty("pointOfSaleActivationCode")]
        public string PointOfSaleActivationCode { get; set; }
    }
}
