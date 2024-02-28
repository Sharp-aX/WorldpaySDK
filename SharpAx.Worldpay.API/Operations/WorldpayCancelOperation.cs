using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Messages.POS.Payment;
using SharpAx.Worldpay.API.Messages.Subscribe;
using SharpAx.Worldpay.API.Payloads;

namespace SharpAx.Worldpay.API.Operations
{
    public class WorldpayCancelOperation : WorldpayOperation
    {
        private readonly WorldpayPaymentCancelInput input;

        public WorldpayCancelOperation(WorldpayPaymentCancelInput input) : base()
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));
            this.input = input;
        }

        public WorldpayPaymentCancelInput Input { get; }

        protected override void OperationProcess()
        {
            var connectMessage = new WorldpayConnectMessage(Properties.Instance.License);
            WebSocket.Send(connectMessage.Serialize());
            Thread.Sleep(1000);
            var errorSubscriptionMessage = new WorldpayErrorSubscribeMessage("0", input.CorrelationId);
            WebSocket.Send(errorSubscriptionMessage.Serialize());
            var payload = new WorldpayPaymentCancelPayload()
            {
                MerchantTransactionReference = input.MerchantTransactionReference,
                GatewayTransactionReference = input.GatewayTransactionReference,
                PaymentInstrument = new PaymentInstrument()
                {
                    Type = "card",
                    CardNumber = input.CardNumber,
                    CardExpiryDate = new CardExpiryDate()
                    {
                        Month = input.CardExpiryDateMonth,
                        Year = input.CardExpiryDateYear
                    }
                }
            };
            var paymentCancelRequestMessage = new WorldpayPaymentCancelRequestMessage(input.CorrelationId, JsonConvert.SerializeObject(payload));
            WebSocket.Send(paymentCancelRequestMessage.Serialize());
            Thread.Sleep(1000);
        }
    }
}
