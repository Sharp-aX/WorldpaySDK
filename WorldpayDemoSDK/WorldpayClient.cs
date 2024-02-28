using SharpAx.Worldpay.API.DTO;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using worldpay = SharpAx.Worldpay.API;

namespace WorldPayDemoSDK
{

    public class DemoWorldpayClient
    {
        private List<WorldpayClient> clients = new List<WorldpayClient>();
        private List<WorldpayPayment> completedTransactions = new List<WorldpayPayment>();
        private readonly TimeSpan timeout;
        private System.Timers.Timer timer = null;

        public DemoWorldpayClient(WorldpayEnvironment environment, IWorldpayPaymentForm paymentForm, WorldpayDevice device, TimeSpan timeout)
        {
            if (paymentForm is null)
                throw new ArgumentNullException(nameof(paymentForm));
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            Environment = environment;
            PaymentForm = paymentForm;
            Device = device;
            this.timeout = timeout;
            PaymentForm.PaymentCancel += PaymentForm_PaymentAbort;
            PaymentForm.Disconnect += PaymentForm_Disconnect;
            PaymentForm.TerminateTransaction += PaymentForm_TerminateTransaction;
        }

        private void PaymentForm_TerminateTransaction(WorldpayTerminateTransactionEventArgs e)
        {
            if (e.Reason == WorldpayTerminateReason.UserTerminated)
            {
                if (timer != null)
                    timer.Stop();
                PaymentForm.SetText("User terminated transaction.");
                PaymentForm.SetExitButtonState(true);
                DisconnectAllClients();
            }
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer(timeout.TotalMilliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void RestartTimer()
        {
            if (timer != null && timer.Enabled)
            {
                timer.Stop();
                timer.Start();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
            PaymentForm.SetText("Transaction timeout.");
            PaymentForm.SetExitButtonState(true);
            DisconnectAllClients();
        }

        private void DisconnectAllClients()
        {
            foreach (var client in clients)
            {
                client.DisconnectFromAllConnections();
            }
            clients.Clear();
            //InternalDeviceTrackingTable.SetWorldpayDeviceUseState(Device, false);
        }

        public IReadOnlyCollection<WorldpayPayment> CompletedTransactions => completedTransactions;

        private void PaymentForm_Disconnect(WorldpayDisconnectEventArgs e)
        {
            DisconnectAllClients();
        }

        private void PaymentForm_PaymentAbort(WorldpayPaymentCancelEventArgs e)
        {
            var sdkClient = new WorldpayClient(Environment, Device.PaypointName, Device.LicenseText, Guid.NewGuid().ToString());
        }

        public WorldpayEnvironment Environment { get; }
        public IWorldpayPaymentForm PaymentForm { get; }
        public WorldpayDevice Device { get; }

        public bool MakePaymentOperation(WorldpayPaymentOperationData worldpayPaymentOperationData, bool skipTokenCheck = false)
        {
            WorldpayToken tokenAndStatus = null;
            var successful = false;
            if (!skipTokenCheck && worldpayPaymentOperationData.PaymentInstrument == WorldpayPaymentInstrument.CardToken)
            {
                tokenAndStatus = worldpayPaymentOperationData.CheckToken();
                if (tokenAndStatus.TokenStatus != WorldpayTokenStatus.Valid)
                {
                    MessageBox.Show(tokenAndStatus.TokenStatusMessage, "Token Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    successful = false;
                    return successful;
                }
            }

            
            var correlationId = Guid.NewGuid().ToString();
            var sdkClient = new WorldpayClient(Environment, Device.PaypointName, Device.LicenseText, Guid.NewGuid().ToString());
            clients.Add(sdkClient);
            var paymentResult = CreatePaymentResult();
            sdkClient.WorldpayProcessStarting += (client, args) =>
            {
                paymentResult.InternalStatus = WorldpayInternalStatus.InProcess;
            };
            sdkClient.ReceiveWorldpayServerResponse += (response) =>
            {
                if (response.ServerMessage != null && response.ServerMessage.DoesCorrelationIdMatch(correlationId))
                {
                    RestartTimer();
                    if (response.ServerMessage.MessageType == StompMessageServerType.Error)
                    {
                        PaymentForm.SetText("");
                        PaymentForm.SetExitButtonState(true);
                        PaymentForm.StopProgressCheck();
                        sdkClient.DisconnectFromAllConnections();
                        successful = false;
                    }
                    else if (response.ServerMessage.MessageType == StompMessageServerType.Message)
                    {
                        if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.PaymentNotification)
                        {
                            var notification = response.ServerMessage.DeserializeBody<WorldpayPaymentNotificationDTO>();
                            PaymentForm.SetText(notification.NotificationText);
                        }
                        else if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.PaymentAction)
                        {
                            if (response.ServerMessage.Body.ToLower().Contains("signature-verification"))
                            {
                                var signatureVerification = response.ServerMessage.DeserializeBody<WorldpayPaymentSignatureDTO>();
                                if (signatureVerification != null && response.Operation != null)
                                {
                                    WorldpayPaymentSignatureInput signatureVerificationInput = null;
                                    if (PaymentForm.AskQuestion("Does the signature match?") == System.Windows.Forms.DialogResult.Yes)
                                        signatureVerificationInput = new WorldpayPaymentSignatureInput(correlationId, signatureVerification.MerchantTransactionReference, true);
                                    else
                                        signatureVerificationInput = new WorldpayPaymentSignatureInput(correlationId, signatureVerification.MerchantTransactionReference, false);
                                    sdkClient.ProcessSignature(response.Operation, signatureVerificationInput);
                                }
                            }
                            else if (response.ServerMessage.Body.ToLower().Contains("voice-authorisation"))
                            {
                                var voiceAuthorisation = response.ServerMessage.DeserializeBody<WorldpayReferralMethodDTO>();
                                if (voiceAuthorisation != null && response.Operation != null)
                                {
                                    var referralResult = PaymentForm.AskReferral(voiceAuthorisation);
                                    sdkClient.ProcessReferral(response.Operation, new WorldpayPaymentReferralInput(correlationId, voiceAuthorisation.MerchantTransactionReference, referralResult.AuthorisationCode));
                                }
                            }
                            else if (response.ServerMessage.Body.ToLower().Contains("avs-confirmation"))
                            {
                                var avsConfirmation = response.ServerMessage.DeserializeBody<WorldpayAvsDto>();
                                if (avsConfirmation != null && response.Operation != null)
                                {
                                    var avsResult = PaymentForm.AskAvsMatch
                                    (
                                        cvvMatch: avsConfirmation.Data.AvsResults.IsCvvMatch == "matched" ? true : false,
                                        addressLineMatch: avsConfirmation.Data.AvsResults.IsAddressLineMatch == "matched" ? true : false,
                                        postalCodeMatch: avsConfirmation.Data.AvsResults.IsPostalCodeMatch == "matched" ? true : false
                                    );
                                    sdkClient.ProcessAvsMatch(response.Operation, new WorldpayPaymentAvsInput(correlationId, avsConfirmation.MerchantTransactionReference, avsResult));
                                }
                            }
                        }
                        else if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.PaymentResult)
                        {
                            var paymentResultDto = response.ServerMessage.DeserializeBody<WorldpayPaymentResultDTO>();
                            CreatePaymentResultRecord(paymentResult, paymentResultDto, worldpayPaymentOperationData.OriginalGatewayTransactionReference);
                        }
                        else if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.PaymentReceipt)
                        {
                            var paymentReceipt = response.ServerMessage.DeserializeBody<WorldpayPaymentReceiptDTO>();
                            var worldpayReceipt = AddReceiptToApplicationCollection(paymentReceipt, worldpayPaymentOperationData.PaymentType);
                            if (worldpayReceipt != null)
                            {
                                worldpayReceipt.AppendMerchantTransactionReference(paymentReceipt.MerchantTransactionReference);
                                Singleton.Instance.WorldpayPrint(Device, worldpayReceipt.DecodedContent, correlationId);
                            }
                        }
                        else if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.PaymentComplete)
                        {
                            paymentResult.InternalStatus = WorldpayInternalStatus.Completed;
                            paymentResult.Save();
                            var payment = paymentResult.Payments.FirstOrDefault();
                            if (payment != null)
                            {
                                var worldpayPaymentStatus = new WorldpayPaymentStatus(payment.Outcome, payment.Result);
                                if (worldpayPaymentStatus.IsSuccessful)
                                    successful = true;
                                else
                                    successful = false;
                            }
                            if (worldpayPaymentOperationData.PaymentRequestType == WorldpayPaymentRequestType.Payment)
                                PaymentForm.SetDescription("Transaction completed.");
                            else if (worldpayPaymentOperationData.PaymentRequestType == WorldpayPaymentRequestType.AccountVerification)
                                PaymentForm.SetDescription("Account Verification Request completed.");
                            PaymentForm.StopProgressCheck();
                            PaymentForm.SetExitButtonState(true);
                        }
                        else if (response.ServerMessage.GetDestination() == WorldpayMessageDestination.Error)
                        {
                            var error = response.ServerMessage.DeserializeBody<WorldpayErrorDTO>();
                            if (error != null)
                            {
                                var errorMessage = $@"Error Code: {error.Code}
Error Message: {error.Message}";
                                PaymentForm.SetText(errorMessage);
                                PaymentForm.SetExitButtonState(true);
                                PaymentForm.StopProgressCheck();
                                sdkClient.DisconnectFromAllConnections();
                            }
                            successful = false;
                        }
                    }
                }
            };

