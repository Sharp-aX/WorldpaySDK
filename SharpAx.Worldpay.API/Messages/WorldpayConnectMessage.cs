using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages
{
    public class WorldpayConnectMessage : WorldpayMessage
    {
        public WorldpayConnectMessage(string license) : base(StompMessageClientType.Connect, string.Empty, string.Empty)
        {
            if (!string.IsNullOrEmpty(license))
                this["x-wp-authorization"] = license;
            this["accept-version"] = "1.1";
            this["host"] = $"/ipc-app/payment/{Properties.Instance.PaypointName}";
        }
    }
}
