using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPaymentInput : WorldpayInput
    {
        public WorldpayPaymentInput(WorldpayPaymentType paymentType, string merchantTransactionReference, decimal amount, WorldpayPaymentInstrument paymentInstrument, string correlationId) : base(correlationId)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Amount must be higher than 0!");
            if (string.IsNullOrEmpty(merchantTransactionReference))
                throw new ArgumentNullException(nameof(merchantTransactionReference));
            OriginalPaymentType = paymentType;
            MerchantTransactionReference = merchantTransactionReference;
            OriginalAmount = amount;
            OriginalPaymentInstrumentType = paymentInstrument;
            HandleInput();
        }

        public WorldpayPaymentInput(WorldpayPaymentType paymentType, string merchantTransactionReference, decimal amount, WorldpayPaymentInstrument paymentInstrument, string correlationId, string token, WorldpayTokenPurpose tokenPurpose) : this(paymentType, merchantTransactionReference, amount, paymentInstrument, correlationId)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (tokenPurpose == WorldpayTokenPurpose.NotSet)
                throw new InvalidOperationException("Token purpose is not set.");
            Token = token;
            TokenPurpose = tokenPurpose.ToString().ToLower();
        }

        public void SetOriginalGatewayTransactionReference(string originalGatewayTransactionReference)
        {
            if (string.IsNullOrEmpty(originalGatewayTransactionReference))
                throw new ArgumentNullException(nameof(originalGatewayTransactionReference));
            OriginalGatewayTransactionReference = originalGatewayTransactionReference;
        }

        private void HandleInput()
        {
            if (OriginalPaymentType == SharpAx.Worldpay.API.WorldpayPaymentType.Sale)
                PaymentType = "sale";
            else if (OriginalPaymentType == SharpAx.Worldpay.API.WorldpayPaymentType.Refund)
                PaymentType = "refund";
            else if (OriginalPaymentType == SharpAx.Worldpay.API.WorldpayPaymentType.PreAuth)
                PaymentType = "pre-auth";
            else if (OriginalPaymentType == SharpAx.Worldpay.API.WorldpayPaymentType.CheckCard)
                PaymentType = "check-card";
            else if (OriginalPaymentType == SharpAx.Worldpay.API.WorldpayPaymentType.CheckCardPayment)
                PaymentType = "check-card-payment";

            if (OriginalPaymentInstrumentType == WorldpayPaymentInstrument.CardPresent)
                PaymentInstrumentType = "card/present";
            else if (OriginalPaymentInstrumentType == WorldpayPaymentInstrument.CardKeyed)
                PaymentInstrumentType = "card/keyed";
            else if (OriginalPaymentInstrumentType == WorldpayPaymentInstrument.CardNotPresent)
                PaymentInstrumentType = "card/not-present";
            else if (OriginalPaymentInstrumentType == WorldpayPaymentInstrument.CardToken)
                PaymentInstrumentType = "card/token";

            Amount = Convert.ToInt32(OriginalAmount * 100);
        }

        public WorldpayPaymentType OriginalPaymentType { get; }
        public decimal OriginalAmount { get; }
        public WorldpayPaymentInstrument OriginalPaymentInstrumentType { get; }
        public string PaymentType { get; private set; }
        public string MerchantTransactionReference { get; }
        public Int64 Amount { get; private set; }
        public string PaymentInstrumentType { get; private set; }
        public string Token { get; }
        public string TokenPurpose { get; }
        public string OriginalGatewayTransactionReference { get; private set; }
    }
}
