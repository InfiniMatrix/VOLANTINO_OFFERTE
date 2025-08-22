using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    /// <summary>
    /// Example of how to integrate the property duplication functionality
    /// into an existing form or user control
    /// </summary>
    public class DuplicationIntegrationExample
    {
        private GridControl _gridControl;
        private GridView _gridView;
        private DataTable _dataTable;
        private BarManager _barManager;
        private GridContextMenuHandler _contextMenuHandler;

        /// <summary>
        /// Example method showing how to integrate duplication into existing grid
        /// </summary>
        public void IntegrateDuplicationFeature(
            GridControl existingGridControl,
            GridView existingGridView,
            DataTable existingDataTable,
            BarManager existingBarManager = null,
            Bar existingToolbar = null)
        {
            // Store references
            _gridControl = existingGridControl;
            _gridView = existingGridView;
            _dataTable = existingDataTable;
            _barManager = existingBarManager;

            // 1. Enable context menu with duplication options
            EnableContextMenuDuplication();

            // 2. Add toolbar button if toolbar exists
            if (existingBarManager != null && existingToolbar != null)
            {
                AddToolbarButton(existingToolbar);
            }

            // 3. Add keyboard shortcut
            AddKeyboardShortcut();

            // 4. Configure grid for better duplication experience
            ConfigureGridForDuplication();
        }

        /// <summary>
        /// Enables context menu duplication
        /// </summary>
        private void EnableContextMenuDuplication()
        {
            // This enables the right-click context menu with duplication options
            _contextMenuHandler = new GridContextMenuHandler(_gridControl, _gridView, _dataTable);
        }

        /// <summary>
        /// Adds toolbar button for duplication
        /// </summary>
        private void AddToolbarButton(Bar toolbar)
        {
            if (_barManager != null)
            {
                DuplicateToolbarButton.AddDuplicateButton(
                    _barManager, 
                    toolbar, 
                    _gridControl, 
                    _gridView, 
                    _dataTable);
            }
        }

        /// <summary>
        /// Adds keyboard shortcut for duplication
        /// </summary>
        private void AddKeyboardShortcut()
        {
            // Add Ctrl+D shortcut to the grid control
            _gridControl.ProcessGridKey += OnGridControlProcessGridKey;
        }

        /// <summary>
        /// Handles grid key processing for shortcuts
        /// </summary>
        private void OnGridControlProcessGridKey(object sender, KeyEventArgs e)
        {
            // Check for Ctrl+D
            if (e.Control && e.KeyCode == Keys.D)
            {
                DuplicateSelectedRows();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Configures grid settings for better duplication experience
        /// </summary>
        private void ConfigureGridForDuplication()
        {
            // Enable multi-selection if not already enabled
            _gridView.OptionsSelection.MultiSelect = true;
            _gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

            // Enable row focus rectangle
            _gridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;

            // Add visual feedback for duplicated rows
            _gridView.RowStyle += OnGridViewRowStyle;
        }

        /// <summary>
        /// Provides visual feedback for newly duplicated rows
        /// </summary>
        private void OnGridViewRowStyle(object sender, RowStyleEventArgs e)
        {
            // Highlight rows that were created in the last minute (newly duplicated)
            if (e.RowHandle >= 0)
            {
                var row = _gridView.GetDataRow(e.RowHandle);
                if (row != null)
                {
                    // Check if row has InsertDate column
                    if (_dataTable.Columns.Contains("InsertDate") && 
                        row["InsertDate"] != DBNull.Value)
                    {
                        var insertDate = Convert.ToDateTime(row["InsertDate"]);
                        if (DateTime.Now.Subtract(insertDate).TotalMinutes < 1)
                        {
                            // Highlight recently added rows
                            e.Appearance.BackColor = System.Drawing.Color.LightGreen;
                            e.Appearance.BackColor2 = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Duplicates selected rows
        /// </summary>
        private void DuplicateSelectedRows()
        {
            try
            {
                int[] selectedRows = _gridView.GetSelectedRows();
                if (selectedRows.Length == 0)
                {
                    MessageBox.Show("Seleziona almeno una proprietà da duplicare.", 
                        "Nessuna Selezione", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
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

                    _gridView.RefreshData();

                    MessageBox.Show($"{selectedRows.Length} proprietà duplicate con successo.", 
                        "Duplicazione Completata", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                finally
                {
                    _gridView.EndDataUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la duplicazione: {ex.Message}", 
                    "Errore", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Example of how to use in a form
        /// </summary>
        public static void ExampleUsage(Form mainForm)
        {
            // Assuming you have these controls on your form:
            // - gridControlProperties: GridControl showing properties
            // - gridViewProperties: GridView of the grid control
            // - dataTableProperties: DataTable with property data
            // - barManager1: BarManager for toolbars
            // - barTools: A toolbar where you want to add the duplicate button

            /*
            var integrator = new DuplicationIntegrationExample();
            integrator.IntegrateDuplicationFeature(
                gridControlProperties,
                gridViewProperties,
                dataTableProperties,
                barManager1,
                barTools);
            */
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public void Dispose()
        {
            if (_gridControl != null)
            {
                _gridControl.ProcessGridKey -= OnGridControlProcessGridKey;
            }

            if (_gridView != null)
            {
                _gridView.RowStyle -= OnGridViewRowStyle;
            }

            _contextMenuHandler?.Dispose();
        }
    }
}