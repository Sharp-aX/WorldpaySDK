using Newtonsoft.Json;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Messages.POS.Payment;
using SharpAx.Worldpay.API.Messages.Subscribe;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Messages.POS.AccountVerification;

namespace SharpAx.Worldpay.API.Operations
{
    internal class WorldpayAccountVerificationOperation : WorldpayOperation
    {
        private readonly WorldpayAccountVerificationInput input;

        public WorldpayAccountVerificationOperation(WorldpayAccountVerificationInput input)
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
            var payload = new WorldpayAccountVerificationPayload()
            {
                PaymentType = input.PaymentType,
                MerchantTransactionReference = input.MerchantTransactionReference,
                Instruction = new WorldpayAccountVerificationPayloadInstruction()
                {
                    Value = new WorldpayAccountVerificationPayloadValue()
                    {
                        Amount = 0
                    },
                    //Narrative = new WorldpayAccountVerificationPayloadNarrative()
                    //{
                    //    Line1 = "Line1" //TODO
                    //},
                    PaymentInstrument = new WorldpayAccountVerificationPayloadPaymentInstrument()
                    {
                        Type = "card/present",
                        IsHandledOnline = true,
                        TokenPurpose = "unscheduled", //TODO recurring
                    }
                }
            };
            var accountVerificationRequestMessage = new WorldpayAccountVerificationRequestMessage(input.CorrelationId, JsonConvert.SerializeObject(payload));
            WebSocket.Send(accountVerificationRequestMessage.Serialize());
            Thread.Sleep(1000);
        }
    }
}
