using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPaymentCancelInput : WorldpayInput
    {
        public WorldpayPaymentCancelInput(string merchantTransactionReference,
            string gatewayTransactionReference,
            string cardNumber,
            int cardExpiryDateMonth,
            int cardExpiryDateYear,
            string correlationId) : base(correlationId)
        {
            if (string.IsNullOrEmpty(merchantTransactionReference))
                throw new ArgumentNullException(nameof(merchantTransactionReference));
            if (string.IsNullOrEmpty(gatewayTransactionReference))
                throw new ArgumentNullException(nameof(gatewayTransactionReference));
            if (string.IsNullOrEmpty(cardNumber))
                throw new ArgumentNullException(nameof(cardNumber));
            if (cardExpiryDateMonth <= 0)
                throw new InvalidOperationException("Card Expiry Month cannot be lower or equal to 0.");
            if (cardExpiryDateMonth > 12)
                throw new InvalidOperationException("Card Expiry Month cannot be higher than 12.");
            MerchantTransactionReference = merchantTransactionReference;
            GatewayTransactionReference = gatewayTransactionReference;
            CardNumber = cardNumber;
            CardExpiryDateMonth = cardExpiryDateMonth;
            CardExpiryDateYear = cardExpiryDateYear;
        }

        public string MerchantTransactionReference { get; }
        public string GatewayTransactionReference { get; }
        public string CardNumber { get; }
        public int CardExpiryDateMonth { get; }
        public int CardExpiryDateYear { get; }
    }
}
