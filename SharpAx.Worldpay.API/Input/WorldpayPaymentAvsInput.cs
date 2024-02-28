using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPaymentAvsInput
    {
        public WorldpayPaymentAvsInput(string correlationId, string merchantTransactionReference, bool avsConfirmed)
        {
            if (string.IsNullOrEmpty(correlationId))
                throw new ArgumentNullException(nameof(correlationId));
            if (string.IsNullOrEmpty(merchantTransactionReference))
                throw new ArgumentNullException(nameof(merchantTransactionReference));
            CorrelationId = correlationId;
            MerchantTransactionReference = merchantTransactionReference;
            AvsConfirmed = avsConfirmed;
        }

        public string CorrelationId { get; }
        public string MerchantTransactionReference { get; }
        public bool AvsConfirmed { get; }
    }
}
