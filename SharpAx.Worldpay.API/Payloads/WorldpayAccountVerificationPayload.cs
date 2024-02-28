using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Payloads
{
    internal class WorldpayAccountVerificationPayload
    {
        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("instruction")]
        public WorldpayAccountVerificationPayloadInstruction Instruction { get; set; }
    }

    internal class WorldpayAccountVerificationPayloadInstruction
    {
        [JsonProperty("value")]
        public WorldpayAccountVerificationPayloadValue Value { get; set; }

        [JsonProperty("narrative")]
        public WorldpayAccountVerificationPayloadNarrative Narrative { get; set; }

        [JsonProperty("paymentInstrument")]
        public WorldpayAccountVerificationPayloadPaymentInstrument PaymentInstrument { get; set; }
    }

    internal class WorldpayAccountVerificationPayloadNarrative
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }
    }

    internal class WorldpayAccountVerificationPayloadPaymentInstrument
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isHandledOnline")]
        public bool IsHandledOnline { get; set; }

        [JsonProperty("tokenPurpose")]
        public string TokenPurpose { get; set; }
    }

    internal class WorldpayAccountVerificationPayloadValue
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
