using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayAccountVerificationInput : WorldpayInput
    {
        public WorldpayAccountVerificationInput(string merchantTransactionReference, string correlationId) : base(correlationId)
        {
            PaymentType = "sale";
            PaymentInstrumentType = "card/present";
            MerchantTransactionReference = merchantTransactionReference;
            Amount = 0;
        }

        public string PaymentType { get; }
        public string PaymentInstrumentType { get; }
        public string MerchantTransactionReference { get; }
        public int Amount { get; }
    }
}
