using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

using MPLogHelper;
using VdO2013Data;
using VdO2013Core;
using VdO2013SRCore;
using DevExpress.Utils.Menu;
using DevExpress.Utils.DragDrop;
using DevExpress.Utils.Behaviors;
using System.Linq;

namespace VdO2013MainCore
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataGridControl : SiteReaderLinkedControl, IDataGridControl
    {
        //protected readonly BehaviorManager BehaviorManager = null;
        /// <summary>
        /// 
        /// </summary>
        public DataGridControl() : base()
        {
            InitializeComponent();

            //this.BehaviorManager = new BehaviorManager(this.Container);
            this.Controls.Remove(this.ctlGrid);
            this.Controls.Remove(this.ctlReaderInfos);

            this.SplitterFixedPane.Controls.Add(this.ctlGrid);
            this.SplitterFixedPane.Controls.Add(this.ctlReaderInfos);

            viwGrid.RowCountChanged += viwGridRowCountChanged;
            viwGrid.PopupMenuShowing += viwGridPopupMenuShowing;
        }

        private void viwGridRowCountChanged(object sender, EventArgs e)
        {
            InternalUpdateReaderInfoPane();
        }

        private void viwGridPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (Global.ActivationSupport.IsAdmin != Global.BooleanValue.True) return;
            if (InvokePropertyViewer() == null) return;
            if (e.Menu == null) return;
            if (e.HitInfo.InRow) return;
            var m = new DXMenuItem()
            {
                Caption = "Proprietà vista",
                Tag = e.HitInfo.View,
                BeginGroup = true,
            };
            m.Click += (__s, __e) =>
            {
                InvokePropertyViewer().Invoke(__s, new InvokePropertyViewerEventArgs(e.HitInfo.View));
            };
            e.Menu.Items.Add(m);

            var m2 = new DXMenuItem()
            {
                Caption = "Proprietà colonna",
                Tag = e.HitInfo.Column,
            };
            m2.Click += (__s, __e) =>
            {
                InvokePropertyViewer().Invoke(__s, new InvokePropertyViewerEventArgs(e.HitInfo.Column));
            };
            e.Menu.Items.Add(m2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="removeDataLink"></param>
        public virtual void GridReset(bool removeDataLink = false)
        {
            MainDataLink.GridMainView.Columns.Clear();
            MainDataLink.Grid.RepositoryItems.Clear();

            if (DataLink.Count > 0)
            {
                if (removeDataLink)
                {
                    MainDataLink.Reset();
                }

                MainDataLink.GridMainView.RefreshData();
                InternalDataLinkChanged(MainDataLink);
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public SiteReaderLinkedInfoControl InfoPane { get { return this.ctlReaderInfos; } }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public bool InfoPaneVisible { get { return this.InfoPane.Visible; } set { this.InfoPane.Visible = value; this.Update(); } }

        /// <summary>
        /// Returns the count of checked rows.
        /// Remarks: the column value must be a Boolean
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected static int GetCheckedCount(GridColumn column)
        {
            if (!(column.View is GridView view)) return 0;
            //var info = view.GetViewInfo() as GridViewInfo;
            int count = 0;

            if (view.GroupCount == 0)
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    var v = view.GetRowCellValue(i, column);
                    if (!DBNull.Value.Equals(v) && v != null && (bool)v == true)
                        count++;
                }
            }
            else
            {
                // Get the list of grouped columns
                var groupedColumnsList = new List<GridColumn>();
                foreach (GridColumn groupedColumn in view.GroupedColumns)
                    groupedColumnsList.Add(groupedColumn);

                for (int rowHandle = -1; view.IsValidRowHandle(rowHandle); rowHandle--)
                {
                    if (view.GetChildRowHandle(rowHandle, 0) > -1)

                        for (int childRowHandle = 0; childRowHandle < view.GetChildRowCount(rowHandle); childRowHandle++)
                        {
                            var v = view.GetRowCellValue(rowHandle, column);
                            if (!DBNull.Value.Equals(v) && v != null && (bool)v == true)
                                count++;
                        }
                }
            }

            return count;
        }

        [Browsable(false)]
        public GridColumn CheckColumn { get; private set; } = null;

        /// <summary>
        /// Updates the InfoPane
        /// </summary>
        /// <param name="checkColumn"></param>
        protected virtual void InternalUpdateReaderInfoPane(GridColumn checkColumn = null)
        {
            if (checkColumn == null) checkColumn = CheckColumn;
            try
            {
                this.ctlReaderInfos.Reader = this.Reader;
                var count = PrivateCheckGridMainView() ? MainDataLink.GridMainView.RowCount : -1;
                if (checkColumn != null)
                {
                    this.ctlReaderInfos.Elementi = "{0}/{1}".FormatWith(GetCheckedCount(checkColumn), count);
                }
                else
                    this.ctlReaderInfos.Elementi = "{0}".FormatWith(count);

                if (this.IsDataFiltered)
                    this.ctlReaderInfos.Elementi += @"*";
            }
            finally
            {
                CheckColumn = checkColumn;
            }
        }
        /// <summary>
        /// Executes after the Reader has changed
        /// </summary>
        protected override void InternalReaderChanged()
        {
            base.InternalReaderChanged();
            InternalDataLinkChanged(MainDataLink);
            InternalUpdateReaderInfoPane();
        }

        private bool PrivateCheckGridMainView()
        {
            return MainDataLink != null && MainDataLink.GridMainView != null;
        }

        /// <summary>
        /// DevExpress GridControl GridView
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public GridView MainDataLinkGridView => MainDataLink?.GridMainView;

        /// <summary>
        /// DevExpress navigator linked to the Grid
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Aspetto")]
        [DXCategory("Aspetto")]
        public ControlNavigator MainDataLinkNavigator => MainDataLink?.Navigator;

        /// <summary>
        /// Gets or sets whether the filtering functionality is enabled.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        public override bool IsDataFiltered
        {
            get
            {
                if (!PrivateCheckGridMainView()) return false;
                return !string.IsNullOrEmpty(MainDataLink.GridMainView.ActiveFilterString) && MainDataLink.GridMainView.ActiveFilterEnabled;
            }
            set
            {
                if (!PrivateCheckGridMainView()) return;
                MainDataLink.GridMainView.ActiveFilterEnabled = value && !string.IsNullOrEmpty(MainDataLink.GridMainView.ActiveFilterString);
                InternalDataLinkChanged(MainDataLink);
                InternalUpdateReaderInfoPane();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        [DefaultValue("")]
        public override string DataFilterString
        {
            get
            {
                if (!PrivateCheckGridMainView()) return string.Empty;
                return MainDataLink.GridMainView.ActiveFilterEnabled ? MainDataLink.GridMainView.ActiveFilterString : string.Empty;
            }
            set
            {
                if (!PrivateCheckGridMainView()) return;
                MainDataLink.GridMainView.ActiveFilterString = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        [DefaultValue("")]
        public override string DataFilterDisplayText
        {
            get
            {
                if (!PrivateCheckGridMainView()) return string.Empty;
                return MainDataLink.GridMainView.GetFilterDisplayText(MainDataLink.GridMainView.ActiveFilter);
            }
        }

        public event CancelEventHandler IsDragDropEnabledChanging;
        public event EventHandler IsDragDropEnabledChanged;

        public event RowsDroppingEventHandler RowsDropping;
        public event RowDroppingEventHandler RowDropping;
        public event RowDroppedEventHandler RowDropped;
        public event RowsDroppedEventHandler RowsDropped;

        private DragDropBehavior gridControlBehavior = null;

        private void viwGridDragOver(object sender, DragOverEventArgs e)
        {
            var args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
            e.InsertType = args.InsertType;
            e.InsertIndicatorLocation = args.InsertIndicatorLocation;
            e.Action = args.Action;
            Cursor.Current = args.Cursor;
            args.Handled = true;
        }

        private bool CancelRowsDropping(IEnumerable<DataRow> newRows)
        {
            var args = new RowsDroppingEventArgs();
            RowsDropping?.Invoke(viwGrid, args);
            return args.Cancel;
        }

        private bool CancelRowDropping(DataRow oldRow, DataRow newRow)
        {
            var args = new RowDroppingEventArgs(oldRow, newRow);
            RowDropping?.Invoke(viwGrid, args);
            return args.Cancel;
        }

        private void InvokeRowDropped(DataRow newRow)
        {
            RowDropped?.Invoke(viwGrid, new RowDroppedEventArgs(newRow));
        }

        private void viwGridDragDrop(object sender, DragDropEventArgs e)
        {
            if (!(e.Source is GridView sourceGrid))
                return;
            if (!(e.Target is GridView targetGrid))
                return;
            if (e.Action == DragDropActions.None || targetGrid != sourceGrid)
                return;
            if (!(sourceGrid.GridControl.DataSource is DataTable sourceTable))
                sourceTable = ((VdO2013DataCore.Base.VDataViewBase)((BindingSource)sourceGrid.DataSource).DataSource).Table;

            var targerHitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
            var targertHitInfo = targetGrid.CalcHitInfo(targerHitPoint);

            var sourceHandles = e.GetData<int[]>();

            var targetRowHandle = targertHitInfo.RowHandle;
            var targetRowIndex = targetGrid.GetDataSourceRowIndex(targetRowHandle);

            var draggedRows = (from h
                               in sourceHandles
                               select sourceTable.Rows[sourceGrid.GetDataSourceRowIndex(h)]).ToList();

            if (CancelRowsDropping(draggedRows))
                return;

            DataRow CloneRow(DataRow source)
            {
                var result = source.Table.NewRow();
                result.ItemArray = source.ItemArray;
                return result;
            }

            bool MoveRow(DataRow oldRow, int newIndex)
            {
                var newRow = CloneRow(oldRow);

                if (CancelRowDropping(oldRow, newRow))
                    return false;

                sourceTable.Rows.Remove(oldRow);
                sourceTable.Rows.InsertAt(newRow, newIndex);

                InvokeRowDropped(newRow);

                return true;
            }

            int newRowIndex;
            switch (e.InsertType)
            {
                case InsertType.Before:
                    newRowIndex = targetRowIndex > sourceHandles.Last() ? targetRowIndex - 1 : targetRowIndex;
                    for (int i = draggedRows.Count - 1; i >= 0; i--)
                    {
                        if (!MoveRow(draggedRows[i], newRowIndex))
                            break;
                    }
                    break;
                case InsertType.After:
                    newRowIndex = targetRowIndex < sourceHandles.First() ? targetRowIndex + 1 : targetRowIndex;
                    for (int i = 0; i < draggedRows.Count; i++)
                    {
                        if (!MoveRow(draggedRows[i], newRowIndex))
                            break;
                    }
                    break;
                case InsertType.AsChild:
                case InsertType.None:
                default:
                    newRowIndex = -1;
                    break;
            }

            sourceTable.AcceptChanges();
            RowsDropped?.Invoke(viwGrid, new RowsDroppedEventArgs());

            int insertedIndex = targetGrid.GetRowHandle(newRowIndex);
            targetGrid.FocusedRowHandle = insertedIndex;
            targetGrid.SelectRow(targetGrid.FocusedRowHandle);
        }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [DXCategory("Behavior")]
        public bool IsDragDropEnabled
        {
            get => gridControlBehavior != null;
            set
            {
                if (gridControlBehavior != null != value)
                {
                    var e = new CancelEventArgs();
                    IsDragDropEnabledChanging?.Invoke(this.viwGrid, e);
                    if (e.Cancel)
                        return;
                }
                if (value)
                {
                    this.viwGrid.ClearSorting();
                    gridControlBehavior = BehaviorManager.GetBehavior<DragDropBehavior>(this.viwGrid);
                    if (gridControlBehavior == null) return;
                    gridControlBehavior.DragOver += viwGridDragOver;
                    gridControlBehavior.DragDrop += viwGridDragDrop;
                }
                else
                {
                    BehaviorManager.Detach<DragDropBehavior>(this.viwGrid);
                    if (gridControlBehavior == null) return;
                    gridControlBehavior.DragOver -= viwGridDragOver;
                    gridControlBehavior.DragDrop -= viwGridDragDrop;
                    gridControlBehavior = null;
                }

                IsDragDropEnabledChanged?.Invoke(this.viwGrid, EventArgs.Empty);
            }
        }
    }// public partial class DataGridControl
}// namespace VdO2013Main.Controls.Base
