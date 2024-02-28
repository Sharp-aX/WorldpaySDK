using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPaymentReferralInput
    {
        public WorldpayPaymentReferralInput(string correlationId, string merchantTransactionReference, string authorisationCode)
        {
            if (string.IsNullOrEmpty(correlationId))
                throw new ArgumentNullException(nameof(correlationId));
            if (string.IsNullOrEmpty(merchantTransactionReference))
                throw new ArgumentNullException(nameof(merchantTransactionReference));
            CorrelationId = correlationId;
            MerchantTransactionReference = merchantTransactionReference;
            AuthorisationCode = authorisationCode;
        }

        public string CorrelationId { get; }
        public string MerchantTransactionReference { get; }
        public string AuthorisationCode { get; }
    }
}
