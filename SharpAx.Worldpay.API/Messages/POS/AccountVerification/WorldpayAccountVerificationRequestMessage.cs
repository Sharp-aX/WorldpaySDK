using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages.POS.AccountVerification
{
    public class WorldpayAccountVerificationRequestMessage : WorldpayMessage
    {
        public WorldpayAccountVerificationRequestMessage(string correlationId, string body) : base(StompMessageClientType.Send, correlationId, body)
        {
            this["destination"] = "/v1/payment";
        }
    }
}
