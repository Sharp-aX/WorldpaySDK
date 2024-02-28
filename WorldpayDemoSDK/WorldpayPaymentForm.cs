using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAx.Worldpay.API.DTO;
using SharpAx.Worldpay.API.Input;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WorldPayDemoSDK
{
    public delegate void WorldpayPaymentCancelEventHandler(WorldpayPaymentCancelEventArgs e);
    public delegate void WorldpayDisconnectEventHandler(WorldpayDisconnectEventArgs e);
    public delegate void WorldpayTerminateTransactionEventHandler(WorldpayTerminateTransactionEventArgs e);

    public class WorldpayPaymentCancelEventArgs : EventArgs
    {
        public WorldpayPaymentCancelEventArgs(WorldpayPaymentCancelInput cancelInput)
        {
        }
    }

    public class WorldpayDisconnectEventArgs : EventArgs
    {
    }

    public class WorldpayTerminateTransactionEventArgs : EventArgs
    {
        public WorldpayTerminateTransactionEventArgs(WorldpayTerminateReason reason)
        {
            Reason = reason;
        }

        public WorldpayTerminateReason Reason { get; }
    }
    
    public interface IWorldpayPaymentForm
    {
        void Process();
        void SetText(string text);
        void SetDescription(string text);
        void SetExitButtonState(bool state);
        void StopProgressCheck();
        DialogResult AskQuestion(string question);
        (string CloseReason, string AuthorisationCode, DialogResult Result) AskReferral(WorldpayReferralMethodDTO referralMethod);
        bool AskAvsMatch(bool cvvMatch, bool addressLineMatch, bool postalCodeMatch);
        event WorldpayPaymentCancelEventHandler PaymentCancel;
        event WorldpayDisconnectEventHandler Disconnect;
        event WorldpayTerminateTransactionEventHandler TerminateTransaction;
    }



    public static class InvokeExtensions
    {
        public static void InvokeIfRequired(this Control control,
                                         MethodInvoker action)
        {
            if (control is null) return;
            if (control.IsHandleCreated)
            {
                if (control.InvokeRequired)
                    control.Invoke(action);
                else
                    action();
            }
        }
    }

    public class MessageForm : Form
    {
        public MessageForm() 
        {
            this.Text = "WORLDPAY";
            this.Size = new Size(400, 200);


            okButton = new Button()
            {
                Dock = DockStyle.Bottom,
                Text = "OK",
                Enabled = false
            };
            okButton.DialogResult = DialogResult.OK;
            
            
            lblCaption = new Label();
            lblCaption.Location = new Point(20, 20);
            lblCaption.Size = new Size(300, 23);

            lblDescription = new Label();
            lblDescription.Location = new Point(20, 50);
            lblDescription.Size = new Size(300, 23);
            panel = new Panel();
            panel.Dock = DockStyle.Fill;

            panel.Controls.Add(lblCaption);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(okButton);
            this.Controls.Add(panel);
            
        }

        public Panel panel;

        public Button okButton;

        public System.Windows.Forms.Label lblDescription;

        public System.Windows.Forms.Label lblCaption;

        public string Description
        {
            get { return "" ;}
            set { lblDescription.Text = value; }
        }

        public string Caption
        {
            get { return ""; }
            set { lblCaption.Text = value; }
        }
    }

    public class WorldpayPaymentForm : IWorldpayPaymentForm
    {
        private object lockObject = new object();
        //private System.Windows.Forms.Button okButton;
        private string initialDescription;

        public WorldpayPaymentForm(string initialText, string initialDescription, decimal amount, WorldpayDevice device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));
            InitialText = initialText;
            InitialDescription = initialDescription;
            Amount = amount;
            Device = device;
            CustomFlyoutDialog = new MessageForm();
        }

        public event WorldpayPaymentCancelEventHandler PaymentCancel;
        public event WorldpayDisconnectEventHandler Disconnect;
        public event WorldpayTerminateTransactionEventHandler TerminateTransaction;

        public string InitialText { get; }
        public string InitialDescription { get; }
        public decimal Amount { get; }
       // public MessageForm ProgressPanel { get; private set; }
        //public UserControl UserControl { get; private set; }
        public MessageForm CustomFlyoutDialog { get; private set; }
        public WorldpayDevice Device { get; }

        public void SetExitButtonState(bool state)
        {
            Button okbtn = CustomFlyoutDialog?.okButton;
            okbtn?.InvokeIfRequired(new MethodInvoker(() =>
            {
                okbtn.Enabled = state;
            }));
        }

        public void Process()
        {
            
            CustomFlyoutDialog = new MessageForm();//(DevExpress.ExpressApp.Win.Templates.XtraFormTemplateBase)Util.app.MainWindow.Template, UserControl);
            CustomFlyoutDialog.KeyDown += CustomFlyoutDialog_KeyDown;
            CustomFlyoutDialog.FormClosing += CustomFlyoutDialog_FormClosing;


            CustomFlyoutDialog.Description = InitialText;
            if (string.IsNullOrEmpty(InitialDescription))
                initialDescription = $"{Amount.ToString("F")}";
            else
                initialDescription = $"{InitialDescription}: {Amount.ToString("F")}";
            CustomFlyoutDialog.Description = initialDescription;
            CustomFlyoutDialog.okButton.Enabled = false;
            CustomFlyoutDialog.ShowDialog();
        }

        private void CustomFlyoutDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Do you really want to terminate transaction?", "Terminate Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    TerminateTransaction?.Invoke(new WorldpayTerminateTransactionEventArgs(WorldpayTerminateReason.UserTerminated));
                }
            }
        }

        private void CustomFlyoutDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect?.Invoke(new WorldpayDisconnectEventArgs());
        }


        public DialogResult AskQuestion(string question)
        {
            if (string.IsNullOrEmpty(question))
                throw new ArgumentNullException(nameof(question));

            DialogResult dialogResult = DialogResult.None;
            CustomFlyoutDialog.InvokeIfRequired(new MethodInvoker(() =>
            {
                dialogResult = MessageBox.Show( question, "", MessageBoxButtons.YesNo);
            }));
            return dialogResult;
        }

        public void SetText(string text)
        {
            if (CustomFlyoutDialog is null) return;
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            CustomFlyoutDialog.InvokeIfRequired(new MethodInvoker(() =>
            {
                CustomFlyoutDialog.Caption = text;
            }));
        }

        public void SetDescription(string text)
        {
            if (CustomFlyoutDialog is null) return;
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            CustomFlyoutDialog.InvokeIfRequired(new MethodInvoker(() =>
            {
                CustomFlyoutDialog.Description = $"{text}";
            }));
        }

        public (string CloseReason, string AuthorisationCode, DialogResult Result) AskReferral(WorldpayReferralMethodDTO referralMethod)
        {
            if (referralMethod is null)
                return ($"{nameof(referralMethod)} argument is null.", "", DialogResult.Abort);
            if (referralMethod.Data is null)
                return ($"{nameof(referralMethod)} argument 'Data' property is null.", "", DialogResult.Abort);
            var referralContactsInformationDictionary = new Dictionary<string, string>();
            foreach (var referralContact in referralMethod.Data.ReferralContacts)
            {
                referralContactsInformationDictionary.Add(FormatMethodText(referralContact.Method), referralContact.Value);
            }
            var worldpayReferralUserControl = new WorldpayReferralUserControl(referralContactsInformationDictionary);
            MessageForm  frm = new MessageForm() { Caption = "Referral" };
            frm.Controls.Add(worldpayReferralUserControl);
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
                return ("OK", worldpayReferralUserControl.AuthorisationCode, DialogResult.OK);
            else
                return ("User cancelled referral action.", "", DialogResult.Cancel);

            string FormatMethodText(string method)
            {
                if (string.IsNullOrEmpty(method))
                    return string.Empty;
                var internalMethodText = method[0].ToString().ToUpper() + method.Substring(1);
                return CapitalizeAfterDash().Replace('-', ' ');

                string CapitalizeAfterDash()
                {
                    StringBuilder sb = new StringBuilder(internalMethodText);
                    for (int i = 0; i < sb.Length - 2; i++)
                    {
                        if (sb[i].Equals('-'))
                            sb[i + 1] = char.ToUpper(sb[i + 1]);
                    }
                    return sb.ToString();
                }
            }
        }

        public bool AskAvsMatch(bool cvvMatch, bool addressLineMatch, bool postalCodeMatch)
        {
            string question = "";
            if (cvvMatch) { question += "CVV Match"; }
            if (addressLineMatch) { question += (question.Length > 1 ? " and ": "") +"Address Line Match"; }
            if (postalCodeMatch) { question += (question.Length > 1 ? " and " : "") +"Postal Code Match"; }
            

            if (MessageBox.Show(question, "Does AVS Match?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public void StopProgressCheck()
        {
            if (CustomFlyoutDialog is null)
                return;
            CustomFlyoutDialog.InvokeIfRequired(new MethodInvoker(() =>
            {
                //ProgressPanel.BarAnimationElementThickness = 0;
            }));
        }
    }


    public class WorldpayReferralUserControl : UserControl
    {
        private TextBox referralContactInformationMemoEdit;
        private TextBox authorisationCodeTextEdit;

        public WorldpayReferralUserControl(Dictionary<string, string> referralContacts)
        {
            var layoutControl = new Panel();
            layoutControl.Dock = DockStyle.Fill;
            var referralContactInformation = new List<string>();
            foreach (var keyValuePair in referralContacts)
            {
                referralContactInformation.Add($@"Method: {keyValuePair.Key}
Value: {keyValuePair.Value}");
            }


            referralContactInformationMemoEdit = new TextBox()
            {
                Multiline = true,
                Text = string.Join($"{Environment.NewLine}------{Environment.NewLine}", referralContactInformation),
                Enabled = false,
                Location = new Point(50, 100)
                
            };
            authorisationCodeTextEdit = new TextBox() { Location = new Point (50,200)};
            authorisationCodeTextEdit.TextChanged += AuthorisationCodeTextEdit_EditValueChanged;
            var okButton = new Button()
            {
                Dock = DockStyle.Bottom,
                Text = "OK",
                Enabled = false,
                Location = new Point(20, 250)
            };
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += OkButton_Click;
            layoutControl.Controls.Add(new Label() { Text = "Contact: ", Location = new Point(20, 50) });
            layoutControl.Controls.Add(referralContactInformationMemoEdit);
            layoutControl.Controls.Add(new Label() { Text = "Authorisation Code: " , Location = new Point(20,150)});
            layoutControl.Controls.Add(authorisationCodeTextEdit);
            layoutControl.Controls.Add(okButton);
            Controls.Add(layoutControl);
            Height = 300;
            Dock = DockStyle.Top;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            (sender as Form).Close();
        }

        private void AuthorisationCodeTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            AuthorisationCode = Convert.ToString(authorisationCodeTextEdit.Text);
        }

        public string AuthorisationCode { get; private set; }
    }

}

