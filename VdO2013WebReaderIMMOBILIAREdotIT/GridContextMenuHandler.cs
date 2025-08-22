using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    /// <summary>
    /// Handles context menu operations for property grid including duplication
    /// </summary>
    public class GridContextMenuHandler
    {
        private GridControl _gridControl;
        private GridView _gridView;
        private DataTable _dataTable;

        public GridContextMenuHandler(GridControl gridControl, GridView gridView, DataTable dataTable)
        {
            _gridControl = gridControl ?? throw new ArgumentNullException(nameof(gridControl));
            _gridView = gridView ?? throw new ArgumentNullException(nameof(gridView));
            _dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));

            InitializeContextMenu();
        }

        /// <summary>
        /// Initializes the context menu for the grid
        /// </summary>
        private void InitializeContextMenu()
        {
            _gridView.PopupMenuShowing += OnPopupMenuShowing;
        }

        /// <summary>
        /// Handles the popup menu showing event
        /// </summary>
        private void OnPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == GridMenuType.Row)
            {
                // Add separator if menu already has items
                if (e.Menu.Items.Count > 0)
                {
                    e.Menu.Items.Add(new DXMenuCheckItem("-", false) { BeginGroup = true });
                }

                // Add duplicate menu item
                var duplicateItem = new DXMenuItem("Duplica Proprietà", OnDuplicateProperty)
                {
                    Image = GetDuplicateIcon(),
                    Shortcut = Shortcut.CtrlD
                };
                e.Menu.Items.Add(duplicateItem);

                // Add duplicate multiple if multiple rows selected
                if (_gridView.SelectedRowsCount > 1)
                {
                    var duplicateMultipleItem = new DXMenuItem(
                        $"Duplica {_gridView.SelectedRowsCount} Proprietà Selezionate", 
                        OnDuplicateMultipleProperties)
                    {
                        Image = GetDuplicateIcon()
                    };
                    e.Menu.Items.Add(duplicateMultipleItem);
                }

                // Add duplicate with modifications
                var duplicateWithModItem = new DXMenuItem("Duplica e Modifica...", OnDuplicateWithModifications)
                {
                    Image = GetEditIcon()
                };
                e.Menu.Items.Add(duplicateWithModItem);
            }
        }

        /// <summary>
        /// Handles single property duplication
        /// </summary>
        private void OnDuplicateProperty(object sender, EventArgs e)
        {
            try
            {
                int[] selectedRows = _gridView.GetSelectedRows();
                if (selectedRows.Length == 0)
                {
                    MessageBox.Show("Seleziona una proprietà da duplicare.", "Nessuna Selezione", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _gridView.BeginDataUpdate();
                try
                {
                    foreach (int rowHandle in selectedRows)
                    {
                        DataRow sourceRow = _gridView.GetDataRow(rowHandle);
                        if (sourceRow != null)
                        {
                            PropertyDuplicator.DuplicateProperty(sourceRow, _dataTable);
                        }
                    }

                    // Refresh the grid
                    _gridView.RefreshData();

                    MessageBox.Show($"Duplicazione completata: {selectedRows.Length} proprietà duplicate.", 
                        "Duplicazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    _gridView.EndDataUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la duplicazione: {ex.Message}", "Errore", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles multiple properties duplication
        /// </summary>
        private void OnDuplicateMultipleProperties(object sender, EventArgs e)
        {
            try
            {
                int[] selectedRows = _gridView.GetSelectedRows();
                if (selectedRows.Length == 0) return;

                var result = MessageBox.Show(
                    $"Sei sicuro di voler duplicare {selectedRows.Length} proprietà?", 
                    "Conferma Duplicazione", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                _gridView.BeginDataUpdate();
                try
                {
                    var sourceRows = selectedRows
                        .Select(rowHandle => _gridView.GetDataRow(rowHandle))
                        .Where(row => row != null)
                        .ToArray();

                    PropertyDuplicator.DuplicateProperties(sourceRows, _dataTable);

                    _gridView.RefreshData();

                    MessageBox.Show($"Duplicazione completata: {sourceRows.Length} proprietà duplicate.", 
                        "Duplicazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    _gridView.EndDataUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la duplicazione: {ex.Message}", "Errore", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles duplication with modifications
        /// </summary>
        private void OnDuplicateWithModifications(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = _gridView.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MessageBox.Show("Seleziona una proprietà da duplicare.", "Nessuna Selezione", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRow sourceRow = _gridView.GetDataRow(rowHandle);
                if (sourceRow == null) return;

                // Show modification dialog
                using (var dialog = new PropertyModificationDialog(sourceRow, _dataTable.Columns))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _gridView.BeginDataUpdate();
                        try
                        {
                            PropertyDuplicator.ClonePropertyWithModifications(
                                sourceRow, _dataTable, dialog.Modifications);

                            _gridView.RefreshData();

                            MessageBox.Show("Proprietà duplicata con modifiche.", 
                                "Duplicazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        finally
                        {
                            _gridView.EndDataUpdate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la duplicazione: {ex.Message}", "Errore", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the duplicate icon (placeholder - replace with actual icon)
        /// </summary>
        private System.Drawing.Image GetDuplicateIcon()
        {
            // Return null or load actual icon from resources
            return null;
        }

        /// <summary>
        /// Gets the edit icon (placeholder - replace with actual icon)
        /// </summary>
        private System.Drawing.Image GetEditIcon()
        {
            // Return null or load actual icon from resources
            return null;
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        public void Dispose()
        {
            if (_gridView != null)
            {
                _gridView.PopupMenuShowing -= OnPopupMenuShowing;
            }
        }
    }

    /// <summary>
    /// Extension methods for easy integration
    /// </summary>
    public static class GridContextMenuExtensions
    {
        /// <summary>
        /// Enables property duplication on the grid
        /// </summary>
        public static GridContextMenuHandler EnablePropertyDuplication(
            this GridControl gridControl, GridView gridView, DataTable dataTable)
        {
            return new GridContextMenuHandler(gridControl, gridView, dataTable);
        }
    }
}