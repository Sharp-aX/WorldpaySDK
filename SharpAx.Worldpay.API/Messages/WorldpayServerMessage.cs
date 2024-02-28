using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages
{
    public class WorldpayServerMessage
    {
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>();

        public WorldpayServerMessage(StompMessageServerType messageType, Dictionary<string, string> headers, string body)
        {
            MessageType = messageType;
            this.headers = headers;
            Body = body;
        }

        public StompMessageServerType MessageType { get; }
        public Dictionary<string, string> Headers => headers;
        public string Body { get; }

        public bool DoesCorrelationIdMatch(string correlationId)
        {
            if (headers != null && headers.ContainsKey("x-wp-correlation-id"))
            {
                if (headers["x-wp-correlation-id"].ToLower() == correlationId.ToLower())
                    return true;
                else
                    return false;
            }
            return true;
        }

        public string this[string header]
        {
            get { return headers.ContainsKey(header) ? headers[header] : string.Empty; }
            set { headers[header] = value; }
        }

        public T DeserializeBody<T>()
        {
            if (string.IsNullOrEmpty(Body))
                return default;
            else
                return JsonConvert.DeserializeObject<T>(Body);
        }

        public WorldpayMessageDestination GetDestination()
        {
            if (MessageType == StompMessageServerType.Message)
            {
                var destination = headers["destination"];
                if (!string.IsNullOrEmpty(destination))
                {
                    if (destination.Equals("/client/v1/error"))
                        return WorldpayMessageDestination.Error;
                    else if (destination.Equals("/client/v1/pos/registration"))
                        return WorldpayMessageDestination.PosRegistration;
                    else if (destination.Equals("/client/v1/payment/notification"))
                        return WorldpayMessageDestination.PaymentNotification;
                    else if (destination.Equals("/client/v1/payment/receipt"))
                        return WorldpayMessageDestination.PaymentReceipt;
                    else if (destination.Equals("/client/v1/payment/result"))
                        return WorldpayMessageDestination.PaymentResult;
                    else if (destination.Equals("/client/v1/payment/complete"))
                        return WorldpayMessageDestination.PaymentComplete;
                    else if (destination.Equals("/client/v1/payment/action"))
                        return WorldpayMessageDestination.PaymentAction;
                    else if (destination.Equals("/client/v1/pos/registration/refresh"))
                        return WorldpayMessageDestination.PosRegistrationRefresh;
                }
            }
            return WorldpayMessageDestination.Unknown;
        }
    }

    public enum StompMessageServerType
    {
        Connected,
        Message,
        Error,
        Unknown
    }
}
