﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages.Subscribe
{
    public class WorldpayPaymentNotificationSubscribeMessage : WorldpayMessage
    {
        public WorldpayPaymentNotificationSubscribeMessage(string id, string correlationId) : base(StompMessageClientType.Subscribe, correlationId)
        {
            this["id"] = id;
            this["destination"] = "/client/v1/payment/notification";
        }
    }
}