            try
            {
                DeviceUsageTracker.SetWorldpayDeviceUseState(Device, true);
                StartTimer();
                if (worldpayPaymentOperationData.PaymentRequestType == WorldpayPaymentRequestType.Payment)
                {
                    if (worldpayPaymentOperationData.PaymentInstrument != WorldpayPaymentInstrument.CardToken)
                    {
                        var input = new worldpay.Input.WorldpayPaymentInput(worldpayPaymentOperationData.PaymentType, worldpayPaymentOperationData.MerchantTransactionReference, worldpayPaymentOperationData.Amount, worldpayPaymentOperationData.PaymentInstrument, correlationId);
                        if (!string.IsNullOrEmpty(worldpayPaymentOperationData.OriginalGatewayTransactionReference))
                            input.SetOriginalGatewayTransactionReference(worldpayPaymentOperationData.OriginalGatewayTransactionReference);
                        sdkClient.ProcessPayment(input);
                    }
                    else
                    {
                        var input = new worldpay.Input.WorldpayPaymentInput(worldpayPaymentOperationData.PaymentType, worldpayPaymentOperationData.MerchantTransactionReference, worldpayPaymentOperationData.Amount, worldpayPaymentOperationData.PaymentInstrument, correlationId, tokenAndStatus.Token, tokenAndStatus.TokenPurpose);
                        if (!string.IsNullOrEmpty(worldpayPaymentOperationData.OriginalGatewayTransactionReference))
                            input.SetOriginalGatewayTransactionReference(worldpayPaymentOperationData.OriginalGatewayTransactionReference);
                        sdkClient.ProcessPayment(input);
                    }
                }
                else if (worldpayPaymentOperationData.PaymentRequestType == WorldpayPaymentRequestType.AccountVerification)
                    sdkClient.ProcessAccountVerification(new WorldpayAccountVerificationInput(worldpayPaymentOperationData.MerchantTransactionReference, correlationId));
                PaymentForm.Process();
            }
            finally
            {
                DeviceUsageTracker.SetWorldpayDeviceUseState(Device, false);
            }
            paymentResult.Successful = successful;
            paymentResult.Save();
            return successful;
        }

