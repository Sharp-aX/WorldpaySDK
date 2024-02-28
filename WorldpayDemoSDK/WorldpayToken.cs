using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WorldPayDemoSDK
{
    public class WorldpayToken
    {
        private const string worldpayTokenStatus_Valid = "Valid";
        private const string worldpayTokenStatus_NotFound = "No token is found for the customer.";
        private const string worldpayTokenStatus_Expired = "Card and token are expired.";
        private const string worldpayTokenStatus_Empty = "For unknown reason customer token is empty.";

        public WorldpayToken(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            (WorldpayTokenStatus TokenStatus, WorldpayAccountVerificationRecord TokenRecord) tokenAndStatus = customer.GetWorldpayTokenByCustomer();
            if (tokenAndStatus.TokenStatus == WorldpayTokenStatus.Valid)
            {
                TokenStatus = WorldpayTokenStatus.Valid;
                TokenStatusMessage = worldpayTokenStatus_Valid;
                Token = tokenAndStatus.TokenRecord.CardTokenId;
                TokenPurpose = tokenAndStatus.TokenRecord.Purpose;
            }
            else if (tokenAndStatus.TokenStatus == WorldpayTokenStatus.NotFound)
            {
                TokenStatus = WorldpayTokenStatus.NotFound;
                TokenStatusMessage = worldpayTokenStatus_NotFound;
                Token = null;
                TokenPurpose = WorldpayTokenPurpose.NotSet;
            }
            else if (tokenAndStatus.TokenStatus == WorldpayTokenStatus.Expired)
            {
                TokenStatus = WorldpayTokenStatus.Expired;
                TokenStatusMessage = worldpayTokenStatus_Expired;
                Token = null;
                TokenPurpose = WorldpayTokenPurpose.NotSet;
            }
            else if (tokenAndStatus.TokenStatus == WorldpayTokenStatus.Empty)
            {
                TokenStatus = WorldpayTokenStatus.Empty;
                TokenStatusMessage = worldpayTokenStatus_Empty;
                Token = null;
                TokenPurpose = WorldpayTokenPurpose.NotSet;
            }
        }

        public WorldpayTokenStatus TokenStatus { get; }
        public string TokenStatusMessage { get; }
        public string Token { get; }
        public WorldpayTokenPurpose TokenPurpose { get; }
    }


    
}
