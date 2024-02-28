using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.DTO
{
    public class WorldpayPaymentAddressDTO
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        public string Line2 { get; set; }

        [JsonProperty("line3")]
        public string Line3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }

    public class WorldpayPaymentAuthorisationDTO
    {
        [JsonProperty("authorisationCode")]
        public string AuthorisationCode { get; set; }

        [JsonProperty("cvvResponseData")]
        public string CvvResponseData { get; set; }
    }

    public class WorldpayPaymentCardDTO
    {
        [JsonProperty("tokenId")]
        public string TokenId { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("expiryDate")]
        public WorldpayPaymentExpiryDateDTO ExpiryDate { get; set; }

        [JsonProperty("track2Data")]
        public string Track2Data { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("issuerCode")]
        public string IssuerCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("panSequenceNumber")]
        public string PanSequenceNumber { get; set; }

        [JsonProperty("applicationLabel")]
        public string ApplicationLabel { get; set; }

        [JsonProperty("applicationIdentifier")]
        public string ApplicationIdentifier { get; set; }

        [JsonProperty("applicationEffectiveDate")]
        public string ApplicationEffectiveDate { get; set; }
    }

    public class WorldpayPaymentDccDTO
    {
        [JsonProperty("convertedCurrencyCode")]
        public string ConvertedCurrencyCode { get; set; }

        [JsonProperty("convertedAmount")]
        public int ConvertedAmount { get; set; }

        [JsonProperty("conversionRate")]
        public decimal ConversionRate { get; set; }
    }

    public class WorldpayPaymentDebugDTO
    {
        [JsonProperty("transactionStatusInfo")]
        public string TransactionStatusInfo { get; set; }

        [JsonProperty("transactionVerificationResults")]
        public string TransactionVerificationResults { get; set; }
    }

    public class WorldpayPaymentExpiryDateDTO
    {
        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }

    public class WorldpayPaymentMerchantDTO
    {
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public WorldpayPaymentAddressDTO Address { get; set; }
    }

    public class WorldpayPaymentDTO
    {
        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("transactionDateTime")]
        public DateTime TransactionDateTime { get; set; }

        [JsonProperty("outcome")]
        public string Outcome { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("merchant")]
        public WorldpayPaymentMerchantDTO Merchant { get; set; }

        [JsonProperty("paypoint")]
        public WorldpayPaymentPaypointDTO Paypoint { get; set; }

        [JsonProperty("value")]
        public WorldpayPaymentValueDTO Value { get; set; }

        [JsonProperty("merchantTransactionReference")]
        public string MerchantTransactionReference { get; set; }

        [JsonProperty("gatewayTransactionReference")]
        public string GatewayTransactionReference { get; set; }

        [JsonProperty("eftSequenceNumber")]
        public int EftSequenceNumber { get; set; }

        [JsonProperty("retrievalReferenceNumber")]
        public int RetrievalReferenceNumber { get; set; }

        [JsonProperty("receiptNumber")]
        public int ReceiptNumber { get; set; }

        [JsonProperty("receiptRetentionReminder")]
        public string ReceiptRetentionReminder { get; set; }

        [JsonProperty("receiptCustomerDeclaration")]
        public string ReceiptCustomerDeclaration { get; set; }

        [JsonProperty("taxFreeVoucher")]
        public string TaxFreeVoucher { get; set; }

        [JsonProperty("paymentInstrument")]
        public WorldpayPaymentInstrumentDTO PaymentInstrument { get; set; }
    }

    public class WorldpayPaymentInstrumentDTO
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("card")]
        public WorldpayPaymentCardDTO Card { get; set; }

        [JsonProperty("posEntryMode")]
        public string PosEntryMode { get; set; }

        [JsonProperty("cardVerificationMethod")]
        public string CardVerificationMethod { get; set; }

        [JsonProperty("isHandledOnline")]
        public bool IsHandledOnline { get; set; }

        [JsonProperty("authorisation")]
        public WorldpayPaymentAuthorisationDTO Authorisation { get; set; }

        [JsonProperty("debug")]
        public WorldpayPaymentDebugDTO Debug { get; set; }
    }

    public class WorldpayPaymentPaypointDTO
    {
        [JsonProperty("paypointId")]
        public string PaypointId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
    }

    public class WorldpayPaymentResultDTO
    {
        [JsonProperty("payments")]
        public List<WorldpayPaymentDTO> Payments { get; set; }
    }

    public class WorldpayPaymentValueDTO
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("cashbackAmount")]
        public int CashbackAmount { get; set; }

        [JsonProperty("gratuityAmount")]
        public int GratuityAmount { get; set; }

        [JsonProperty("donationAmount")]
        public int DonationAmount { get; set; }

        [JsonProperty("dcc")]
        public WorldpayPaymentDccDTO Dcc { get; set; }
    }
}
