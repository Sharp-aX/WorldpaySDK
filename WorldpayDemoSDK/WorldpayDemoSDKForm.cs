using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace WorldPayDemoSDK
{
    public partial class WorldpayDemoSDKForm : Form
    {
        public WorldpayDemoSDKForm()
        {
            InitializeComponent();

            cbxEnvironment.DataSource = Enum.GetValues(typeof(WorldpayEnvironment));
            cbxPaymentType.DataSource = Enum.GetValues(typeof(WorldpayPaymentType));
            cbxPaymentInstrument.DataSource = Enum.GetValues(typeof(WorldpayPaymentInstrument));
            cbxPaymentInstrument.SelectedItem = WorldpayPaymentInstrument.CardPresent;

            dtpIssuedOnUTC.Value = DateTime.Now.AddMonths(-1);
            dtpExpiryOnUTC.Value = DateTime.Now.AddMonths(1);
            dtpIssuedOn.Value = DateTime.Now.AddMonths(-1);
            dtpExpiryOn.Value = DateTime.Now.AddMonths(1);


            tbxPaypointName.Text = "Test-Sharp-aX";
            tbxDeviceId.Text = "806440376-VIPA";
            tbxTID.Text = "23773330";
            tbxEmid.Text = "000000000033641";
            tbxLicenseId.Text = "5UmtzB";
            tbxLicenseActivationCode.Text = "7Z7LRlrz";
            tbxLicenseReference.Text = "saxMantasT1";
            tbxLicenseText.Text = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJwZHBvc3JlZiI6InNheE1hbnRhc1QxIiwiYXVkIjoiYWNjZXNzLndvcmxkcGF5LmNvbS90b3RhbC9wb3MiLCJzdWIiOiIxNDE1NDY2NzciLCJwZHBvc2lkIjoiNVVtdHpCIiwiaXNzIjoiaXBzLWRzLXdvcmxkcGF5IiwicGRtIjpbIjAwMDAwMDAwMDAzMzY0MSJdLCJleHAiOjE3MjgwNDU1NDgsImlhdCI6MTY5NjQyMzE0OCwianRpIjoiZjg4ZDRjNjMtZjM4OS00ZDk1LWFhMTktNTkzOGQ1NjBmODhjIn0.zvyPcPR16YOEqCZ1RfSHq7xJ1zEFb2sluJoH0WxUqixsUX5hCi3GmOTno2GOggJWeArp76UF5OxiJmFdP_w_mLRI0FuDzVmh2btb28hBICyQ5aCmddUJPK3b8bFulvQr02ppCPww30LG11uunkokpIXVKTRTM-T46MNRBnROkVIFcLG6FLjf7HGL5vYR5LL4v5kc-rQTLgDHAgn2Zlzw2Y4gKXwsBPHZQBoET2PAaeScjoZiCHXDLewxTK1SmnDNrmlhxUhVN1irIDe5a4uCObwyHCcaC7yNP8-dWxQWnbL9ev5-2A8jNnE_oRJ9CAOHGsvXBqwuQ-WJ9zKJbW4Aqg";
            
            tbxToken.Text = "99679999032037330010";

        }

        private WorldpayDevice ReadDeviceFromForm()
        {
            WorldpayDevice device = new WorldpayDevice();
            device.PaypointName = tbxPaypointName.Text;
            device.DeviceId = tbxDeviceId.Text;
            device.TID = tbxTID.Text;
            device.Emid = tbxEmid.Text;
            device.LicenseId = tbxLicenseId.Text;
            device.LicenseActivationCode = tbxLicenseActivationCode.Text;
            device.LicenseReference = tbxLicenseReference.Text;
            device.IssuedOnUTC = dtpIssuedOnUTC.Value;
            device.ExpiryOnUTC = dtpExpiryOnUTC.Value;
            device.IssuedOn = dtpIssuedOn.Value;
            device.ExpiryOn = dtpExpiryOn.Value;
            device.LicenseText = tbxLicenseText.Text;
            return device;
        }

        private void DeviceToForm(WorldpayDevice device)
        {
            tbxPaypointName.Text = device.PaypointName;
            tbxDeviceId.Text = device.DeviceId;
            tbxTID.Text = device.TID;
            tbxEmid.Text = device.Emid;
            tbxLicenseId.Text = device.LicenseId;
            tbxLicenseActivationCode.Text = device.LicenseActivationCode;
            tbxLicenseReference.Text = device.LicenseReference;
            tbxLicenseText.Text = device.LicenseText;

            dtpIssuedOnUTC.Value = device.IssuedOnUTC;
            dtpExpiryOnUTC.Value = device.ExpiryOnUTC;
            dtpIssuedOn.Value = device.IssuedOn;
            dtpExpiryOn.Value = device.ExpiryOn;
        }

        
        private Customer ReadCustomerFromForm()
        {
            Customer Customer = new Customer();
            WorldpayAccountVerificationRecord avr = new WorldpayAccountVerificationRecord();
            avr.Customer = Customer;
            avr.TokenAcquiredDateTime = DateTime.Now.AddDays(-7);
            avr.CardTokenId = tbxToken.Text;
            avr.CardNumber = "679999XXXXXXXXX0010";
            avr.CardExpiryDateMonth = 12;
            avr.CardExpiryDateYear = 25;
            avr.Purpose = WorldpayTokenPurpose.Unscheduled;

            Customer.WorldpayAccountVerifications.Add(avr);

            return Customer;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

            WorldpayDevice device = ReadDeviceFromForm();
            var client = new SharpAx.Worldpay.API.WorldpayClient((WorldpayEnvironment)cbxEnvironment.SelectedItem, device.PaypointName, Guid.NewGuid().ToString());
            var correlationId = Guid.NewGuid().ToString();
            client.ReceiveWorldpayServerResponse += (response) =>
            {
                if (response.ServerMessage != null && response.ServerMessage.DoesCorrelationIdMatch(correlationId))
                {
                    if (response.ServerMessage.MessageType == SharpAx.Worldpay.API.Messages.StompMessageServerType.Error)
                    {

                    }
                    else if (response.ServerMessage.MessageType == SharpAx.Worldpay.API.Messages.StompMessageServerType.Message)
                    {
                        if (response.ServerMessage.GetDestination() == SharpAx.Worldpay.API.WorldpayMessageDestination.PosRegistration)
                        {
                            var worldpayRegistration = response.ServerMessage.DeserializeBody<SharpAx.Worldpay.API.DTO.WorldpayPointOfSaleRegistrationDTO>();
                            if (!string.IsNullOrEmpty(worldpayRegistration.PointOfSaleLicenseKey))
                            {
                                device.LicenseText = worldpayRegistration.PointOfSaleLicenseKey;
                                var timestampText = response.ServerMessage["timestamp"];
                                if (!string.IsNullOrEmpty(timestampText))
                                {
                                    long timestamp;
                                    if (long.TryParse(timestampText, out timestamp))
                                    {
                                        var issuedOn = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
                                        var expiresOn = issuedOn.AddYears(1);
                                        device.IssuedOnUTC = issuedOn;
                                        device.ExpiryOnUTC = expiresOn;
                                        device.IssuedOn = issuedOn.ToLocalTime();
                                        device.ExpiryOn = expiresOn.ToLocalTime();
                                    }
                                }
                                device.Save();
                                DeviceToForm(device);
                                MessageBox.Show("Registration is completed.", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (response.ServerMessage.GetDestination() == SharpAx.Worldpay.API.WorldpayMessageDestination.Error)
                        {
                            var error = response.ServerMessage.DeserializeBody<SharpAx.Worldpay.API.DTO.WorldpayErrorDTO>();
                            if (error != null)
                            {
                                MessageBox.Show($@"Code: {error.Code}
Message: {error.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            };
            client.ProcessRegistration(new SharpAx.Worldpay.API.Input.WorldpayPointOfSaleRegistrationInput(correlationId)
            {
                PointOfSaleId = device.LicenseId,
                PointOfSaleActivationCode = device.LicenseActivationCode,
                PointOfSaleReference = device.LicenseReference
            });
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            WorldpayDevice device = ReadDeviceFromForm();

            var client = new SharpAx.Worldpay.API.WorldpayClient((WorldpayEnvironment)cbxEnvironment.SelectedItem, device.PaypointName, device.LicenseText, Guid.NewGuid().ToString());
            var correlationId = Guid.NewGuid().ToString();
            client.ReceiveWorldpayServerResponse += (response) =>
            {
                if (response.ServerMessage != null && response.ServerMessage.DoesCorrelationIdMatch(correlationId))
                {
                    if (response.ServerMessage.MessageType == SharpAx.Worldpay.API.Messages.StompMessageServerType.Error)
                    {

                    }
                    else if (response.ServerMessage.MessageType == SharpAx.Worldpay.API.Messages.StompMessageServerType.Message)
                    {
                        if (response.ServerMessage.GetDestination() == SharpAx.Worldpay.API.WorldpayMessageDestination.PosRegistrationRefresh)
                        {
                            var worldpayRegistration = response.ServerMessage.DeserializeBody<SharpAx.Worldpay.API.DTO.WorldpayPointOfSaleRegistrationDTO>();
                            if (!string.IsNullOrEmpty(worldpayRegistration.PointOfSaleLicenseKey))
                            {
                                device.LicenseText = worldpayRegistration.PointOfSaleLicenseKey;
                                var timestampText = response.ServerMessage["timestamp"];
                                if (!string.IsNullOrEmpty(timestampText))
                                {
                                    long timestamp;
                                    if (long.TryParse(timestampText, out timestamp))
                                    {
                                        var issuedOn = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
                                        var expiresOn = issuedOn.AddYears(1);
                                        device.IssuedOnUTC = issuedOn;
                                        device.ExpiryOnUTC = expiresOn;
                                        device.IssuedOn = issuedOn.ToLocalTime();
                                        device.ExpiryOn = expiresOn.ToLocalTime();
                                    }
                                }
                                device.Save();
                                DeviceToForm(device);
                                MessageBox.Show("Registration refresh is completed.", "Registration Refresh Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (response.ServerMessage.GetDestination() == SharpAx.Worldpay.API.WorldpayMessageDestination.Error)
                        {
                            var error = response.ServerMessage.DeserializeBody<SharpAx.Worldpay.API.DTO.WorldpayErrorDTO>();
                            if (error != null)
                            {
                                MessageBox.Show($@"Code: {error.Code}
Message: {error.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                };
            };
            client.ProcessRegistrationRefresh(new SharpAx.Worldpay.API.Input.WorldpayPointOfSaleRegistrationRefreshInput(correlationId));

        }



        private void btnSend_Click(object sender, EventArgs e)
        {
            

            #region Check Input values
            if (string.IsNullOrWhiteSpace(tbxAmount.Text))
            {
                System.Windows.Forms.MessageBox.Show("Amount empty!");
                return;
            }

            bool DeviceInUse = false;
            if (DeviceInUse)
            {
                System.Windows.Forms.MessageBox.Show($"Device already in use. Cannot process payment.");
                return;
            }
            #endregion


            string merchantTransactionReference = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 29).ToUpper();
            decimal amount = Convert.ToDecimal(tbxAmount.Text);
            int WorldpayTimeoutInMinutes = 3;

            WorldpayDevice device = ReadDeviceFromForm();

            Customer customer = ReadCustomerFromForm();

            var worldpayPaymentOperationData = new WorldpayPaymentOperationData((WorldpayPaymentType)cbxPaymentType.SelectedItem, merchantTransactionReference, amount, (WorldpayPaymentInstrument)cbxPaymentInstrument.SelectedItem, WorldpayPaymentRequestType.Payment);
            worldpayPaymentOperationData.SetCustomer(customer);


            var demoWorldpayClient = new DemoWorldpayClient((WorldpayEnvironment)cbxEnvironment.SelectedItem,
                new WorldpayPaymentForm("Waiting for terminal", worldpayPaymentOperationData.PaymentType.ToString().ToUpper(), worldpayPaymentOperationData.Amount, device),
                device,
                TimeSpan.FromMinutes(WorldpayTimeoutInMinutes));

            bool result = demoWorldpayClient.MakePaymentOperation(worldpayPaymentOperationData);
            WorldpayPaymentResultToForm();
        }

        private void WorldpayPaymentResultToForm()
        {
            tbxResult.Text = ""; 
            foreach (var member in Singleton.Instance.LatestWorldpayPayment.GetType().GetMembers() )
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    tbxResult.Text += member.Name + ":" + Environment.NewLine + GetMemberValue(member, Singleton.Instance.LatestWorldpayPayment)?.ToString() + Environment.NewLine + Environment.NewLine;
                }
            }

        }

        private object GetMemberValue(MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    return null;
            }
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {
            var device = this.ReadDeviceFromForm();
            int WorldpayTimeoutInMinutes = 3;
            string merchantTransactionReference = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 29).ToUpper();

            var continueForAvr = true;
            if (device.PerformCardCheckForTokens)
            {

                var worldpayClient = new DemoWorldpayClient((WorldpayEnvironment)cbxEnvironment.SelectedItem,
                    new WorldpayPaymentForm("Waiting for terminal", "CHECK CARD", 0.01M, device),
                    device,
                    TimeSpan.FromMinutes(WorldpayTimeoutInMinutes));
                var worldpayOperationData = new WorldpayPaymentOperationData(WorldpayPaymentType.CheckCard, merchantTransactionReference, 0.01M, WorldpayPaymentInstrument.CardPresent, WorldpayPaymentRequestType.Payment);
                continueForAvr = worldpayClient.MakePaymentOperation(worldpayOperationData, skipTokenCheck: true);
                if (continueForAvr)
                {
                    var cardCheckPayment = Singleton.Instance.LatestWorldpayPayment;
                    WorldpayPaymentResultToForm();
                    if (cardCheckPayment != null)
                    {
                        var applicationLabel = cardCheckPayment.CardApplicationLabel;
                        if (applicationLabel.ToLower().Contains("mastercard"))
                            continueForAvr = true;
                        else if (applicationLabel.ToLower().Contains("visa"))
                            continueForAvr = true;
                        else
                            continueForAvr = false;

                        if (!continueForAvr)
                        {
                            MessageBox.Show("Stored Credentials are currently valid only for Visa and Mastercard. Cannot continue for account verification.", "Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else
                    {
                        //TODO if payment is not found
                        continueForAvr = false;
                    }
                }
            }
            if (continueForAvr)
            {
                var worldpayClient = new DemoWorldpayClient((WorldpayEnvironment)cbxEnvironment.SelectedItem,
                    new WorldpayPaymentForm("Waiting for terminal", "Account Verification Request", 0M, device),
                    device,
                    TimeSpan.FromMinutes(WorldpayTimeoutInMinutes));
                merchantTransactionReference = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 29).ToUpper();
                var worldpayOperationData = new WorldpayPaymentOperationData(WorldpayPaymentType.Sale, merchantTransactionReference, 0M, WorldpayPaymentInstrument.CardPresent, WorldpayPaymentRequestType.AccountVerification);
                if (worldpayClient.MakePaymentOperation(worldpayOperationData, skipTokenCheck: true))
                {
                    var currentCustomer = new Customer();
                    var accountVerificationPayment = Singleton.Instance.LatestWorldpayPayment;
                    if (accountVerificationPayment != null)
                    {
                        var accountVerificationRecord = new WorldpayAccountVerificationRecord();
                        accountVerificationRecord.Customer = currentCustomer;
                        accountVerificationRecord.AccountVerificationPayment = accountVerificationPayment;
                        accountVerificationRecord.CardTokenId = accountVerificationRecord.AccountVerificationPayment.CardTokenId;
                        accountVerificationRecord.CardNumber = accountVerificationPayment.CardNumber;
                        accountVerificationRecord.CardExpiryDateMonth = accountVerificationPayment.CardExpiryDateMonth;
                        accountVerificationRecord.CardExpiryDateYear = accountVerificationPayment.CardExpiryDateYear;
                        accountVerificationRecord.TokenAcquiredDateTime = DateTime.Now;
                        accountVerificationRecord.Purpose = WorldpayTokenPurpose.Unscheduled;
                        currentCustomer.WorldpayAccountVerifications.Add(accountVerificationRecord);
                        accountVerificationRecord.Save();
                        currentCustomer.Save();

                        this.tbxToken.Text = accountVerificationRecord.AccountVerificationPayment.CardTokenId;
                        WorldpayPaymentResultToForm();
                    }
                }
            }
        }
    }


    public class WorldpayPaymentOperationData
    {
        public WorldpayPaymentOperationData(WorldpayPaymentType paymentType, string merchantTransactionReference, decimal amount, WorldpayPaymentInstrument paymentInstrument, WorldpayPaymentRequestType paymentRequestType)
        {
            PaymentType = paymentType;
            MerchantTransactionReference = merchantTransactionReference;
            if (amount < 0M)
                throw new InvalidOperationException("Amount cannot be lower than 0.00!");
            Amount = amount;
            PaymentInstrument = paymentInstrument;
            PaymentRequestType = paymentRequestType;
        }

        public WorldpayPaymentType PaymentType { get; }
        public string MerchantTransactionReference { get; }
        public decimal Amount { get; }
        public WorldpayPaymentInstrument PaymentInstrument { get; }
        public WorldpayPaymentRequestType PaymentRequestType { get; }
        public string OriginalGatewayTransactionReference { get; private set; }
        public bool StoreToken { get; private set; }

        
        public void SetOriginalGatewayTransactionReference(string originalGatewayTransactionReference)
        {
            if (string.IsNullOrEmpty(originalGatewayTransactionReference))
                throw new ArgumentNullException(nameof(originalGatewayTransactionReference));
            OriginalGatewayTransactionReference = originalGatewayTransactionReference;
        }

        public void SetStoreToken(bool store) => StoreToken = store;


        public Customer Customer { get; private set; }
        public void SetCustomer(Customer customer) => Customer = customer;

        public WorldpayToken CheckToken() => new WorldpayToken(Customer);
    }
}
