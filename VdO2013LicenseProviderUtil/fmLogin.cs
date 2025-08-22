using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace VdO2013LicenseProviderUtil
{
#if LICENSEPROVIDER_VER1
    using Credential = WSActivation.LPCredential;
    using LicenseProvider = WSActivation.VdOLicenseProvider;
    using Message = WSActivation.LPMessage;
#elif LICENSEPROVIDER_VER2
    using Credential = WSActivation2.LPCredential;
    using Service = WSActivation2.VdOLicenseProvider2;
    using Message = WSActivation2.LPMessage;
#elif LICENSEPROVIDER_VER3
    using Credential = WSActivation3.LPCredential;
    using Service = WSActivation3.VdOLicenseProvider3;
    using Message = WSActivation3.LPMessage;
#endif

    public partial class fmLogin : DevExpress.XtraEditors.XtraForm
    {
        public fmLogin()
        {
            InitializeComponent();
        }

        public static Credential GetCredential(IWin32Window owner, Object login, Object password, Object context)
        {
            try
            {
                var svc = fmMain.GetService();
                return svc.CredentialGet(login.ToString(), password.ToString(), context.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// try
            return null;
        }

        public static Credential Validate(IWin32Window owner, Object login, Object password, Object context)
        {
            try
            {
                var svc = fmMain.GetService();

                var cred = svc.CredentialGet(login.ToString(), password.ToString(), context.ToString());
                var result = svc.DoLogin(cred);
#if !LICENSEPROVIDER_VER3
                if (!result.Error.Exists)
#else
                if (!result.Error.Assigned)
#endif
                {

                    return GetCredential(owner, login, password, context);
                }
                else
                {
                    MessageBox.Show(owner, result.Error.Text, result.Operation + " failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// try
            return null;
        }

        public static Credential Login(IWin32Window owner)
        {
            using (var fLogin = new fmLogin())
            {
                switch (fLogin.ShowDialog(owner))
                {
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        var credential = Validate(owner, fLogin.edtLogin.EditValue, fLogin.edtPassword.EditValue, fLogin.edtContext.EditValue);
                        if (credential != null)
                        {
                            Properties.Settings.Default.login = fLogin.edtLogin.Text;
                            Properties.Settings.Default.context = fLogin.edtContext.Text;
                            Properties.Settings.Default.Save();
                            return credential;
                        }
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Yes:
                        break;
                    default:
                        break;
                }
            }// using(var fLogin = new fmLogin())
            return null;
        }

        private void EditsValueChanged(Object sender, EventArgs e)
        {
            btnOk.Enabled = !String.IsNullOrEmpty(edtLogin.Text) && !String.IsNullOrEmpty(edtPassword.Text) && !String.IsNullOrEmpty(edtContext.Text);
        }

        private void fmLogin_Load(object sender, EventArgs e)
        {
            edtLogin.Text = Properties.Settings.Default.login;
            edtContext.Text = Properties.Settings.Default.context;
        }
    }
}