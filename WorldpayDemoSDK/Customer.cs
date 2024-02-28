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
    public class Customer : DatabaseClass
    {
        private List<WorldpayAccountVerificationRecord> _WorldpayAccountVerifications = new List<WorldpayAccountVerificationRecord>();
        public List<WorldpayAccountVerificationRecord> WorldpayAccountVerifications
        {
            get { return _WorldpayAccountVerifications; }
            set { _WorldpayAccountVerifications = value; }
        }

        public (WorldpayTokenStatus TokenStatus, WorldpayAccountVerificationRecord TokenRecord) GetWorldpayTokenByCustomer()
        {
            var token = WorldpayAccountVerifications.OrderByDescending(d => d.TokenAcquiredDateTime).FirstOrDefault();
            if (token is null)
                return (TokenStatus: WorldpayTokenStatus.NotFound, null);
            if (token.CardExpiryDateMonth < DateTime.Now.Month && token.CardExpiryDateYear <= DateTime.Now.Year)
                return (TokenStatus: WorldpayTokenStatus.Expired, null);
            if (string.IsNullOrEmpty(token.CardTokenId))
                return (TokenStatus: WorldpayTokenStatus.Empty, null);
            else
                return (TokenStatus: WorldpayTokenStatus.Valid, token);
        }
    }


    public class WorldpayAccountVerificationRecord :DatabaseClass
    {
        public WorldpayAccountVerificationRecord()
        {
        }

        private Customer _Customer;
        public Customer Customer
        {
            get { return _Customer; }
            set { SetPropertyValue(nameof(Customer), ref _Customer, value); }
        }

        
        private WorldpayPayment _AccountVerificationPayment;
        public WorldpayPayment AccountVerificationPayment
        {
            get { return _AccountVerificationPayment; }
            set { SetPropertyValue(nameof(AccountVerificationPayment), ref _AccountVerificationPayment, value); }
        }
        

        private DateTime _TokenAcquiredDateTime;
        public DateTime TokenAcquiredDateTime
        {
            get { return _TokenAcquiredDateTime; }
            set { SetPropertyValue<DateTime>(nameof(TokenAcquiredDateTime), ref _TokenAcquiredDateTime, value); }
        }

        private string _CardTokenId;
        [PasswordPropertyText(true)]
        public string CardTokenId
        {
            get { return _CardTokenId; }
            set { SetPropertyValue<string>(nameof(CardTokenId), ref _CardTokenId, value); }
        }

        private string _CardNumber;
        public string CardNumber
        {
            get { return _CardNumber; }
            set { SetPropertyValue<string>(nameof(CardNumber), ref _CardNumber, value); }
        }

        private int _CardExpiryDateMonth;
        public int CardExpiryDateMonth
        {
            get { return _CardExpiryDateMonth; }
            set { SetPropertyValue<int>(nameof(CardExpiryDateMonth), ref _CardExpiryDateMonth, value); }
        }

        private int _CardExpiryDateYear;
        public int CardExpiryDateYear
        {
            get { return _CardExpiryDateYear; }
            set { SetPropertyValue<int>(nameof(CardExpiryDateYear), ref _CardExpiryDateYear, value); }
        }

        private WorldpayTokenPurpose _Purpose;
        public WorldpayTokenPurpose Purpose
        {
            get { return _Purpose; }
            set { SetPropertyValue<WorldpayTokenPurpose>(nameof(Purpose), ref _Purpose, value); }
        }

    }


}
