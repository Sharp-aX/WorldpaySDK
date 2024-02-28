using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API
{
    internal class Properties
    {
        private static readonly Lazy<Properties> lazy =
    new Lazy<Properties>(() => new Properties());

        public static Properties Instance { get { return lazy.Value; } }

        private Properties()
        {
        }

        public string PublisherId { get; internal set; }
        internal string BaseUrl { get; set; }
        internal string PaypointName { get; set; }
        internal string License { get; set; }
        internal WorldpayClient Client { get; set; }
    }
}
