using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Attributes;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayPointOfSaleRegistrationDTO
    {
        [JsonProperty("pointOfSaleLicenseKey")]
        [WorldpaySensitiveProperty]
        public string PointOfSaleLicenseKey { get; set; }
    }
}
