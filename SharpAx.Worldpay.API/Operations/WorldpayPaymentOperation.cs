using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using SharpAx.Worldpay.API.Messages.Subscribe;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Payloads;
using SharpAx.Worldpay.API.Messages.POS.Payment;
using Newtonsoft.Json;
using System.Threading;
using SharpAx.Worldpay.API.DTO;

namespace SharpAx.Worldpay.API.Operations
{
    internal class WorldpayPaymentOperation : WorldpayOperation
    {
        private readonly WorldpayPaymentInput input;

        public WorldpayPaymentOperation(WorldpayPaymentInput input) : base()
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));
            this.input = input;
        }

        protected override void OperationProcess()
        {
            var connectMessage = new WorldpayConnectMessage(Properties.Instance.License);
            WebSocket.Send(connectMessage.Serialize());
            Thread.Sleep(1000);
            var errorSubscriptionMessage = new WorldpayErrorSubscribeMessage("0", input.CorrelationId);
            WebSocket.Send(errorSubscriptionMessage.Serialize());
            var paymentNotificationSubscriptionMessage = new WorldpayPaymentNotificationSubscribeMessage("1", input.CorrelationId);
            WebSocket.Send(paymentNotificationSubscriptionMessage.Serialize());
            var paymentReceiptSubscriptionMessage = new WorldpayPaymentReceiptSubscribeMessage("2", input.CorrelationId);
            WebSocket.Send(paymentReceiptSubscriptionMessage.Serialize());
            var paymentActionSubscriptionMessage = new WorldpayPaymentActionSubscribeMessage("3", input.CorrelationId);
            WebSocket.Send(paymentActionSubscriptionMessage.Serialize());
            var paymentResultSubscriptionMessage = new WorldpayPaymentResultSubscribeMessage("4", input.CorrelationId);
            WebSocket.Send(paymentResultSubscriptionMessage.Serialize());
            var paymentCompleteSubscriptionMessage = new WorldpayPaymentCompleteSubscribeMessage("5", input.CorrelationId);
            WebSocket.Send(paymentCompleteSubscriptionMessage.Serialize());
            var payload = new WorldpayPaymentPayload();
            payload.PaymentType = input.PaymentType;
            payload.MerchantTransactionReference = input.MerchantTransactionReference;
            payload.Instruction = new WorldpayPaymentPayloadInstruction()
            {
                Value = new WorldpayPaymentPayloadValue()
                {
                    Amount = input.Amount
                },
            };
            if (input.PaymentInstrumentType != "card/token")
            {
                payload.Instruction.PaymentInstrument = new WorldpayPaymentPayloadPaymentInstrument()
                {
                    Type = input.PaymentInstrumentType,
                    IsHandledOnline = true
                };
            }
            else if (input.PaymentType == "sale" && input.PaymentInstrumentType == "card/token")
            {
                payload.Instruction.PaymentInstrument = new WorldpayPaymentPayloadPaymentInstrument()
                {
                    Type = input.PaymentInstrumentType,
                    IsHandledOnline = null,
                    Token = string.IsNullOrEmpty(input.Token) ? null : input.Token,
                    TokenPurpose = string.IsNullOrEmpty(input.TokenPurpose) ? null : input.TokenPurpose
                };
            }
            else if (input.PaymentType == "refund" && input.PaymentInstrumentType == "card/token")
            {
                payload.Instruction.OriginalGatewayTransactionReference = input.OriginalGatewayTransactionReference;
                payload.Instruction.PaymentInstrument = new WorldpayPaymentPayloadPaymentInstrument()
                {
                    Type = input.PaymentInstrumentType,
                    IsHandledOnline = null,
                    Token = string.IsNullOrEmpty(input.Token) ? null : input.Token,
                };
            }
            var paymentRequestMessage = new WorldpayPaymentRequestMessage(input.CorrelationId, JsonConvert.SerializeObject(payload));
            WebSocket.Send(paymentRequestMessage.Serialize());
            Thread.Sleep(1000);
        }

        public void SignatureVerification(WorldpayPaymentSignatureInput worldpayPaymentSignatureInput)
        {
            if (worldpayPaymentSignatureInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentSignatureInput));
            var signatureDto = new WorldpayPaymentSignatureDTO()
            {
                MerchantTransactionReference = worldpayPaymentSignatureInput.MerchantTransactionReference,
                Data = new WorldpayPaymentSignatureDataDTO()
                {
                    Action = "signature-verification",
                    IsSignatureVerified = worldpayPaymentSignatureInput.SignatureVerified
                }
            };
            if (WebSocket.IsAlive && WebSocket.ReadyState == WebSocketState.Open)
            {
                var paymentSignatureMessage = new WorldpayPaymentSignatureMessage(worldpayPaymentSignatureInput.CorrelationId, JsonConvert.SerializeObject(signatureDto));
                WebSocket.Send(paymentSignatureMessage.Serialize());
                Thread.Sleep(1000);
            }
        }

        public void ReferralVerification(WorldpayPaymentReferralInput worldpayPaymentReferralInput)
        {
            if (worldpayPaymentReferralInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentReferralInput));
            var verificationDto = new WorldpayReferralMethodVerificationDTO()
            {
                MerchantTransactionReference = worldpayPaymentReferralInput.MerchantTransactionReference,
                Data = new WorldpayReferralMethodVerificationDataDTO()
                {
                    Action = "voice-authorisation",
                    AuthorisationCode = string.IsNullOrEmpty(worldpayPaymentReferralInput.AuthorisationCode) ? null : worldpayPaymentReferralInput.AuthorisationCode
                }
            };
            if (WebSocket.IsAlive && WebSocket.ReadyState == WebSocketState.Open)
            {
                var paymentReferralMessage = new WorldpayPaymentReferralMessage(worldpayPaymentReferralInput.CorrelationId, JsonConvert.SerializeObject(verificationDto));
                WebSocket.Send(paymentReferralMessage.Serialize());
                Thread.Sleep(1000);
            }
        }

        public void AvsMatch(WorldpayPaymentAvsInput worldpayPaymentAvsInput)
        {
            if (worldpayPaymentAvsInput is null)
                throw new ArgumentNullException(nameof(worldpayPaymentAvsInput));
            var avsMatch = new WorldpayAvsVerificationDTO()
            {
                MerchantTransactionReference = worldpayPaymentAvsInput.MerchantTransactionReference,
                Data = new WorldpayAvsVerificationDataDTO()
                {
                    Action = "avs-confirmation",
                    IsAvsConfirmed = worldpayPaymentAvsInput.AvsConfirmed
                }
            };
            if (WebSocket.IsAlive && WebSocket.ReadyState == WebSocketState.Open)
            {
                var paymentAvsMatchMessage = new WorldpayPaymentAvsMatchMessage(worldpayPaymentAvsInput.CorrelationId, JsonConvert.SerializeObject(avsMatch));
                WebSocket.Send(paymentAvsMatchMessage.Serialize());
                Thread.Sleep(1000);
            }
        }
    }
}
