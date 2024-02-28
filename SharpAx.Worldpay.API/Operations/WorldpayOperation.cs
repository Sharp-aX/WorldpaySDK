using System;
using System.Threading;
using System.Timers;
using WebSocketSharp;
using SharpAx.Worldpay.API.Messages;

namespace SharpAx.Worldpay.API.Operations
{
    public abstract class WorldpayOperation
    {
        public WorldpayOperation()
        {
            Properties.Instance.Client.Operations.Add(this);
        }

        internal WebSocket WebSocket { get; private set; }

        protected virtual void BaseProcess()
        {
            WebSocket = new WebSocket(Properties.Instance.BaseUrl);
            WebSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            WebSocket.OnMessage += WebSocket_OnMessage;
            WebSocket.OnError += WebSocket_OnError;
            WebSocket.Connect();
        }

        protected abstract void OperationProcess();

        internal void Process()
        {
            BaseProcess();
            OperationProcess();
        }

        protected virtual void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Properties.Instance.Client.RaiseReceiveWorldpayServerResponse(new WorldpayServerResponseEventArgs(new WorldpayMessageSerializer().Deserialize(e.Message), this));
            DisconnectAndCloseWebSocket();
        }

        protected virtual void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data, "worldpay");
            var message = new WorldpayMessageSerializer().Deserialize(e.Data);
            if (message.MessageType == StompMessageServerType.Error)
                DisconnectAndCloseWebSocket();
            else
            {
                var args = new WorldpayServerResponseEventArgs(message, this);
                Properties.Instance.Client.RaiseReceiveWorldpayServerResponse(args);
                if (message.GetDestination() == WorldpayMessageDestination.Error)
                    DisconnectAndCloseWebSocket();
            }
        }

        internal void DisconnectAndCloseWebSocket()
        {
            try
            {
                if (WebSocket != null && WebSocket.ReadyState == WebSocketState.Open && WebSocket.IsAlive)
                {
                    WebSocket.Send(new WorldpayDisconnectMessage().Serialize());
                    WebSocket.Close();
                }
            }
            catch
            {

            }
        }
    }
}
