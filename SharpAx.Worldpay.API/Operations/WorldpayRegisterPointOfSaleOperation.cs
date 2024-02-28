using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using SharpAx.Worldpay.API.DTO;
using SharpAx.Worldpay.API.Input;
using SharpAx.Worldpay.API.Messages.Subscribe;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Payloads;
using Newtonsoft.Json;
using SharpAx.Worldpay.API.Messages.POS.Registration;
using System.Threading;
using System.Collections.Concurrent;

namespace SharpAx.Worldpay.API.Operations
{
    internal class WorldpayRegisterPointOfSaleOperation : WorldpayOperation
    {
        private readonly WorldpayPointOfSaleRegistrationInput input;

        public WorldpayRegisterPointOfSaleOperation(WorldpayPointOfSaleRegistrationInput input) : base()
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));
            this.input = input;
        }

        protected override void OperationProcess()
        {
            var connectMessage = new WorldpayConnectMessage(license: string.Empty);
            WebSocket.Send(connectMessage.Serialize());
            Thread.Sleep(1000);
            var errorSubscriptionMessage = new WorldpayErrorSubscribeMessage("0", input.CorrelationId);
            WebSocket.Send(errorSubscriptionMessage.Serialize());
            var pointOfSaleRegistrationSubscriptionMessage = new WorldpayPointOfSalesRegistrationSubscribeMessage("1", input.CorrelationId);
            WebSocket.Send(pointOfSaleRegistrationSubscriptionMessage.Serialize());
            var pointOfSaleRegistrationPayload = new WorldpayRegistrationPayload()
            {
                PointOfSaleId = input.PointOfSaleId,
                PointOfSaleReference = input.PointOfSaleReference,
                PointOfSaleActivationCode = input.PointOfSaleActivationCode
            };
            var posRegistrationRequestMessage = new WorldpayPointOfSalesRegistrationRequestMessage(input.CorrelationId, JsonConvert.SerializeObject(pointOfSaleRegistrationPayload));
            WebSocket.Send(posRegistrationRequestMessage.Serialize());
        }
    }
}
