using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.IO;
using System.Xml;

using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

using MPExtensionMethods;

namespace VdO2013LicenseProviderUtil
{
#if LICENSEPROVIDER_VER1
    using Credential = WSActivation.LPCredential;
    using LicenseProvider = WSActivation.VdOLicenseProvider;
    using Message = WSActivation.LPMessage;
    using Bool = WSActivation.LPBoolean;
#elif LICENSEPROVIDER_VER2
    using Credential = WSActivation2.LPCredential;
    using Service = WSActivation2.VdOLicenseProvider2;
    using Message = WSActivation2.LPMessage;
    using Bool = WSActivation2.LPBoolean;
#elif LICENSEPROVIDER_VER3
    using Credential = WSActivation3.LPCredential;
    using Service = WSActivation3.VdOLicenseProvider3;
    using Message = WSActivation3.LPMessage;
    using Bool = WSActivation3.LPBoolean;
#endif

    internal class WSActivationHelper
    {
        public static int LoadData(string xml, out DataSet data, out Exception error)
        {
            data = null;
            error = null;
            if (string.IsNullOrEmpty(xml)) return -1;
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    using (var xr = new XmlTextReader(ms))
                    {
                        data = new DataSet();
                        data.ReadXml(xr);
                    }
                }
                return data.Tables.Count;
            }
            catch (XmlException ex)
            {
                var lines = xml.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string text = ex.LineNumber < lines.Length ? lines[ex.LineNumber] : string.Empty;
                error = new XmlException(text, ex);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return -1;
        }

