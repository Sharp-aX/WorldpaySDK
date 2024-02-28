using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages.POS.Payment
{
    public class WorldpayPaymentReferralMessage : WorldpayMessage
    {
        public WorldpayPaymentReferralMessage(string correlationId, string body) : base(StompMessageClientType.Send, correlationId, body)
        {
            this["destination"] = "/v1/payment/action";
        }
    }
}
