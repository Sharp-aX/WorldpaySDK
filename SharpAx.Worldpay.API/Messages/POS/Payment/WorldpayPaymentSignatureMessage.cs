using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages.POS.Payment
{
    public class WorldpayPaymentSignatureMessage : WorldpayMessage
    {
        public WorldpayPaymentSignatureMessage(string correlationId, string body) : base(StompMessageClientType.Send, correlationId, body)
        {
            this["destination"] = "/v1/payment/action";
        }
    }
}