        private WorldpayPaymentResult CreatePaymentResult()
        {
            var paymentResult = new WorldpayPaymentResult(Device);
            return paymentResult;
        }

        private WorldpayReceipt AddReceiptToApplicationCollection(WorldpayPaymentReceiptDTO receipt, WorldpayPaymentType paymentType)
        {
            if (Singleton.Instance.WorldpayReceipts.ContainsKey((MerchantTransactionReference: receipt.MerchantTransactionReference, PaymentType: paymentType)))
            {
                var worldpayReceipt = new WorldpayReceipt(receipt.Type.ToLower() == "customer" ? WorldpayReceiptType.Customer : WorldpayReceiptType.Merchant, receipt.Content);
                Singleton.Instance.WorldpayReceipts[(MerchantTransactionReference: receipt.MerchantTransactionReference, PaymentType: paymentType)].Add(worldpayReceipt);
                return worldpayReceipt;
            }
            else
            {
                var worldpayReceipt = new WorldpayReceipt(receipt.Type.ToLower() == "customer" ? WorldpayReceiptType.Customer : WorldpayReceiptType.Merchant, receipt.Content);
                Singleton.Instance.WorldpayReceipts.Add((MerchantTransactionReference: receipt.MerchantTransactionReference, PaymentType: paymentType), new System.Collections.Generic.List<WorldpayReceipt>()
                {
                    worldpayReceipt
                });
                return worldpayReceipt;
            }
        }

