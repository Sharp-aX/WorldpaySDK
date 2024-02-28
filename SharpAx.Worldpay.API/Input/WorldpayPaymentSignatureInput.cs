using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPaymentSignatureInput
    {
        public WorldpayPaymentSignatureInput(string correlationId, string merchantTransactionReference, bool signatureVerified)
        {
            if (string.IsNullOrEmpty(correlationId))
                throw new ArgumentNullException(nameof(correlationId));
            if (string.IsNullOrEmpty(merchantTransactionReference))
                throw new ArgumentNullException(nameof(merchantTransactionReference));
            CorrelationId = correlationId;
            MerchantTransactionReference = merchantTransactionReference;
            SignatureVerified = signatureVerified;
        }

        public string CorrelationId { get; }
        public string MerchantTransactionReference { get; }
        public bool SignatureVerified { get; }
    }
}
