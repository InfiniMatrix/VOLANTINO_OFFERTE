using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using System.Reflection;
using System.IO;

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
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;

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

    public partial class fmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Credential _credential = null;
        private static Service _service = null;
        public Credential Credential { get { return _credential; } }
        public Service Service { get { return _service; } }

        private DataSet _dataSet = null;
        public bool Connected { get { return _credential != null && _service != null && _dataSet != null; } }
        public static Uri ActivationServiceUri
        {
            get
            {
                return new Uri(WSSettings.WSActivationUri);
            }
        }

        public static Service GetService()
        {
            if (_service == null)
                _service = new Service() { Url = ActivationServiceUri.ToString() };
            return _service;
        }

        public fmMain()
        {
            InitializeComponent();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void fmMain_Shown(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            this.Text += string.Format(" [{0}]", ActivationServiceUri);
        }

        private void LoadControls(Control parentControl = null, TreeListNode parentNode = null)
        {
            if (parentControl == null)
                parentControl = this;

            if (edtControls.Properties.TreeList.Nodes.Count == 0)
            {
                parentNode = edtControls.Properties.TreeList.Nodes.Add(this.Text, this);
                parentNode.Tag = this;
            }

            if (parentNode == null)
                parentNode = edtControls.Properties.TreeList.Nodes[0];

            for (int i = 0; i < parentControl.Controls.Count; i++)
            {
                var c = parentControl.Controls[i];
                var n = parentNode.Nodes.Add(string.IsNullOrEmpty(c.Name) ? c.Text : c.Name, c);
                n.Tag = c;
                if (c.HasChildren)
                    LoadControls(c, n);

                if (c is GridControl)
                {
                    foreach (BaseView view in (c as GridControl).ViewCollection)
                    {
                        var nv = n.Nodes.Add(string.IsNullOrEmpty(view.ViewCaption) ? view.Name : view.ViewCaption, view);
                        nv.Tag = view;
                        if (view is DevExpress.XtraGrid.Views.Grid.GridView)
                        {
                            foreach (DevExpress.XtraGrid.Columns.GridColumn col in (view as DevExpress.XtraGrid.Views.Grid.GridView).Columns)
                            {
                                var nc = nv.Nodes.Add(string.IsNullOrEmpty(col.Caption) ? col.Name : col.Caption, col);
                                nc.Tag = col;
                            }
                        }
                    }
                }

                if (c is VGridControl)
                {
                    var view = c as VGridControl;
                    var nv = n.Nodes.Add(view.Name);
                    nv.Tag = view;
                    foreach (BaseRow row in view.Rows)
                    {
                        var nr = nv.Nodes.Add(string.IsNullOrEmpty(row.Properties.Caption) ? row.Name : row.Properties.Caption, row);
                        nr.Tag = row;
                    }// foreach (var row in view.Rows)
                }// if (c is VGridControl)

            }// for (int i = 0; i < parent.Controls.Count; i++)
        }

        private void Login(bool reload = false)
        {
            if (!reload)
            {
                _credential = fmLogin.Login(this);
                if (_credential != null || _service == null)
                {
                    _service = GetService();
                }
            }//if (!reload)

            if (this.bbiConnect.Down != (_credential != null && _service != null))
                this.bbiConnect.Down = _credential != null && _service != null;

            if (this.bbiConnect.Down)
            {
                if (LoadData() > 0)
                    LoadControls();
                else
                    this.bbiConnect.Down = false;
            }

            this.bbiUpload.Enabled = this.bbiConnect.Down;
            this.bbiReload.Enabled = this.bbiConnect.Down;
            this.bbiActivationCheck.Enabled = this.bbiConnect.Down;
            this.bbiSignupForDemo.Enabled = this.bbiConnect.Down;
            this.ctlLayout.Enabled = this.bbiConnect.Down;
        }

        private void Logout(bool reload = false)
        {
            if (!reload)
            {
                if (_service != null)
                {
                    _service.Dispose();
                    _service = null;
                }

                if (_credential != null)
                {
                    _credential = null;
                }
            }
            if (_dataSet != null)
            {
                _dataSet.Dispose();
                _dataSet = null;
            }

            this.bbiUpload.Enabled = false;
            this.bbiReload.Enabled = false;
            this.bbiActivationCheck.Enabled = false;
            this.bbiSignupForDemo.Enabled = false;
            this.ctlLayout.Enabled = false;
        }

        private void ConfigureView(GridView view, DataSet dataSet, string dataMember)
        {
            string fkDisplayMember = _service.GetExtendedPropertyForeignKeyReferenceDisplayMemberName();
            string fkDisplayMemberSep = _service.GetExtendedPropertyForeignKeyReferenceDisplayMemberSeparator();
            foreach (DataRelation crel in dataSet.Tables[dataMember].ParentRelations)
            {
                var ccol = view.Columns.ColumnByFieldName(crel.ChildColumns[0].ColumnName);
                var tcol = dataSet.Tables[dataMember].Columns[crel.ChildColumns[0].ColumnName];
                if (ccol != null && tcol != null)
                {
                    var crep = new RepositoryItemGridLookUpEdit()
                    {
                        DataSource = crel.ParentTable,
                        ValueMember = crel.ParentColumns[0].ColumnName
                    };
                    string display = tcol.ExtendedProperties[fkDisplayMember].ToString();
                    string[] allDisplay = null;

                    if (display.Contains(fkDisplayMemberSep))
                        allDisplay = display.Split(new string[] { fkDisplayMemberSep }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        allDisplay = new string[] { display };

                    crep.DisplayMember = allDisplay[0];
                    crep.View.Columns.Clear();
                    foreach (string dm in allDisplay)
                    {
                        crep.View.Columns.AddVisible(dm);
                    }
                    ccol.ColumnEdit = crep;
                }
            }

            string fieldID = _service.GetFieldNameID();
            string fieldCode = _service.GetFieldNameCode();

            foreach (GridColumn col in view.Columns)
            {
                var tcol = dataSet.Tables[dataMember].Columns[col.FieldName];
                if (tcol == null) continue;
                if (tcol.AutoIncrement || tcol.ReadOnly)
                {
                    col.AppearanceCell.ForeColor = Color.DarkGray;
                    col.AppearanceCell.Font = new Font(col.AppearanceCell.Font, FontStyle.Bold);
                    col.OptionsColumn.ReadOnly = true;
                }
                else
                    if (col.ColumnType.Equals(typeof(Guid)))
                {
                    var crep = new RepositoryItemButtonEdit();
                    crep.Buttons.Clear();
                    crep.Buttons.Add(new EditorButton(ButtonPredefines.Redo));
                    crep.ButtonClick += GuidCreate_ButtonClick;
                    col.ColumnEdit = crep;
                }
                else
                        if (col.ColumnType.Equals(typeof(string)) && tcol.MaxLength >= 1024)
                {
                    var crep = new RepositoryItemTextEdit();
                    col.ColumnEdit = crep;
                }
            }
        }
        private void ConfigureView(VGridControl view, DataSet dataSet, string dataMember)
        {
            string fkDisplayMember = _service.GetExtendedPropertyForeignKeyReferenceDisplayMemberName();
            string fkDisplayMemberSep = _service.GetExtendedPropertyForeignKeyReferenceDisplayMemberSeparator();
            foreach (DataRelation crel in dataSet.Tables[dataMember].ParentRelations)
            {
                var crow = view.Rows.GetRowByFieldName(crel.ChildColumns[0].ColumnName);
                var tcol = dataSet.Tables[dataMember].Columns[crel.ChildColumns[0].ColumnName];
                if (crow != null && tcol != null)
                {
                    var crep = new RepositoryItemGridLookUpEdit()
                    {
                        DataSource = crel.ParentTable,
                        ValueMember = crel.ParentColumns[0].ColumnName
                    };
                    string display = tcol.ExtendedProperties[fkDisplayMember].ToString();
                    string[] allDisplay = null;

                    if (display.Contains(fkDisplayMemberSep))
                        allDisplay = display.Split(new string[] { fkDisplayMemberSep }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        allDisplay = new string[] { display };

                    crep.DisplayMember = allDisplay[0];
                    crep.View.Columns.Clear();
                    foreach (string dm in allDisplay)
                    {
                        crep.View.Columns.AddVisible(dm);
                    }
                    crow.Properties.RowEdit = crep;
                }
            }

            string fieldID = _service.GetFieldNameID();
            string fieldCode = _service.GetFieldNameCode();

            foreach (EditorRow row in view.Rows)
            {
                var tcol = dataSet.Tables[dataMember].Columns[row.Properties.FieldName];
                if (tcol == null) continue;
                if (tcol.AutoIncrement || tcol.ReadOnly)
                {
                    row.Appearance.ForeColor = Color.DarkGray;
                    row.Appearance.Font = new Font(row.Appearance.Font, FontStyle.Bold);
                    row.Properties.ReadOnly = true;
                }
                else
                    if (row.Properties.RowType.Equals(typeof(Guid)))
                {
                    var crep = new RepositoryItemButtonEdit();
                    crep.Buttons.Clear();
                    crep.Buttons.Add(new EditorButton(ButtonPredefines.Redo));
                    crep.ButtonClick += GuidCreate_ButtonClick;
                    row.Properties.RowEdit = crep;
                }
                else
                        if (row.Properties.RowType.Equals(typeof(string)) && tcol.MaxLength >= 1024)
                {
                    var crep = new RepositoryItemTextEdit();
                    row.Properties.RowEdit = crep;
                }
            }
        }

        private void GuidCreate_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Redo)
            {
                if (sender is ButtonEdit edit)
                {
                    edit.EditValue = Guid.NewGuid();
                }
            }
        }

        private void GridView_MasterRowExpanded(object sender, CustomMasterRowEventArgs e)
        {
            var view = (sender as GridView).GetDetailView(e.RowHandle, e.RelationIndex);
            if (view is GridView && view.DataSource is DataView)
            {
                ConfigureView(view as GridView, (view.DataSource as DataView).Table.DataSet, (view.DataSource as DataView).Table.TableName);
            }
        }

        private void ConfigureGrid(GridControl grid, DataSet dataSet, string dataMember)
        {
            grid.DataSource = dataSet;
            grid.DataMember = dataMember;
            grid.RefreshDataSource();

            grid.MainView.PopulateColumns();
            if (grid.MainView is GridView)
                ConfigureView(grid.MainView as GridView, dataSet, dataMember);
        }

        private void ConfigureGrid(VGridControl grid, DataSet dataSet, string dataMember)
        {
            grid.DataSource = dataSet;
            grid.DataMember = dataMember;
            grid.RefreshDataSource();

            //grid.PopulateRows();
            ConfigureView(grid, dataSet, dataMember);
        }

        private int LoadData()
        {
            if (_credential == null) return -1;
            if (_service == null) return -2;

#if !LICENSEPROVIDER_VER3
            var rDataSet = _service.getDataSet(_credential);
            if (rDataSet.Error.Exists)
#else
            var rDataSet = _service.GetDataSet(_credential);
            if (rDataSet.Error.Assigned)
#endif
                MessageBox.Show(this, rDataSet.Error.Text, rDataSet.Operation, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                _dataSet = null;
                string xml = rDataSet.Result.Value.ToString();
                if (WSActivationHelper.LoadData(xml, out _dataSet, out Exception error) > 0)
                {
                    if (error != null)
                    {
                        MessageBox.Show(this, error.ToReccurentString(), "LoadData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -3;
                    }

                    if (_dataSet.HasChanges()) _dataSet.AcceptChanges();
#if DEBUG
                    Console.WriteLine("DataSet {0}", _dataSet.DataSetName);
#endif
                    foreach (DataTable t in _dataSet.Tables)
                    {
#if DEBUG
                        Console.WriteLine("\t{0}", t.TableName);
#endif
                        t.RowChanging += DataSetTable_RowChanging;
                        t.RowChanged += DataSetTable_RowChanged;
                    }
                    ConfigureGrid(grdContexts, _dataSet, _service.ContextTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdSuites, _dataSet, _service.SuiteTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdCustomers, _dataSet, _service.CustomerTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdActivations, _dataSet, _service.ActivationTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdKeys, _dataSet, _service.KeyTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdWebSites, _dataSet, _service.WebSiteTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdWebSiteScripts, _dataSet, _service.WebSiteScriptTableName(_credential).Result.Value.ToString());
                    ConfigureGrid(grdWebSiteFeatures, _dataSet, _service.WebSiteFeatureTableName(_credential).Result.Value.ToString());

                    return 5;
                }
                else
                    MessageBox.Show(this, "No tables found", rDataSet.Operation, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return -4;
        }

        private void DataSetTable_RowChanging(object sender, DataRowChangeEventArgs e)
        {
#if DEBUG
            int rowIndex = e.Action == DataRowAction.Add ? e.Row.Table.Rows.Count : e.Row.Table.Rows.IndexOf(e.Row);
            Console.WriteLine("RowChanging on {0} with {1} for row at index {2}.", e.Row.Table, e.Action, rowIndex);
#endif
        }

        private void DataSetTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
#if DEBUG
            int rowIndex = e.Action == DataRowAction.Add ? e.Row.Table.Rows.Count : e.Row.Table.Rows.IndexOf(e.Row);
            Console.WriteLine("RowChanged on {0} with {1} for row at index {2}.", e.Row.Table, e.Action, rowIndex);
#endif
        }

        private int RefreshData()
        {
            grdContexts.RefreshDataSource();
            grdSuites.RefreshDataSource();
            grdCustomers.RefreshDataSource();
            grdActivations.RefreshDataSource();
            grdKeys.RefreshDataSource();
            grdWebSites.RefreshDataSource();
            return 6;
        }

        private bool UpdateData()
        {
            if (!Connected || !_dataSet.HasChanges()) return false;
            if (WSActivationHelper.SaveData(_credential, _dataSet, out Exception error) == 0)
                RefreshData();
            if (error != null)
                MessageBox.Show(this, error.ToReccurentString(), "SaveData", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return error == null;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void edtControls_EditValueChanged(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (edtControls.Properties.TreeList.FocusedNode != null)
            {
                ctlUsersProperty.SelectedObject = edtControls.Properties.TreeList.FocusedNode.Tag;
                ctlUsersProperty.RetrieveFields();
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        private void edtControls_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (edtControls.Properties.TreeList.FocusedNode != null)
                e.DisplayText = edtControls.Properties.TreeList.FocusedNode[0].ToString();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiConnect_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (bbiConnect.Down)
                Login();
            else
                Logout();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            switch (UpdateData())
            {
                case false:
                    MessageBox.Show(this, "Failed");
                    break;
                case true:
                    MessageBox.Show(this, "Success");
                    break;
                default:
                    break;
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiActivationCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            var customerCode = viwCustomers.GetFocusedDataRow()[_service.GetFieldNameCode()];
            WSActivationHelper.CustomerActivationsGet(this, customerCode.ToString(), out Exception error);
            if (error != null)
                MessageBox.Show(this, error.ToReccurentString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(this, "Success!", "CustomerActivationsGet", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiSignupForDemo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            WSActivationHelper.CustomerSignupForDemo(this, out Exception error);
            if (error != null)
                MessageBox.Show(this, error.ToReccurentString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(this, "Success!", "CustomerSignupForDemo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            Logout(true);
            Login(true);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            this.Close();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = (MessageBox.Show(this, "Quit application?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Cancel);

            if (!e.Cancel)
            {
                if (bbiConnect.Down) bbiConnect.Down = false;
            }//if (!e.Cancel)

        }
    }
}