        private void CreatePaymentResultRecord(WorldpayPaymentResult paymentResultPersistentRecord, WorldpayPaymentResultDTO paymentResultDto, string saleGatewayTransactionReference = null)
        {
            foreach (var paymentDto in paymentResultDto.Payments)
            {
                var payment = new WorldpayPayment();
                if (!string.IsNullOrEmpty(saleGatewayTransactionReference))
                    payment.SaleGatewayTransactionReference = saleGatewayTransactionReference;
                payment.PaymentResult = paymentResultPersistentRecord;
                payment.PaymentType = GetPaymentType(paymentDto);
                payment.TransactionDateTime = paymentDto.TransactionDateTime;
                payment.Outcome = GetPaymentOutcome(paymentDto);
                var merchantTransactionReference = paymentDto.MerchantTransactionReference;
                //Util.Inst.WorldpayPaymentOutcomes[merchantTransactionReference] = payment.Outcome;
                payment.Result = GetPaymentResult(paymentDto);
                payment.Merchant = GetMerchant(paymentDto.Merchant);
                if (paymentDto.Paypoint != null)
                {
                    payment.PaypointId = paymentDto.Paypoint?.PaypointId;
                    payment.TerminalId = paymentDto.Paypoint?.TerminalId;
                }
                if (paymentDto.Value != null)
                {
                    payment.Amount = GetAmountFromMinorCurrency(paymentDto.Value.Amount);
                    payment.Currency = paymentDto.Value.CurrencyCode;
                    payment.CashbackAmount = GetAmountFromMinorCurrency(paymentDto.Value.CashbackAmount);
                    payment.GratuityAmount = GetAmountFromMinorCurrency(paymentDto.Value.GratuityAmount);
                    payment.DonationAmount = GetAmountFromMinorCurrency(paymentDto.Value.DonationAmount);
                    payment.AmountInMinorCurrencyUnits = paymentDto.Value.Amount;
                    payment.CashbackAmountInMinorCurrencyUnits = paymentDto.Value.CashbackAmount;
                    payment.GratuityAmountInMinorCurrencyUnits = paymentDto.Value.GratuityAmount;
                    payment.DonationAmountInMinorCurrencyUnits = paymentDto.Value.DonationAmount;
                    if (paymentDto.Value.Dcc != null)
                    {
                        payment.ConvertedCurrency = paymentDto.Value.Dcc.ConvertedCurrencyCode;
                        payment.ConvertedAmount = GetAmountFromMinorCurrency(paymentDto.Value.Dcc.ConvertedAmount);
                        payment.ConvertedAmountInMinorCurrencyUnits = paymentDto.Value.Dcc.ConvertedAmount;
                        payment.ConversionRate = paymentDto.Value.Dcc.ConversionRate;
                    }
                }
                payment.MerchantTransactionReference = paymentDto.MerchantTransactionReference;
                payment.GatewayTransactionReference = paymentDto.GatewayTransactionReference;
                payment.TransactionSequenceNumber = paymentDto.EftSequenceNumber;
                payment.RetrievalReferenceNumber = paymentDto.RetrievalReferenceNumber;
                payment.ReceiptNumber = paymentDto.ReceiptNumber;
                payment.ReceiptRetentionReminder = paymentDto.ReceiptRetentionReminder;
                payment.ReceiptCustomDeclaration = paymentDto.ReceiptCustomerDeclaration;
                if (paymentDto.PaymentInstrument != null)
                {
                    payment.PaymentInstrumentType = GetPaymentInstrument(paymentDto.PaymentInstrument);
                    if (paymentDto.PaymentInstrument.Card != null)
                    {
                        payment.CardTokenId = paymentDto.PaymentInstrument.Card.TokenId;
                        payment.CardNumber = paymentDto.PaymentInstrument.Card.CardNumber;
                        if (paymentDto.PaymentInstrument.Card.ExpiryDate != null)
                        {
                            payment.CardExpiryDateMonth = paymentDto.PaymentInstrument.Card.ExpiryDate.Month;
                            payment.CardExpiryDateYear = paymentDto.PaymentInstrument.Card.ExpiryDate.Year;
                        }
                        payment.Track2Data = paymentDto.PaymentInstrument.Card.Track2Data;
                        payment.CardType = GetCardType(paymentDto.PaymentInstrument.Card);
                        payment.CardIssuerCode = paymentDto.PaymentInstrument.Card.IssuerCode;
                        payment.CardCountryCode = paymentDto.PaymentInstrument.Card.CountryCode;
                        payment.PANSequenceNumber = paymentDto.PaymentInstrument.Card.PanSequenceNumber;
                        payment.CardApplicationLabel = paymentDto.PaymentInstrument.Card.ApplicationLabel;
                        payment.CardApplicationIdentifier = paymentDto.PaymentInstrument.Card.ApplicationIdentifier;
                        if (!string.IsNullOrEmpty(paymentDto.PaymentInstrument.Card.ApplicationEffectiveDate))
                            payment.CardApplicationEffectiveDate = DateTime.Parse(paymentDto.PaymentInstrument.Card.ApplicationEffectiveDate);
                        payment.EntryMode = GetEntryMode(paymentDto.PaymentInstrument);
                        payment.CardVerificationMethod = GetVerificationMethod(paymentDto.PaymentInstrument);
                        payment.IsHandledOnline = paymentDto.PaymentInstrument.IsHandledOnline;
                        if (paymentDto.PaymentInstrument.Authorisation != null)
                        {
                            payment.AuthorisationCode = paymentDto.PaymentInstrument.Authorisation.AuthorisationCode;
                            payment.CvvResponseData = paymentDto.PaymentInstrument.Authorisation.CvvResponseData;
                        }

                        if (Singleton.Instance.WorldpayReceipts.ContainsKey((MerchantTransactionReference: payment.MerchantTransactionReference, PaymentType: payment.PaymentType)))
                        {
                            var receiptsList = Singleton.Instance.WorldpayReceipts[(MerchantTransactionReference: payment.MerchantTransactionReference, PaymentType: payment.PaymentType)];
                            if (receiptsList != null)
                            {
                                var customerReceipt = receiptsList.FirstOrDefault(f => f.Type == WorldpayReceiptType.Customer);
                                if (customerReceipt != null)
                                    payment.CustomerReceipt = customerReceipt.DecodedContent;
                                var merchantReceipt = receiptsList.FirstOrDefault(f => f.Type == WorldpayReceiptType.Merchant);
                                if (merchantReceipt != null)
                                    payment.MerchantReceipt = merchantReceipt.DecodedContent;
                                var signatureReceipt = receiptsList.FirstOrDefault(f => f.Type == WorldpayReceiptType.Signature);
                                if (signatureReceipt != null)
                                    payment.SignatureReceipt = signatureReceipt.DecodedContent;

                                Singleton.Instance.WorldpayReceipts.Remove((MerchantTransactionReference: payment.MerchantTransactionReference, PaymentType: payment.PaymentType));
                            }
                        }
                    }
                    payment.Save();
                    completedTransactions.Add(payment);
                }

                paymentResultPersistentRecord.Payments.Add(payment);
                paymentResultPersistentRecord.Save();
            }

            //objSpace.CommitChanges();
        }

