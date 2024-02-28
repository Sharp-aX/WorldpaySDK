using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages.POS.Registration
{
    public class WorldpayPointOfSalesRegistrationRefreshRequestMessage : WorldpayMessage
    {
        public WorldpayPointOfSalesRegistrationRefreshRequestMessage(string correlationId, string body) : base(StompMessageClientType.Send, correlationId, body)
        {
            this["destination"] = "/v1/pos/registration/refresh";
        }
    }
}