        public static int LoadData(string xml, out DataTable data, out Exception error)
        {
            data = null;
            error = null;
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    using (var xr = new XmlTextReader(ms))
                    {
                        data = new System.Data.DataTable();
                        data.ReadXml(xr);
                    }
                }
                return data.Rows.Count;
            }
            catch (XmlException ex)
            {
                var lines = xml.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string text = ex.LineNumber < lines.Length ? lines[ex.LineNumber] : string.Empty;
                error = new XmlException(text, ex);
            }
            catch (System.Exception ex)
            {
                error = ex;
            }
            return -1;
        }

        public static int SaveData(Credential _credential, DataSet _dataSet, out Exception error)
        {
            error = null;
            if (!_dataSet.HasChanges()) return -1;

            Service _service = fmMain.GetService();
#if !LICENSEPROVIDER_VER3
            Message message = _service.setDataSet(_credential, _dataSet);
            if (message.Error.Exists)
#else
            Message message = _service.SetDataSet(_credential, _dataSet);
            if (message.Error.Assigned)
#endif
            {
                error = new Exception(message.Error.Text);
                _dataSet.RejectChanges();
            }
            else
            {
                _dataSet.AcceptChanges();
                if (message.Result.IsBoolean && message.Result.AsBoolean == Bool.True)
                    return 0;
            }
            return -2;
        }

        public static int CustomerActivationsGet(IWin32Window owner, string customerCode, out Exception error)
        {
            error = null;
            if (string.IsNullOrEmpty(customerCode)) return -1;

            try
            {
                Service _service = fmMain.GetService();
                Message ractivations = _service.CustomerActivationsGet(customerCode.ToString(), null, null);
#if !LICENSEPROVIDER_VER3
                if (ractivations.Error.Exists)
#else
                if (ractivations.Error.Assigned)
#endif
                {
                    error = new Exception(ractivations.Error.Text);
                    return -2;
                }

                using (XtraForm form = new XtraForm())
                {
                    form.Text = string.Format("Customer Code: {0}", customerCode);
                    form.Width = 600;
                    form.Height = 300;

                    var pnlButtons = new PanelControl();
                    form.Controls.Add(pnlButtons);
                    pnlButtons.Dock = DockStyle.Bottom;
                    pnlButtons.Height = 32;

                    var btnOk = new SimpleButton()
                    {
                        Text = "OK",
                        DialogResult = System.Windows.Forms.DialogResult.OK
                    };
                    form.AcceptButton = btnOk;
                    pnlButtons.Controls.Add(btnOk);
                    btnOk.Top = 4;
                    btnOk.Left = pnlButtons.Width - (btnOk.Width + 4);
                    btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                    var grid = new GridControl();
                    form.Controls.Add(grid);
                    grid.Dock = DockStyle.Fill;

#if !LICENSEPROVIDER_VER3
                    if (!ractivations.Error.Exists && ractivations.Result.IsDataTable)
#else
                    if (!ractivations.Error.Assigned && ractivations.Result.IsDataTable)
#endif
                    {
                        grid.DataSource = ractivations.Result.AsDataTable;
                        var view = new GridView();
                        view.OptionsBehavior.ReadOnly = true;
                        view.OptionsBehavior.Editable = false;
                        view.OptionsView.ShowGroupPanel = false;
                        grid.MainView = view;
                    }
                    else
                    {
                        return -3;
                    }

                    form.ShowDialog(owner);
                }// using(XtraForm form = new XtraForm())
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, ex.ToReccurentString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -4;
            }// try
            return 0;
        }

        private static BindingFlags GetBindingFlags()
        {
            return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
        }

        private static ParameterInfo[] GetMethodParameters(Service _service, string methodName)
        {
            var st = _service.GetType();
            var mi = st.GetMethod(methodName, GetBindingFlags());
            return mi.GetParameters();
        }

        private static Message GetMethodInvoke(Service _service, string methodName, object[] args)
        {
            return _service.GetType().GetMethod(methodName, GetBindingFlags()).Invoke(_service, args) as Message;
        }

        public static int CustomerSignupForDemo(IWin32Window owner, out Exception error)
        {
            error = null;

            try
            {
                Service _service = fmMain.GetService();
                var mn = _service.GetCustomerSignupForDemoMethodName();
                var ps = GetMethodParameters(_service, mn);

                using (XtraForm form = new XtraForm())
                {
                    form.Text = "Signup for DEMO";
                    form.Width = 300;
                    form.Height = 340;
                    form.AutoSize = true;
                    form.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;

                    var pnlButtons = new PanelControl();
                    form.Controls.Add(pnlButtons);
                    pnlButtons.Dock = DockStyle.Bottom;
                    pnlButtons.Height = 32;

                    var btnOk = new SimpleButton()
                    {
                        Text = "OK",
                        DialogResult = System.Windows.Forms.DialogResult.OK
                    };
                    form.AcceptButton = btnOk;
                    pnlButtons.Controls.Add(btnOk);
                    btnOk.Top = 4;
                    btnOk.Left = pnlButtons.Width - (btnOk.Width + 4);
                    btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                    var btnCancel = new SimpleButton()
                    {
                        Text = "Annulla",
                        DialogResult = System.Windows.Forms.DialogResult.Cancel
                    };
                    form.CancelButton = btnCancel;
                    pnlButtons.Controls.Add(btnCancel);
                    btnCancel.Bounds = btnOk.Bounds;
                    btnCancel.Left = btnOk.Left - (btnCancel.Width + 4);
                    btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                    var ctlLayout = new LayoutControl();
                    form.Controls.Add(ctlLayout);
                    ctlLayout.Dock = DockStyle.Fill;
                    ctlLayout.Root = new LayoutControlGroup()
                    {
                        TextLocation = DevExpress.Utils.Locations.Top,
                        TextVisible = false
                    };
                    ctlLayout.OptionsCustomizationForm.ShowLayoutTreeView = true;
                    ctlLayout.OptionsCustomizationForm.ShowPropertyGrid = true;

                    List<Control> ctrls = new List<Control>();
                    foreach (var pi in ps)
                    {
                        Control ctrl = null;
                        TypeCode tc = Type.GetTypeCode(pi.ParameterType);
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                                ctrl = new CheckEdit() { Tag = pi };
                                break;
                            case TypeCode.Char:
                                break;
                            case TypeCode.DateTime:
                                ctrl = new DateEdit() { Tag = pi };
                                break;
                            case TypeCode.Decimal:
                                var dc = new SpinEdit() { Tag = pi };
                                dc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dc.Properties.EditFormat.FormatString = "n2";
                                ctrl = dc;
                                break;
                            case TypeCode.Double:
                                var db = new SpinEdit() { Tag = pi };
                                db.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                db.Properties.EditFormat.FormatString = "n2";
                                ctrl = db;
                                break;
                            case TypeCode.String:
                                var tx = new TextEdit() { Tag = pi };
                                ctrl = tx;
                                break;
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.SByte:
                            case TypeCode.Single:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                var nm = new SpinEdit() { Tag = pi };
                                nm.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                ctrl = nm;
                                break;
                            case TypeCode.DBNull:
                                break;
                            case TypeCode.Empty:
                                break;
                            case TypeCode.Object:
                                break;
                            default:
                                break;
                        }

                        if (ctrl != null)
                        {
                            ctrls.Add(ctrl);
                            var lci = new LayoutControlItem()
                            {
                                Text = pi.Name,
                                Control = ctrl
                            };
                            ctlLayout.Root.Add(lci);
                        }
                    }

                    if (form.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                    {
                        var args = new List<object>();
                        foreach (var ctrl in ctrls)
                            args.Add((ctrl as BaseEdit).EditValue);
                        var result = GetMethodInvoke(_service, mn, args.ToArray());

#if !LICENSEPROVIDER_VER3
                        if (result.Error.Exists)
#else
                        if (result.Error.Assigned)
#endif
                        {
                            error = new Exception(result.Error.Text);
                            return -4;
                        }
                        return 0;
                    }
                    else
                        return -2;
                }// using(XtraForm form = new XtraForm())
            }
            catch (Exception ex)
            {
                error = ex;
                return -3;
            }// try
        }
    }
}
