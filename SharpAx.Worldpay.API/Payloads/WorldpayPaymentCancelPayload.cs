using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Payloads
{
    internal class WorldpayPaymentCancelPayload
    {
        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("gatewayTransactionReference")]
        public string GatewayTransactionReference { get; set; }

        [JsonProperty("paymentInstrument")]
        public PaymentInstrument PaymentInstrument { get; set; }
    }

    internal class CardExpiryDate
    {
        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }

    internal class PaymentInstrument
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("cardExpiryDate")]
        public CardExpiryDate CardExpiryDate { get; set; }
    }
}