        decimal GetAmountFromMinorCurrency(int amount) => Convert.ToDecimal(amount) / 100M;

        WorldpayMerchant GetMerchant(WorldpayPaymentMerchantDTO merchant)
        {
            if (merchant is null)
                return null;
            
            WorldpayMerchant MerchantFromDatabase = null;
            /*
                 * select all from LocalWorldpayMerchant in database where LocalWorldpayMerchant.MerchantId is merchant passed via parameters and assign to MerchantFromDatabase.
                 * MerchantFromDatabase = result from database;
                 */
            if (MerchantFromDatabase == null)
            {
                MerchantFromDatabase = new WorldpayMerchant()
                {
                    MerchantId = merchant.MerchantId,
                    Name = merchant.Name,
                    AddressLine1 = merchant.Address?.Line1,
                    AddressLine2 = merchant.Address?.Line2,
                    AddressLine3 = merchant.Address?.Line3,
                    City = merchant.Address?.City,
                    State = merchant.Address?.State,
                    PostalCode = merchant.Address?.PostalCode,
                    CountryCode = merchant.Address?.CountryCode
                };
            }
            return MerchantFromDatabase;
        }

        WorldpayPaymentType GetPaymentType(WorldpayPaymentDTO payment)
        {
            if (payment.PaymentType is null)
                return WorldpayPaymentType.Unknown;

            if (payment.PaymentType.ToLower().Equals("sale"))
                return WorldpayPaymentType.Sale;
            else if (payment.PaymentType.ToLower().Equals("refund"))
                return WorldpayPaymentType.Refund;
            else if (payment.PaymentType.ToLower().Equals("check-card"))
                return WorldpayPaymentType.CheckCard;
            else if (payment.PaymentType.ToLower().Equals("check-card-payment"))
                return WorldpayPaymentType.CheckCardPayment;
            else if (payment.PaymentType.ToLower().Equals("cancel"))
                return WorldpayPaymentType.Cancel;
            else
                return WorldpayPaymentType.Unknown;
        }

