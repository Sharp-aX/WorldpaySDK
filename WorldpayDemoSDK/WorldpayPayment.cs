using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WorldPayDemoSDK
{
    public class WorldpayPayment : DatabaseClass
    {
        private object lockObject = new object();

        public WorldpayPayment()
        {
        }


        private WorldpayPayment _RefundPayment;
        public WorldpayPayment RefundPayment
        {
            get { return _RefundPayment; }
            set { SetPropertyValue<WorldpayPayment>(nameof(RefundPayment), ref _RefundPayment, value); }
        }

        private WorldpayPayment _RefundedPayment;
        public WorldpayPayment RefundedPayment
        {
            get { return _RefundedPayment; }
            set { SetPropertyValue<WorldpayPayment>(nameof(RefundedPayment), ref _RefundedPayment, value); }
        }

        private WorldpayPaymentResult _PaymentResult;
        public WorldpayPaymentResult PaymentResult
        {
            get { return _PaymentResult; }
            set { SetPropertyValue(nameof(PaymentResult), ref _PaymentResult, value); }
        }

        private WorldpayPaymentType _PaymentType;
        public WorldpayPaymentType PaymentType
        {
            get { return _PaymentType; }
            set { SetPropertyValue(nameof(PaymentType), ref _PaymentType, value); }
        }

        private DateTime _TransactionDateTime;
        public DateTime TransactionDateTime
        {
            get { return _TransactionDateTime; }
            set { SetPropertyValue<DateTime>(nameof(TransactionDateTime), ref _TransactionDateTime, value); }
        }

        private WorldpayPaymentOutcome _Outcome;
        public WorldpayPaymentOutcome Outcome
        {
            get { return _Outcome; }
            set { SetPropertyValue(nameof(Outcome), ref _Outcome, value); }
        }

        private SharpAx.Worldpay.API.WorldpayPaymentResult _Result;
        public SharpAx.Worldpay.API.WorldpayPaymentResult Result
        {
            get { return _Result; }
            set { SetPropertyValue(nameof(Result), ref _Result, value); }
        }

        private WorldpayMerchant _Merchant;
        public WorldpayMerchant Merchant
        {
            get { return _Merchant; }
            set { SetPropertyValue(nameof(Merchant), ref _Merchant, value); }
        }

        private string _PaypointId;
        public string PaypointId
        {
            get { return _PaypointId; }
            set { SetPropertyValue<string>(nameof(PaypointId), ref _PaypointId, value); }
        }

        private string _TerminalId;
        public string TerminalId
        {
            get { return _TerminalId; }
            set { SetPropertyValue<string>(nameof(TerminalId), ref _TerminalId, value); }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue<decimal>(nameof(Amount), ref _Amount, value); }
        }

        private decimal _CashbackAmount;
        public decimal CashbackAmount
        {
            get { return _CashbackAmount; }
            set { SetPropertyValue<decimal>(nameof(CashbackAmount), ref _CashbackAmount, value); }
        }

        private decimal _GratuityAmount;
        public decimal GratuityAmount
        {
            get { return _GratuityAmount; }
            set { SetPropertyValue<decimal>(nameof(GratuityAmount), ref _GratuityAmount, value); }
        }

        private decimal _DonationAmount;
        public decimal DonationAmount
        {
            get { return _DonationAmount; }
            set { SetPropertyValue<decimal>(nameof(DonationAmount), ref _DonationAmount, value); }
        }

        private int _AmountInMinorCurrencyUnits;
        public int AmountInMinorCurrencyUnits
        {
            get { return _AmountInMinorCurrencyUnits; }
            set { SetPropertyValue<int>(nameof(AmountInMinorCurrencyUnits), ref _AmountInMinorCurrencyUnits, value); }
        }

        private int _CashbackAmountInMinorCurrencyUnits;
        public int CashbackAmountInMinorCurrencyUnits
        {
            get { return _CashbackAmountInMinorCurrencyUnits; }
            set { SetPropertyValue<int>(nameof(CashbackAmountInMinorCurrencyUnits), ref _CashbackAmountInMinorCurrencyUnits, value); }
        }

        private int _GratuityAmountInMinorCurrencyUnits;
        public int GratuityAmountInMinorCurrencyUnits
        {
            get { return _GratuityAmountInMinorCurrencyUnits; }
            set { SetPropertyValue<int>(nameof(GratuityAmountInMinorCurrencyUnits), ref _GratuityAmountInMinorCurrencyUnits, value); }
        }

        private int _DonationAmountInMinorCurrencyUnits;
        public int DonationAmountInMinorCurrencyUnits
        {
            get { return _DonationAmountInMinorCurrencyUnits; }
            set { SetPropertyValue<int>(nameof(DonationAmountInMinorCurrencyUnits), ref _DonationAmountInMinorCurrencyUnits, value); }
        }

        private string _Currency;
        public string Currency
        {
            get { return _Currency; }
            set { SetPropertyValue<string>(nameof(Currency), ref _Currency, value); }
        }

        private string _ConvertedCurrency;
        public string ConvertedCurrency
        {
            get { return _ConvertedCurrency; }
            set { SetPropertyValue<string>(nameof(ConvertedCurrency), ref _ConvertedCurrency, value); }
        }

        private decimal _ConvertedAmount;
        public decimal ConvertedAmount
        {
            get { return _ConvertedAmount; }
            set { SetPropertyValue<decimal>(nameof(ConvertedAmount), ref _ConvertedAmount, value); }
        }

        private int _ConvertedAmountInMinorCurrencyUnits;
        public int ConvertedAmountInMinorCurrencyUnits
        {
            get { return _ConvertedAmountInMinorCurrencyUnits; }
            set { SetPropertyValue<int>(nameof(ConvertedAmountInMinorCurrencyUnits), ref _ConvertedAmountInMinorCurrencyUnits, value); }
        }

        private decimal _ConversionRate;
        public decimal ConversionRate
        {
            get { return _ConversionRate; }
            set { SetPropertyValue<decimal>(nameof(ConversionRate), ref _ConversionRate, value); }
        }

        private string _MerchantTransactionReference;
        public string MerchantTransactionReference
        {
            get { return _MerchantTransactionReference; }
            set { SetPropertyValue<string>(nameof(MerchantTransactionReference), ref _MerchantTransactionReference, value); }
        }

        private string _GatewayTransactionReference;
        public string GatewayTransactionReference
        {
            get { return _GatewayTransactionReference; }
            set { SetPropertyValue<string>(nameof(GatewayTransactionReference), ref _GatewayTransactionReference, value); }
        }

        private int _TransactionSequenceNumber;
        public int TransactionSequenceNumber
        {
            get { return _TransactionSequenceNumber; }
            set { SetPropertyValue<int>(nameof(TransactionSequenceNumber), ref _TransactionSequenceNumber, value); }
        }

        private int _RetrievalReferenceNumber;
        public int RetrievalReferenceNumber
        {
            get { return _RetrievalReferenceNumber; }
            set { SetPropertyValue<int>(nameof(RetrievalReferenceNumber), ref _RetrievalReferenceNumber, value); }
        }

        private int _ReceiptNumber;
        public int ReceiptNumber
        {
            get { return _ReceiptNumber; }
            set { SetPropertyValue<int>(nameof(ReceiptNumber), ref _ReceiptNumber, value); }
        }

        private string _ReceiptRetentionReminder;
        public string ReceiptRetentionReminder
        {
            get { return _ReceiptRetentionReminder; }
            set { SetPropertyValue<string>(nameof(ReceiptRetentionReminder), ref _ReceiptRetentionReminder, value); }
        }

        private string _ReceiptCustomDeclaration;
        public string ReceiptCustomDeclaration
        {
            get { return _ReceiptCustomDeclaration; }
            set { SetPropertyValue<string>(nameof(ReceiptCustomDeclaration), ref _ReceiptCustomDeclaration, value); }
        }

        private WorldpayPaymentInstrument _PaymentInstrumentType;
        public WorldpayPaymentInstrument PaymentInstrumentType
        {
            get { return _PaymentInstrumentType; }
            set { SetPropertyValue<WorldpayPaymentInstrument>(nameof(PaymentInstrumentType), ref _PaymentInstrumentType, value); }
        }

        private string _CardTokenId;
        [PasswordPropertyText(true)]
        public string CardTokenId
        {
            get { return _CardTokenId; }
            set { SetPropertyValue<string>(nameof(CardTokenId), ref _CardTokenId, value); }
        }

        private string _CardNumber;
        public string CardNumber
        {
            get { return _CardNumber; }
            set { SetPropertyValue<string>(nameof(CardNumber), ref _CardNumber, value); }
        }

        private int _CardExpiryDateMonth;
        public int CardExpiryDateMonth
        {
            get { return _CardExpiryDateMonth; }
            set { SetPropertyValue<int>(nameof(CardExpiryDateMonth), ref _CardExpiryDateMonth, value); }
        }

        private int _CardExpiryDateYear;
        public int CardExpiryDateYear
        {
            get { return _CardExpiryDateYear; }
            set { SetPropertyValue<int>(nameof(CardExpiryDateYear), ref _CardExpiryDateYear, value); }
        }

        private string _Track2Data;
        public string Track2Data
        {
            get { return _Track2Data; }
            set { SetPropertyValue<string>(nameof(Track2Data), ref _Track2Data, value); }
        }

        private WorldpayPaymentInstrumentCardType _CardType;
        public WorldpayPaymentInstrumentCardType CardType
        {
            get { return _CardType; }
            set { SetPropertyValue<WorldpayPaymentInstrumentCardType>(nameof(CardType), ref _CardType, value); }
        }

        private string _CardIssuerCode;
        public string CardIssuerCode
        {
            get { return _CardIssuerCode; }
            set { SetPropertyValue<string>(nameof(CardIssuerCode), ref _CardIssuerCode, value); }
        }

        private string _CardCountryCode;
        public string CardCountryCode
        {
            get { return _CardCountryCode; }
            set { SetPropertyValue<string>(nameof(CardCountryCode), ref _CardCountryCode, value); }
        }

        private string _PANSequenceNumber;
        [System.ComponentModel.DisplayName("PAN Sequence Number")]
        public string PANSequenceNumber
        {
            get { return _PANSequenceNumber; }
            set { SetPropertyValue<string>(nameof(PANSequenceNumber), ref _PANSequenceNumber, value); }
        }

        private string _CardApplicationLabel;
        public string CardApplicationLabel
        {
            get { return _CardApplicationLabel; }
            set { SetPropertyValue<string>(nameof(CardApplicationLabel), ref _CardApplicationLabel, value); }
        }

        private string _CardApplicationIdentifier;
        public string CardApplicationIdentifier
        {
            get { return _CardApplicationIdentifier; }
            set { SetPropertyValue<string>(nameof(CardApplicationIdentifier), ref _CardApplicationIdentifier, value); }
        }

        private DateTime _CardApplicationEffectiveDate;
        public DateTime CardApplicationEffectiveDate
        {
            get { return _CardApplicationEffectiveDate; }
            set { SetPropertyValue<DateTime>(nameof(CardApplicationEffectiveDate), ref _CardApplicationEffectiveDate, value); }
        }

        private WorldpayEntryMode _EntryMode;
        public WorldpayEntryMode EntryMode
        {
            get { return _EntryMode; }
            set { SetPropertyValue<WorldpayEntryMode>(nameof(EntryMode), ref _EntryMode, value); }
        }

        private WorldpayCardVerificationMethod _CardVerificationMethod;
        public WorldpayCardVerificationMethod CardVerificationMethod
        {
            get { return _CardVerificationMethod; }
            set { SetPropertyValue<WorldpayCardVerificationMethod>(nameof(CardVerificationMethod), ref _CardVerificationMethod, value); }
        }

        private bool _IsHandledOnline;
        public bool IsHandledOnline
        {
            get { return _IsHandledOnline; }
            set { SetPropertyValue<bool>(nameof(IsHandledOnline), ref _IsHandledOnline, value); }
        }

        private string _AuthorisationCode;
        public string AuthorisationCode
        {
            get { return _AuthorisationCode; }
            set { SetPropertyValue<string>(nameof(AuthorisationCode), ref _AuthorisationCode, value); }
        }

        private string _CvvResponseData;
        [System.ComponentModel.DisplayName("CVV Response Data")]
        public string CvvResponseData
        {
            get { return _CvvResponseData; }
            set { SetPropertyValue<string>(nameof(CvvResponseData), ref _CvvResponseData, value); }
        }

        private string _CustomerReceipt;
        public string CustomerReceipt
        {
            get { return _CustomerReceipt; }
            set { SetPropertyValue<string>(nameof(CustomerReceipt), ref _CustomerReceipt, value); }
        }

        private string _MerchantReceipt;
        public string MerchantReceipt
        {
            get { return _MerchantReceipt; }
            set { SetPropertyValue<string>(nameof(MerchantReceipt), ref _MerchantReceipt, value); }
        }

        private string _SignatureReceipt;
        public string SignatureReceipt
        {
            get { return _SignatureReceipt; }
            set { SetPropertyValue<string>(nameof(SignatureReceipt), ref _SignatureReceipt, value); }
        }

        private string _SaleGatewayTransactionReference;
        [System.ComponentModel.Browsable(false)]
        public string SaleGatewayTransactionReference
        {
            get { return _SaleGatewayTransactionReference; }
            set { SetPropertyValue<string>(nameof(SaleGatewayTransactionReference), ref _SaleGatewayTransactionReference, value); }
        }


        public override void OnSaved()
        {
            base.OnSaved();
            lock (lockObject)
            {
                Singleton.Instance.LatestWorldpayPayment = this;
            }
        }


    }



    public class WorldpayPaymentResult : DatabaseClass
    {
        public WorldpayPaymentResult()
        {
            InternalStatus = WorldpayInternalStatus.NotStarted;
            Created = DateTime.Now;
        }

        public WorldpayPaymentResult(WorldpayDevice device) : this()
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));
            Device = device;
        }

        public bool Successful;

        private DateTime _Created;
        public DateTime Created
        {
            get { return _Created; }
            set { SetPropertyValue<DateTime>(nameof(Created), ref _Created, value); }
        }

        private WorldpayInternalStatus _InternalStatus;
        [System.ComponentModel.Browsable(false)]
        public WorldpayInternalStatus InternalStatus
        {
            get { return _InternalStatus; }
            set { SetPropertyValue<WorldpayInternalStatus>(nameof(InternalStatus), ref _InternalStatus, value); }
        }

        private WorldpayDevice _Device;
        public WorldpayDevice Device
        {
            get { return _Device; }
            set { SetPropertyValue<WorldpayDevice>(nameof(Device), ref _Device, value); }
        }

        public List<WorldpayPayment> Payments
        {
            get {
                 List<WorldpayPayment> LocalWorldpayPaymentFromDatabase = new List<WorldpayPayment>();
                /*
                 * select all from LocalWorldpayPayment in database where LocalWorldpayPayment.PaymentResult is this instance and add to the list
                 */
                LocalWorldpayPaymentFromDatabase.Add(new WorldpayPayment() { });
                return LocalWorldpayPaymentFromDatabase; 
            }
        }
    }

    public class WorldpayMerchant :DatabaseClass
    {
        public WorldpayMerchant()
        {
        }

        private string _MerchantId;
        public string MerchantId
        {
            get { return _MerchantId; }
            set { SetPropertyValue<string>(nameof(MerchantId), ref _MerchantId, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }

        private string _AddressLine1;
        public string AddressLine1
        {
            get { return _AddressLine1; }
            set { SetPropertyValue<string>(nameof(AddressLine1), ref _AddressLine1, value); }
        }

        private string _AddressLine2;
        public string AddressLine2
        {
            get { return _AddressLine2; }
            set { SetPropertyValue<string>(nameof(AddressLine2), ref _AddressLine2, value); }
        }

        private string _AddressLine3;
        public string AddressLine3
        {
            get { return _AddressLine3; }
            set { SetPropertyValue<string>(nameof(AddressLine3), ref _AddressLine3, value); }
        }

        private string _City;
        public string City
        {
            get { return _City; }
            set { SetPropertyValue<string>(nameof(City), ref _City, value); }
        }

        private string _State;
        public string State
        {
            get { return _State; }
            set { SetPropertyValue<string>(nameof(State), ref _State, value); }
        }

        private string _PostalCode;
        public string PostalCode
        {
            get { return _PostalCode; }
            set { SetPropertyValue<string>(nameof(PostalCode), ref _PostalCode, value); }
        }

        private string _CountryCode;
        public string CountryCode
        {
            get { return _CountryCode; }
            set { SetPropertyValue<string>(nameof(CountryCode), ref _CountryCode, value); }
        }

    }

    public class WorldpayPaymentStatus
    {
        private SharpAx.Worldpay.API.WorldpayPaymentResult result;

        public WorldpayPaymentStatus(WorldpayPaymentOutcome outcome, SharpAx.Worldpay.API.WorldpayPaymentResult result)
        {
            Outcome = outcome;
            this.result = result;
            SetProperties();
        }

        private void SetProperties()
        {
            if (Outcome == WorldpayPaymentOutcome.Succeeded)
            {
                IsSuccessful = true;
                if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedOnline)
                    ResultText = "Authorised Online: The payment was authorised by card issuer, payment scheme or your acquirer.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedOffline)
                    ResultText = "Authorised Offline: The payment was accepted locally by IPS.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedReferral)
                    ResultText = "Authorised Referral: The payment was accepted locally by IPS on entry of a referral code by the POS attendant.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.KeyedRecoverySuccess)
                    ResultText = "Keyed Recovery Success: The keyed recovery payment was successfully stored by the Integrated POS gateway and will be submitted for settlement to your acquirer.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.Succeeded)
                    ResultText = "Suceeded: The abort, cancel or settle request was processed successfully.";
            }
            else if (Outcome == WorldpayPaymentOutcome.Refused)
            {
                IsSuccessful = false;
                IsRefused = true;
                if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOnline)
                    ResultText = "Refused Online: The payment was refused by card issuer, payment scheme or your acquirer.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOffline)
                    ResultText = "Refused Offline: The payment was refused locally by IPS.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOnlineCaptureCard)
                    ResultText = "Refused Online Capture Card: The payment was refused by card issuer, payment scheme or your acquirer and the card should be kept by the merchant.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedAvsRequired)
                    ResultText = "Refused AVS Required: The payment was refused by card issuer, payment scheme or your acquirer because AVS was required but not provided.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedDoNotReattempt)
                    ResultText = "Refused Do Not Reattempt: The payment was refused and no further attempts should be made with the card.";
            }
            else if (Outcome == WorldpayPaymentOutcome.Pending)
            {
                IsSuccessful = false;
                IsPending = true;
                if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.Started)
                    ResultText = "Started: The payment process has started.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InProgress)
                    ResultText = "In Progress: The payment authorisation is in progress.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AuthCompleted)
                    ResultText = "Auth Completed: The payment authorisation process has completed. At this stage the payment record may not yet be saved to the Integrated POS gateway.";
            }
            else if (Outcome == WorldpayPaymentOutcome.Failed)
            {
                IsSuccessful = false;
                IsFailed = true;
                if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.Cancelled)
                    ResultText = "Cancelled: The request process was cancelled by the merchant or customer.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.Aborted)
                    ResultText = "Aborted: The request was has been aborted unexpectedly.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AbortedDeviceUnavailable)
                    ResultText = "Aborted Device Unavailable: The request been aborted because the payment device is unavailable.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.AbortedBusy)
                    ResultText = "Aborted Busy: The request been aborted because the system is currently busy.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardNumberNotMatched)
                    ResultText = "Rejected Card Number Not Matched: The request was rejected because the card number did not match the expected value.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardExpiryDateNotMatched)
                    ResultText = "Rejected Card Expiry Date Not Matched: The request was rejected because the card expiry date did not match the expected value.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardNotAccepted)
                    ResultText = "Rejected Card Not Accepted: The request was rejected because the card use is not accepted by the system.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidTransactionState)
                    ResultText = "Invalid Transaction State: The request cannot be performed on the transaction in its current state.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidOperation)
                    ResultText = "Invalid Operation: The requested operation is not supported.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidGatewayTransactionsReference)
                    ResultText = "Invalid Gateway Transactions Reference: The request cannot be processed because the gateway transaction reference is not recognised.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidMerchant)
                    ResultText = "Invalid Merchant: The request cannot be processed as the configured merchant details are invalid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidTerminal)
                    ResultText = "Invalid Terminal: The request cannot be processed as the configured terminal details are invalid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidMerchantStatus)
                    ResultText = "Invalid Merchant Status: The request cannot be processed as merchant is not active.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidCardNumber)
                    ResultText = "Invalid Card Number: The request cannot be processed as the card number is invalid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidExpiredCard)
                    ResultText = "Invalid Expired Card: The request cannot be processed as the card has expired.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidIssueNumber)
                    ResultText = "Invalid Issue Number: The request cannot be processed as the card issuer number is invalid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidCardExpiryDate)
                    ResultText = "Invalid Card Expiry Date: The request cannot be processed as the card expiry date is invalid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidAmount)
                    ResultText = "Invalid Amount: The requested sale or refund amount is not valid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.DeniedTransaction)
                    ResultText = "Denied Transaction: The transaction was denied.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.DeniedCashback)
                    ResultText = "Denied Cashback: The request for cashback was denied.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.PreValidCard)
                    ResultText = "Pre-Valid Card: The request cannot be processed as the card is not yet valid.";
                else if (result == SharpAx.Worldpay.API.WorldpayPaymentResult.Failed)
                    ResultText = "Failed: The request could not be processed.";
            }
        }

        public bool IsSuccessful { get; private set; }
        public bool IsPending { get; private set; }
        public bool IsRefused { get; private set; }
        public bool IsFailed { get; private set; }
        public WorldpayPaymentOutcome Outcome { get; private set; }
        public string ResultText { get; private set; }
    }


    public enum WorldpayInternalStatus
    {
        NotStarted,
        InProcess,
        Completed
    }




}
