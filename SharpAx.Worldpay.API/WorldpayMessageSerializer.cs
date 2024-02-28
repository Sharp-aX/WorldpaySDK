using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAx.Worldpay.API.Messages;

namespace SharpAx.Worldpay.API
{
    internal class WorldpayMessageSerializer
    {
        /// <summary>
        ///   Serializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A serialized version of the given <see cref="WorldpayMessage"/></returns>
        public string Serialize(WorldpayMessage message)
        {
            var buffer = new StringBuilder();

            buffer.Append(message.ClientMessageType.ToString().ToUpper() + "\n");

            if (message.Headers != null)
            {
                foreach (var header in message.Headers)
                {
                    buffer.Append(header.Key + ":" + header.Value + "\n");
                }
            }

            buffer.Append("\n");
            buffer.Append(message.Body);
            buffer.Append('\0');

            return buffer.ToString();
        }

        /// <summary>
        /// Deserializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="StompMessage"/> instance</returns>
        public WorldpayServerMessage Deserialize(string message)
        {
            var reader = new StringReader(message);

            var command = reader.ReadLine();

            var headers = new Dictionary<string, string>();

            var header = reader.ReadLine();
            while (!string.IsNullOrEmpty(header))
            {
                var split = header.Split(':');
                if (split.Length == 2) headers[split[0].Trim()] = split[1].Trim();
                header = reader.ReadLine() ?? string.Empty;
            }

            var body = reader.ReadToEnd() ?? string.Empty;
            body = body.TrimEnd('\r', '\n', '\0');

            if (command.ToLower().Equals("connected"))
                return new WorldpayServerMessage(StompMessageServerType.Connected, headers, body);
            else if (command.ToLower().Equals("message"))
                return new WorldpayServerMessage(StompMessageServerType.Message, headers, body);
            else if (command.ToLower().Equals("message"))
                return new WorldpayServerMessage(StompMessageServerType.Error, headers, body);
            else
                return new WorldpayServerMessage(StompMessageServerType.Unknown, headers, body);
        }
    }
}