        WorldpayPaymentOutcome GetPaymentOutcome(WorldpayPaymentDTO payment)
        {
            if (payment.Outcome is null)
                return WorldpayPaymentOutcome.Unknown;

            if (payment.Outcome.ToLower().Equals("succeeded"))
                return WorldpayPaymentOutcome.Succeeded;
            else if (payment.Outcome.ToLower().Equals("refused"))
                return WorldpayPaymentOutcome.Refused;
            else if (payment.Outcome.ToLower().Equals("pending"))
                return WorldpayPaymentOutcome.Pending;
            else if (payment.Outcome.ToLower().Equals("failed"))
                return WorldpayPaymentOutcome.Failed;
            else
                return WorldpayPaymentOutcome.Unknown;
        }

        SharpAx.Worldpay.API.WorldpayPaymentResult GetPaymentResult(WorldpayPaymentDTO payment)
        {
            if (payment.Result is null)
                return SharpAx.Worldpay.API.WorldpayPaymentResult.Unknown;

            switch (payment.Result.ToLower())
            {
                case "authorised-online":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedOnline;
                case "authorised-offline":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedOffline;
                case "authorised-referral":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AuthorisedReferral;
                case "keyed-recovery-success":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.KeyedRecoverySuccess;
                case "succeeded":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Succeeded;
                case "refused-online":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOnline;
                case "refused-offline":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOffline;
                case "refused-online-capture-card":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedOnlineCaptureCard;
                case "refused-avs-required":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedAvsRequired;
                case "refused-do-not-reattempt":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RefusedDoNotReattempt;
                case "started":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Started;
                case "in-progress":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InProgress;
                case "auth-completed":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AuthCompleted;
                case "cancelled":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Cancelled;
                case "aborted":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Aborted;
                case "aborted-device-unavailable":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AbortedDeviceUnavailable;
                case "aborted-busy":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.AbortedBusy;
                case "rejected-card-number-not-matched":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardNumberNotMatched;
                case "rejected-card-expiry-date-not-matched":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardExpiryDateNotMatched;
                case "rejected-card-not-accepted":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.RejectedCardNotAccepted;
                case "invalid-transaction-state":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidTransactionState;
                case "invalid-operation":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidOperation;
                case "invalid-gateway-transactions-reference":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidGatewayTransactionsReference;
                case "invalid-merchant":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidMerchant;
                case "invalid-terminal":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidTerminal;
                case "invalid-merchant-status":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidMerchantStatus;
                case "invalid-card-number":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidCardNumber;
                case "invalid-expired-card":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidExpiredCard;
                case "invalid-issue-number":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidIssueNumber;
                case "invalid-card-expiry-date":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidCardExpiryDate;
                case "invalid-amount":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.InvalidAmount;
                case "denied-transaction":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.DeniedTransaction;
                case "denied-cashback":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.DeniedCashback;
                case "pre-valid-card":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.PreValidCard;
                case "failed":
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Failed;
                default:
                    return SharpAx.Worldpay.API.WorldpayPaymentResult.Unknown;
            }
        }

