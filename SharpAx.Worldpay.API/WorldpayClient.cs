using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Operations;

namespace SharpAx.Worldpay.API
{
    public class WorldpayClient
    {
        public WorldpayClient(WorldpayEnvironment environment, string paypointName, string publisherId)
        {
            if (string.IsNullOrEmpty(paypointName))
                throw new ArgumentNullException(nameof(paypointName));
            if (string.IsNullOrEmpty(publisherId))
                throw new ArgumentNullException(nameof(publisherId));
            Properties.Instance.PaypointName = paypointName;
            Properties.Instance.PublisherId = publisherId;
            if (environment == WorldpayEnvironment.Testing)
                Properties.Instance.BaseUrl = $"wss://ws.muat.worldpaypp.com:443/ipc-app/payment/{paypointName}";
            else
                Properties.Instance.BaseUrl = $"wss://ipscloud.worldpay.com:443/ipc-app/payment/{paypointName}";
            Properties.Instance.Client = this;
        }

        internal List<WorldpayOperation> Operations { get; } = new List<WorldpayOperation>();

        public WorldpayClient(WorldpayEnvironment environment, string paypointName, string license, string publisherId) : this(environment, paypointName, publisherId)
        {
            if (string.IsNullOrEmpty(license))
                throw new ArgumentNullException(nameof(license));
            Properties.Instance.License = license;
        }

        public void DisconnectFromAllConnections()
        {
            foreach (var operation in Operations)
            {
                operation.DisconnectAndCloseWebSocket();
            }
            Operations.Clear();
        }

        public void ProcessRegistration(WorldpayPointOfSaleRegistrationInput input)
        {
            var registration = new WorldpayRegisterPointOfSaleOperation(input);
            registration.Process();
        }

        public void ProcessRegistrationRefresh(WorldpayPointOfSaleRegistrationRefreshInput input)
        {
            var registrationRefresh = new WorldpayPointOfSaleRegistrationRefreshOperation(input);
            registrationRefresh.Process();
        }

        public void ProcessPayment(WorldpayPaymentInput input)
        {
            var payment = new WorldpayPaymentOperation(input);
            payment.Process();
        }

        public void ProcessAccountVerification(WorldpayAccountVerificationInput input)
        {
            var accountVerification = new WorldpayAccountVerificationOperation(input);
            accountVerification.Process();
        }

        public void ProcessCancel(WorldpayPaymentCancelInput input)
        {
            var paymentCancelOperation = new WorldpayCancelOperation(input);
            paymentCancelOperation.Process();
        }

        public void ProcessSignature(WorldpayOperation worldpayOperation, WorldpayPaymentSignatureInput worldpayPaymentSignatureInput)
        {
            if (worldpayOperation is null)
                throw new ArgumentNullException(nameof(worldpayOperation));
            if (worldpayPaymentSignatureInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentSignatureInput));
            if (worldpayOperation is WorldpayPaymentOperation worldpayPaymentOperation)
                worldpayPaymentOperation.SignatureVerification(worldpayPaymentSignatureInput);
        }

