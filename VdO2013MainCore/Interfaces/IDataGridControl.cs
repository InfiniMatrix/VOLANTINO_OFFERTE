using DevExpress.Utils.Serializing;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using VdO2013Core;

namespace VdO2013MainCore
{
    public class RowsDroppingEventArgs : CancelEventArgs
    {
    }
    public delegate void RowsDroppingEventHandler(object sender, RowsDroppingEventArgs e);

    public class RowDroppingEventArgs : CancelEventArgs
    {
        public DataRow OldDataRow { get; private set; }
        public DataRow NewDataRow { get; private set; }
        public RowDroppingEventArgs(DataRow oldDataRow, DataRow newDataRow)
        {
            OldDataRow = oldDataRow;
            NewDataRow = newDataRow;
        }
    }
    public delegate void RowDroppingEventHandler(object sender, RowDroppingEventArgs e);

    public class RowsDroppedEventArgs : EventArgs
    {
    }
    public delegate void RowsDroppedEventHandler(object sender, RowsDroppedEventArgs e);
    public class RowDroppedEventArgs : EventArgs
    {
        public DataRow DataRow { get; private set; }
        public RowDroppedEventArgs(DataRow dataRow)
        {
            DataRow = dataRow;
        }
    }
    public delegate void RowDroppedEventHandler(object sender, RowDroppedEventArgs e);

    interface IDataGridControl  : IDevExpressLayoutSupported
    {
        string DataFilterDisplayText { get; }
        bool IsDataFiltered { get; set; }
        bool IsDragDropEnabled { get; set; }

        event CancelEventHandler IsDragDropEnabledChanging;
        event EventHandler IsDragDropEnabledChanged;

        event RowsDroppingEventHandler RowsDropping;
        event RowDroppedEventHandler RowDropped;
        event RowDroppingEventHandler RowDropping;
        event RowsDroppedEventHandler RowsDropped;
        string DataFilterString { get; set; }
        void GridReset(bool removeDataLink = false);
        SiteReaderLinkedInfoControl InfoPane { get; }
        bool InfoPaneVisible { get; set; }
        DevExpress.XtraGrid.Views.Grid.GridView MainDataLinkGridView { get; }
        DevExpress.XtraEditors.ControlNavigator MainDataLinkNavigator { get; }

        //Boolean LoadLayout();
        //Boolean SaveLayout();
        //string GetLayoutName(ISupportXtraSerializer reference);
    }
}
