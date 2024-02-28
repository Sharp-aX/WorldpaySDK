using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public abstract class WorldpayInput
    {
        public WorldpayInput(string correlationId)
        {
            if (string.IsNullOrEmpty(correlationId))
                throw new ArgumentNullException(nameof(correlationId));
            CorrelationId = correlationId;
        }

        public string CorrelationId { get; }
    }
}