        public void ProcessReferral(WorldpayOperation worldpayOperation, WorldpayPaymentReferralInput worldpayPaymentReferralInput)
        {
            if (worldpayOperation is null)
                throw new ArgumentNullException(nameof(worldpayOperation));
            if (worldpayPaymentReferralInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentReferralInput));
            if (worldpayOperation is WorldpayPaymentOperation worldpayPaymentOperation)
                worldpayPaymentOperation.ReferralVerification(worldpayPaymentReferralInput);
        }

        public void ProcessAvsMatch(WorldpayOperation worldpayOperation, WorldpayPaymentAvsInput worldpayPaymentAvsInput)
        {
            if (worldpayOperation is null)
                throw new ArgumentNullException(nameof(worldpayOperation));
            if (worldpayPaymentAvsInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentAvsInput));
            if (worldpayOperation is WorldpayPaymentOperation worldpayPaymentOperation)
                worldpayPaymentOperation.AvsMatch(worldpayPaymentAvsInput);
        }

        public event EventHandler WorldpayProcessStarting;
        internal void RaiseWorldpayProcessStarting(EventArgs e) => WorldpayProcessStarting?.Invoke(this, e);

        public delegate void WorldpayServerResponseEventHandler(WorldpayServerResponseEventArgs e);
        public event WorldpayServerResponseEventHandler ReceiveWorldpayServerResponse;
        internal void RaiseReceiveWorldpayServerResponse(WorldpayServerResponseEventArgs e) => ReceiveWorldpayServerResponse?.Invoke(e);
    }

    public class WorldpayServerResponseEventArgs : EventArgs
    {
        public WorldpayServerResponseEventArgs(WorldpayServerMessage serverMessage, WorldpayOperation operation)
        {
            ServerMessage = serverMessage;
            Operation = operation;
        }

        public WorldpayServerMessage ServerMessage { get; }  
        public WorldpayOperation Operation { get; }
    }

    public enum WorldpayEnvironment
    {
        Testing,
        Production
    }

    public enum WorldpayPaymentType
    {
        Sale,
        Refund,
        PreAuth,
        CheckCard,
        CheckCardPayment,
        Cancel,
        Unknown
    }

    public enum WorldpayPaymentInstrument
    {
        NotSet,
        CardPresent,
        CardKeyed,
        CardNotPresent,
        CardToken,
        Unknown
    }

    public enum WorldpayMessageDestination
    {
        Unknown,
        Error,
        PosRegistration,
        PaymentNotification,
        PaymentReceipt,
        PaymentResult,
        PaymentComplete,
        PaymentAction,
        PosRegistrationRefresh
    }

    public enum WorldpayPaymentOutcome
    {
        Succeeded,
        Refused,
        Pending,
        Failed,
        Unknown
    }

    public enum WorldpayPaymentResult
    {
        AuthorisedOnline,
        AuthorisedOffline,
        AuthorisedReferral,
        KeyedRecoverySuccess,
        Succeeded,
        RefusedOnline,
        RefusedOffline,
        RefusedOnlineCaptureCard,
        RefusedAvsRequired,
        RefusedDoNotReattempt,
        Started,
        InProgress,
        AuthCompleted,
        Cancelled,
        Aborted,
        AbortedDeviceUnavailable,
        AbortedBusy,
        RejectedCardNumberNotMatched,
        RejectedCardExpiryDateNotMatched,
        RejectedCardNotAccepted,
        InvalidTransactionState,
        InvalidOperation,
        InvalidGatewayTransactionsReference,
        InvalidMerchant,
        InvalidTerminal,
        InvalidMerchantStatus,
        InvalidCardNumber,
        InvalidExpiredCard,
        InvalidIssueNumber,
        InvalidCardExpiryDate,
        InvalidAmount,
        DeniedTransaction,
        DeniedCashback,
        PreValidCard,
        Failed,
        Unknown
    }

    public enum WorldpayPaymentInstrumentCardType
    {
        Fuel,
        Debit,
        Credit,
        Unknown
    }

    public enum WorldpayEntryMode
    {
        Keyed,
        Magstripe,
        IntegratedCircuitChip,
        ContactlessChip,
        ContactlessMagstripe,
        ContactlessOnDevice,
        CardholderNotPresent,
        Unknown
    }

    public enum WorldpayCardVerificationMethod
    {
        Signature,
        PinOrConsumerDevice,
        PinAndSignature,
        CardholderNotPresent,
        None,
        Unknown
    }

    public enum WorldpayReceiptType
    {
        Customer,
        Merchant,
        Signature
    }

    public enum WorldpayPaymentRequestType
    {
        Payment,
        AccountVerification
    }

    public enum WorldpayTokenPurpose
    {
        NotSet,
        Unscheduled,
        Recurring
    }
}
