using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Messages
{
    public abstract class WorldpayMessage
    {
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>();

        private WorldpayMessage(string correlationId)
        {
            //this["heart-beat"] = "0,1000";
            if (!string.IsNullOrEmpty(correlationId))
            {
                CorrelationId = correlationId;
                MessageId = Guid.NewGuid().ToString();
                PublisherId = Properties.Instance.PublisherId;

                this["x-wp-correlation-id"] = correlationId.ToString();
                this["x-wp-message-id"] = MessageId.ToString();
                this["x-wp-publisher-id"] = PublisherId.ToString();
            }
        }

        protected WorldpayMessage(StompMessageClientType clientMessageType)
        {
            ClientMessageType = clientMessageType;
        }

        public WorldpayMessage(StompMessageClientType clientMessageType, string correlationId) : this(correlationId)
        {
            ClientMessageType = clientMessageType;
        }

        public WorldpayMessage(StompMessageClientType clientMessageType, string correlationId, string body) : this(clientMessageType, correlationId)
        {
            Body = body;
        }

        public StompMessageClientType ClientMessageType { get; }
        public string CorrelationId { get; }
        public string MessageId { get; }
        public string PublisherId { get; }
        public string Body { get; }
        public Dictionary<string, string> Headers => headers;

        public string Serialize()
        {
            var serializer = new WorldpayMessageSerializer();
            return serializer.Serialize(this);
        }

        public string this[string header]
        {
            get { return headers.ContainsKey(header) ? headers[header] : string.Empty; }
            set { headers[header] = value; }
        }
    }

    public enum StompMessageClientType
    {
        Connect,
        Disconnect,
        Subscribe,
        Unsubscribe,
        Send,
        Get
    }
}
