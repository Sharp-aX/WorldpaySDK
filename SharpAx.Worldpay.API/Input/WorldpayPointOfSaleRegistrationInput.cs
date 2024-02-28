using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Input
{
    public class WorldpayPointOfSaleRegistrationInput : WorldpayInput
    {
        public WorldpayPointOfSaleRegistrationInput(string correlationId) : base(correlationId)
        {
        }

        public string PointOfSaleId { get; set; }
        public string PointOfSaleReference { get; set; }
        public string PointOfSaleActivationCode { get; set; }
    }
}
