using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Payloads
{
    internal class WorldpayPaymentPayload
    {
        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }
        [JsonProperty("instruction")]
        public WorldpayPaymentPayloadInstruction Instruction { get; set; }
    }

    internal class WorldpayPaymentPayloadInstruction
    {
        [JsonProperty("originalGatewayTransactionReference", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalGatewayTransactionReference { get; set; }
        [JsonProperty("value")]
        public WorldpayPaymentPayloadValue Value { get; set; }
        [JsonProperty("paymentInstrument")]
        public WorldpayPaymentPayloadPaymentInstrument PaymentInstrument { get; set; }
    }

    internal class WorldpayPaymentPayloadPaymentInstrument
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("isHandledOnline", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsHandledOnline { get; set; }
        [JsonProperty("tokenId", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("tokenPurpose", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenPurpose { get; set; }
    }

    internal class WorldpayPaymentPayloadValue
    {
        [JsonProperty("amount")]
        public Int64 Amount { get; set; }
    }
}
