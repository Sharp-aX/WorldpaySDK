using SharpAx.Worldpay.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldPayDemoSDK
{
    public class Singleton
    {
        private static readonly Singleton _Instance = new Singleton();

        private Singleton() { }

        public static Singleton Instance
        {
            get { return _Instance; }
        }

        public WorldpayPayment LatestWorldpayPayment;

        public Dictionary<(string MerchantTransactionReference, WorldpayPaymentType PaymentType), List<WorldpayReceipt>> WorldpayReceipts { get; private set; } = new Dictionary<(string merchantTransactionReference, WorldpayPaymentType paymentType), List<WorldpayReceipt>>();





        public void WorldpayPrint(WorldpayDevice device, string content, string correlationId)
        {
            var isSignature = false;
            if (!string.IsNullOrEmpty(content) && content.ToUpper().Contains("SIGN HERE"))
                isSignature = true;
            if (!device.EnablePrinting && !isSignature)
                return;
            var printerName = device.PrinterName;
            if (string.IsNullOrEmpty(printerName))
            {
                GetPrinterNameFromPrintDialog();
            }
            if (string.IsNullOrEmpty(printerName)) return;
            using (System.Drawing.Printing.PrintDocument p = new System.Drawing.Printing.PrintDocument())
            {
                p.PrintPage += (object sender1, System.Drawing.Printing.PrintPageEventArgs e1) =>
                {
                    e1.Graphics.DrawString(content, new Font("Courier New", 10), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                };
                p.PrinterSettings.PrinterName = printerName;
                try
                {
                    p.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception Occured While Printing: " + ex);
                }
            }

            string GetPrinterNameFromPrintDialog()
            {
                var printDialog = new PrintDialog();
                var currentForm = new Form();
                currentForm.WindowState = FormWindowState.Minimized;
                currentForm.Show();
                currentForm.WindowState = FormWindowState.Normal;
                currentForm.Activate();
                currentForm.TopMost = true;
                currentForm.Focus();
                currentForm.Visible = false;
                if (printDialog.ShowDialog(currentForm) == DialogResult.OK)
                    printerName = printDialog.PrinterSettings.PrinterName;
                currentForm.Close();
                return printerName;
            }
        }



    }

}
