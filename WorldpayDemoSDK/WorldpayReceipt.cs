using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldPayDemoSDK
{
    public class WorldpayReceipt
    {
        public WorldpayReceipt(WorldpayReceiptType type, string content)
        {
            DecodedContent = GetDecodedReceipt(content);
            if (type == WorldpayReceiptType.Merchant && DecodedContent.ToUpper().Contains("SIGN HERE"))
                Type = WorldpayReceiptType.Signature;
            else
                Type = type;
        }

        public WorldpayReceiptType Type { get; }
        public string DecodedContent { get; private set; }

        private string GetDecodedReceipt(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            var data = Convert.FromBase64String(content);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        public void AppendMerchantTransactionReference(string merchantTransactionReference)
        {
            if (string.IsNullOrWhiteSpace(DecodedContent))
                return;
            if (string.IsNullOrWhiteSpace(merchantTransactionReference))
                return;
            DecodedContent = $@"{DecodedContent}
MTR: {merchantTransactionReference}";
        }
    }

}
