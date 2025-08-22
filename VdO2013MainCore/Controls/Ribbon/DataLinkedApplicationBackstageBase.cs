using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPExtensionMethods;
using VdO2013DataCore;

namespace VdO2013MainCore.Controls.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataLinkedApplicationBackstageBase : ApplicationBackstageBase, IDataLinkedApplicationBackstage
    {
        private List<Binding> _controlBindings = new List<Binding>();

        /// <summary>
        /// 
        /// </summary>
        public DataLinkedApplicationBackstageBase()
        {
            InitializeComponent();
            BindingSource.CurrentChanged += BindingSource_CurrentChanged;
            BindingSource.CurrentItemChanged += BindingSource_CurrentItemChanged;
        }

        #region Data Support
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BindingSource_CurrentItemChanged(Object sender, EventArgs e) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BindingSource_CurrentChanged(Object sender, EventArgs e) { }

        protected Binding[] CreatedBindings { get { return _controlBindings.ToArray(); } }

        protected class BindingSettings
        {
            public readonly Control Control;
            public readonly object DataSource;
            public readonly string DataMember;
            public readonly string PropertyName = "EditValue";
            public readonly bool FormattingEnabled = false;

            public BindingSettings(Control control, object dataSource, string dataMember, string propertyName = "EditValue", bool formattingEnabled = false)
            {
                Control = control ?? throw new ArgumentNullException(nameof(control));
                DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
                DataMember = dataMember ?? throw new ArgumentNullException(nameof(dataMember));
                PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
                FormattingEnabled = formattingEnabled;
            }
        }

        protected Boolean CreateBinding(BindingSettings settings)
        {
            if (settings.Control == null || String.IsNullOrEmpty(settings.DataMember)) return false;
            if (settings.Control.DataBindings.Count > 0)
                settings.Control.DataBindings.RemoveAt(0);

            var binding = new Binding(settings.PropertyName, settings.DataSource, settings.DataMember, settings.FormattingEnabled)
            {
                DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
                ControlUpdateMode = ControlUpdateMode.OnPropertyChanged
            };

            _controlBindings.Add(binding);
            settings.Control.DataBindings.Add(binding);
            settings.Control.Tag = settings.DataMember;
            return binding != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        protected Boolean TryGetDataRow(out DataRowView row, int rowIndex = 0)
        {
            row = null;
            if (Data == null) return false;
            if (Data.Count == 0) return false;
            row = Data[rowIndex];
            return row != null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        protected DataRowView GetDataRow(int rowIndex = 0)
        {
            return TryGetDataRow(out DataRowView result, rowIndex) ? result : null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fieldName"></param>
        /// <param name="isNullValue"></param>
        /// <returns></returns>
        protected virtual String ReadFieldAsString(int rowIndex, String fieldName, String isNullValue = null)
        {
            if (!TryGetDataRow(out DataRowView r, rowIndex)) return isNullValue;
            if (r.IsDBNull(fieldName) != false) return isNullValue;
            return r[fieldName].ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="isFileName"></param>
        /// <returns></returns>
        protected virtual Boolean WriteFieldAsString(int rowIndex, String fieldName, String value, Boolean isFileName = false)
        {
            if (isFileName && !System.IO.File.Exists(value)) return false;
            DataRowView r = GetDataRow(rowIndex);
            if (r == null) r = Data.AddNew();
            r.BeginEdit();
            r[fieldName] = value ?? String.Empty;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fieldName"></param>
        /// <param name="isNullValue"></param>
        /// <returns></returns>
        protected virtual Int32 ReadFieldAsInt32(int rowIndex, String fieldName, Int32 isNullValue = Int32.MinValue)
        {
            if (!TryGetDataRow(out DataRowView r, rowIndex)) return isNullValue;
            if (r.IsDBNull(fieldName) != false) return isNullValue;
            if (!(r[fieldName] is Int32)) return isNullValue;
            return (Int32)r[fieldName];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="isFileName"></param>
        /// <returns></returns>
        protected virtual Boolean WriteFieldAsInt32(int rowIndex, String fieldName, Int32 value)
        {
            DataRowView r = GetDataRow(rowIndex);
            if (r == null) r = Data.AddNew();
            r.BeginEdit();
            r[fieldName] = value;
            return true;
        }
        #endregion Data Support

        #region Membri di IDataLinkedApplicationBackstage
        /// <summary>
        /// 
        /// </summary>
        public BindingSource BindingSource { get; } = new BindingSource();

        /// <summary>
        /// 
        /// </summary>
        public Object DataSource { get { return BindingSource.DataSource; } }
        /// <summary>
        /// 
        /// </summary>
        public String DataMember { get { return BindingSource.DataMember; } }
        /// <summary>
        /// 
        /// </summary>
        public IVDataView Data { get { return DataSource as IVDataView; } }
        #endregion Membri di IDataLinkedApplicationBackstage

        #region Membri di IVDataViewLinked
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Boolean LoadData()
        {
            var mi = System.Reflection.MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            BindingSource.DataSource = Data.Definition.GetData(out Exception error);
            Logger.WriteError(error);
            var result = BindingSource.DataSource != null;
            Logger.WriteMethodResult(mi, result);
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Boolean SaveData()
        {
            var mi = System.Reflection.MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            var result = Data.Definition.SetData(Data, out Exception error);
            Logger.WriteError(error);
            Logger.WriteMethodResult(mi, result);
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Boolean ClearData()
        {
            var mi = System.Reflection.MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            BindingSource.DataSource = null;
            var result = BindingSource.DataSource == null;
            Logger.WriteMethodResult(mi, result);
            return result;
        }

        /// <summary>
        /// Removes all data
        /// </summary>
        /// <returns></returns>
        public bool DeleteData()
        {
            var mi = System.Reflection.MethodBase.GetCurrentMethod();
            Logger.WriteMethod(mi);
            var result = Data.Definition.DeleteAll(Data, out Exception error);
            Logger.WriteError(error);
            Logger.WriteMethodResult(mi, result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Object[] GetColumnValueList(String columnName) { return Data.GetColumnValues(columnName); }
        #endregion

    }// public partial class DataLinkedApplicationBackstageBase

}// namespace VdO2013Main.Controls.Ribbon
