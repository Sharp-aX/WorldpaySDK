using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Messages.POS.Registration;
using SharpAx.Worldpay.API.Messages.Subscribe;
using SharpAx.Worldpay.API.Messages;
using SharpAx.Worldpay.API.Payloads;
using SharpAx.Worldpay.API.Input;

namespace SharpAx.Worldpay.API.Operations
{
    internal class WorldpayPointOfSaleRegistrationRefreshOperation : WorldpayOperation
    {
        private readonly WorldpayPointOfSaleRegistrationRefreshInput input;

        public WorldpayPointOfSaleRegistrationRefreshOperation(WorldpayPointOfSaleRegistrationRefreshInput input)
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
            var pointOfSaleRegistrationRefreshSubscriptionMessage = new WorldpayPointOfSalesRegistrationRefreshSubscribeMessage("1", input.CorrelationId);
            WebSocket.Send(pointOfSaleRegistrationRefreshSubscriptionMessage.Serialize());
            var posRegistrationRequestMessage = new WorldpayPointOfSalesRegistrationRefreshRequestMessage(input.CorrelationId, JsonConvert.SerializeObject(new WorldpayRegistrationRefreshPayload()));
            WebSocket.Send(posRegistrationRequestMessage.Serialize());
        }
    }
}
