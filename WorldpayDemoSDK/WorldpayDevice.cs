using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WorldPayDemoSDK
{
    public class WorldpayDevice :DatabaseClass
    {
        public WorldpayDevice()
        {
            EnablePrinting = true;
        }

        private string _PaypointName;
        public string PaypointName
        {
            get { return _PaypointName; }
            set { SetPropertyValue<string>(nameof(PaypointName), ref _PaypointName, value); }
        }

        private string _DeviceId;
        public string DeviceId
        {
            get { return _DeviceId; }
            set { SetPropertyValue<string>(nameof(DeviceId), ref _DeviceId, value); }
        }

        private string _TID;
        public string TID
        {
            get { return _TID; }
            set { SetPropertyValue<string>(nameof(TID), ref _TID, value); }
        }

        private string _Emid;
        public string Emid
        {
            get { return _Emid; }
            set { SetPropertyValue<string>(nameof(Emid), ref _Emid, value); }
        }

        private string _LicenseId;
        public string LicenseId
        {
            get { return _LicenseId; }
            set { SetPropertyValue<string>(nameof(LicenseId), ref _LicenseId, value); }
        }

        private string _LicenseActivationCode;
        public string LicenseActivationCode
        {
            get { return _LicenseActivationCode; }
            set { SetPropertyValue<string>(nameof(LicenseActivationCode), ref _LicenseActivationCode, value); }
        }

        private string _LicenseReference;
        public string LicenseReference
        {
            get { return _LicenseReference; }
            set { SetPropertyValue<string>(nameof(LicenseReference), ref _LicenseReference, value); }
        }

        private string _LicenseText;
        [PasswordPropertyText(true)]
        public string LicenseText
        {
            get { return _LicenseText; }
            set { SetPropertyValue<string>(nameof(LicenseText), ref _LicenseText, value); }
        }

        private DateTime _IssuedOnUTC;
        public DateTime IssuedOnUTC
        {
            get { return _IssuedOnUTC; }
            set { SetPropertyValue<DateTime>(nameof(IssuedOnUTC), ref _IssuedOnUTC, value); }
        }

        private DateTime _ExpiryOnUTC;
        public DateTime ExpiryOnUTC
        {
            get { return _ExpiryOnUTC; }
            set { SetPropertyValue<DateTime>(nameof(ExpiryOnUTC), ref _ExpiryOnUTC, value); }
        }

        private DateTime _IssuedOn;
        public DateTime IssuedOn
        {
            get { return _IssuedOn; }
            set { SetPropertyValue<DateTime>(nameof(IssuedOn), ref _IssuedOn, value); }
        }

        private DateTime _ExpiryOn;
        public DateTime ExpiryOn
        {
            get { return _ExpiryOn; }
            set { SetPropertyValue<DateTime>(nameof(ExpiryOn), ref _ExpiryOn, value); }
        }

        private bool _PerformCardCheckForTokens;
        public bool PerformCardCheckForTokens
        {
            get { return _PerformCardCheckForTokens; }
            set { SetPropertyValue<bool>(nameof(PerformCardCheckForTokens), ref _PerformCardCheckForTokens, value); }
        }

        private bool _EnablePrinting;
        public bool EnablePrinting
        {
            get { return _EnablePrinting; }
            set { SetPropertyValue<bool>(nameof(EnablePrinting), ref _EnablePrinting, value); }
        }

        private string _PrinterName;
        public string PrinterName
        {
            get { return _PrinterName; }
            set { SetPropertyValue<string>(nameof(PrinterName), ref _PrinterName, value); }
        }

        private bool _InUse;
        public bool InUse
        {
            get { return _InUse; }
            set { SetPropertyValue<bool>(nameof(InUse), ref _InUse, value); }
        }

        
    }

    public class DeviceUsageTracker: DatabaseClass
    {
        public DeviceUsageTracker() { }

        public WorldpayDevice Device
        { get; set; }

        public bool InUse { get; set; }

        public string UserIdentifier { get; set; }

        public static void SetWorldpayDeviceUseState (WorldpayDevice device, bool pInUse)
        {
            DeviceUsageTracker dut = new DeviceUsageTracker();
            dut.Device = device;
            dut.InUse = pInUse;
            dut.UserIdentifier = "MACHINE, USER, LOCATION, ... ";
            dut.Save();
        }
    }


}