        WorldpayPaymentInstrument GetPaymentInstrument(WorldpayPaymentInstrumentDTO paymentInstrument)
        {
            if (paymentInstrument.Type is null)
                return WorldpayPaymentInstrument.Unknown;

            if (paymentInstrument.Type.ToLower().Equals("card/present"))
                return WorldpayPaymentInstrument.CardPresent;
            else if (paymentInstrument.Type.ToLower().Equals("card/keyed"))
                return WorldpayPaymentInstrument.CardKeyed;
            else if (paymentInstrument.Type.ToLower().Equals("card/not-present"))
                return WorldpayPaymentInstrument.CardNotPresent;
            else if (paymentInstrument.Type.ToLower().Equals("card/token"))
                return WorldpayPaymentInstrument.CardToken;
            else
                return WorldpayPaymentInstrument.Unknown;
        }

        WorldpayPaymentInstrumentCardType GetCardType(WorldpayPaymentCardDTO card)
        {
            if (card.Type is null)
                return WorldpayPaymentInstrumentCardType.Unknown;

            if (card.Type.ToLower().Equals("fuel"))
                return WorldpayPaymentInstrumentCardType.Fuel;
            else if (card.Type.ToLower().Equals("debit"))
                return WorldpayPaymentInstrumentCardType.Debit;
            else if (card.Type.ToLower().Equals("credit"))
                return WorldpayPaymentInstrumentCardType.Credit;
            else
                return WorldpayPaymentInstrumentCardType.Unknown;
        }

        WorldpayEntryMode GetEntryMode(WorldpayPaymentInstrumentDTO paymentInstrument)
        {
            if (paymentInstrument.PosEntryMode is null)
                return WorldpayEntryMode.Unknown;

            if (paymentInstrument.PosEntryMode.ToLower().Equals("keyed"))
                return WorldpayEntryMode.Keyed;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("magstripe"))
                return WorldpayEntryMode.Magstripe;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("integrated-circuit-chip"))
                return WorldpayEntryMode.IntegratedCircuitChip;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("contactless-chip"))
                return WorldpayEntryMode.ContactlessChip;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("contactless-magstripe"))
                return WorldpayEntryMode.ContactlessMagstripe;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("contactless-on-device"))
                return WorldpayEntryMode.ContactlessOnDevice;
            else if (paymentInstrument.PosEntryMode.ToLower().Equals("cardholder-not-present"))
                return WorldpayEntryMode.CardholderNotPresent;
            else
                return WorldpayEntryMode.Unknown;
        }

        WorldpayCardVerificationMethod GetVerificationMethod(WorldpayPaymentInstrumentDTO paymentInstrument)
        {
            if (paymentInstrument.CardVerificationMethod is null)
                return WorldpayCardVerificationMethod.Unknown;

            if (paymentInstrument.CardVerificationMethod.ToLower().Equals("signature"))
                return WorldpayCardVerificationMethod.Signature;
            else if (paymentInstrument.CardVerificationMethod.ToLower().Equals("pin-or-consumer-device"))
                return WorldpayCardVerificationMethod.PinOrConsumerDevice;
            else if (paymentInstrument.CardVerificationMethod.ToLower().Equals("pin-and-signature"))
                return WorldpayCardVerificationMethod.PinAndSignature;
            else if (paymentInstrument.CardVerificationMethod.ToLower().Equals("cardholder-not-present"))
                return WorldpayCardVerificationMethod.CardholderNotPresent;
            else if (paymentInstrument.CardVerificationMethod.ToLower().Equals("none"))
                return WorldpayCardVerificationMethod.None;
            else if (paymentInstrument.CardVerificationMethod.ToLower().Equals("unknown"))
                return WorldpayCardVerificationMethod.Unknown;
            else
                return WorldpayCardVerificationMethod.Unknown;
        }
    }

    public enum WorldpayTerminateReason
    {
        UserTerminated
    }

    public enum WorldpayTokenStatus
    {
        Valid,
        Expired,
        Empty,
        NotFound
    }



}
