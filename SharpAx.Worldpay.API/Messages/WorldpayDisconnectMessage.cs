using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages
{
    internal class WorldpayDisconnectMessage : WorldpayMessage
    {
        public WorldpayDisconnectMessage() : base(StompMessageClientType.Disconnect)
        {
        }
    }
}
