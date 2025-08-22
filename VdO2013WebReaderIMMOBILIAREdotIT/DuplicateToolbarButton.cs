using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    /// <summary>
    /// Provides toolbar button functionality for property duplication
    /// </summary>
    public static class DuplicateToolbarButton
    {
        /// <summary>
        /// Adds a duplicate button to the specified toolbar
        /// </summary>
        /// <param name="barManager">The bar manager</param>
        /// <param name="toolbar">The toolbar to add the button to</param>
        /// <param name="gridControl">The grid control</param>
        /// <param name="gridView">The grid view</param>
        /// <param name="dataTable">The data table</param>
        /// <returns>The created bar button item</returns>
        public static BarButtonItem AddDuplicateButton(
            BarManager barManager,
            Bar toolbar,
            GridControl gridControl,
            GridView gridView,
            DataTable dataTable)
        {
            if (barManager == null) throw new ArgumentNullException(nameof(barManager));
            if (toolbar == null) throw new ArgumentNullException(nameof(toolbar));
            if (gridControl == null) throw new ArgumentNullException(nameof(gridControl));
            if (gridView == null) throw new ArgumentNullException(nameof(gridView));
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            // Create the duplicate button
            var duplicateButton = new BarButtonItem(barManager, "Duplica Proprietà");
            duplicateButton.Caption = "Duplica";
            duplicateButton.Hint = "Duplica la proprietà selezionata (Ctrl+D)";
            duplicateButton.ItemShortcut = new BarShortcut(Keys.Control | Keys.D);
            duplicateButton.Glyph = GetDuplicateIcon16();
            duplicateButton.LargeGlyph = GetDuplicateIcon32();
            duplicateButton.PaintStyle = BarItemPaintStyle.CaptionGlyph;

            // Add click handler
            duplicateButton.ItemClick += (sender, e) =>
            {
                DuplicateSelectedProperties(gridView, dataTable);
            };

            // Add to toolbar
            toolbar.LinksPersistInfo.Add(new LinkPersistInfo(duplicateButton, true));

            // Enable/disable based on selection
            gridView.SelectionChanged += (sender, e) =>
            {
                duplicateButton.Enabled = gridView.SelectedRowsCount > 0;
            };

            // Initial state
            duplicateButton.Enabled = gridView.SelectedRowsCount > 0;

            return duplicateButton;
        }

        /// <summary>
        /// Creates a duplicate button for a ribbon
        /// </summary>
        public static BarButtonItem CreateRibbonDuplicateButton(
            BarManager barManager,
            GridControl gridControl,
            GridView gridView,
            DataTable dataTable)
        {
            if (barManager == null) throw new ArgumentNullException(nameof(barManager));
            if (gridControl == null) throw new ArgumentNullException(nameof(gridControl));
            if (gridView == null) throw new ArgumentNullException(nameof(gridView));
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            var duplicateButton = new BarButtonItem(barManager, "Duplica Proprietà");
            duplicateButton.Caption = "Duplica Proprietà";
            duplicateButton.Hint = "Duplica le proprietà selezionate";
            duplicateButton.ItemShortcut = new BarShortcut(Keys.Control | Keys.D);
            duplicateButton.Glyph = GetDuplicateIcon16();
            duplicateButton.LargeGlyph = GetDuplicateIcon32();
            duplicateButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;

            // Add click handler
            duplicateButton.ItemClick += (sender, e) =>
            {
                DuplicateSelectedProperties(gridView, dataTable);
            };

            // Enable/disable based on selection
            gridView.SelectionChanged += (sender, e) =>
            {
                duplicateButton.Enabled = gridView.SelectedRowsCount > 0;
                UpdateButtonCaption(duplicateButton, gridView.SelectedRowsCount);
            };

            // Initial state
            duplicateButton.Enabled = gridView.SelectedRowsCount > 0;
            UpdateButtonCaption(duplicateButton, gridView.SelectedRowsCount);

            return duplicateButton;
        }

        /// <summary>
        /// Updates button caption based on selection count
        /// </summary>
        private static void UpdateButtonCaption(BarButtonItem button, int selectedCount)
        {
            if (selectedCount <= 0)
            {
                button.Caption = "Duplica Proprietà";
            }
            else if (selectedCount == 1)
            {
                button.Caption = "Duplica Proprietà";
            }
            else
            {
                button.Caption = $"Duplica {selectedCount} Proprietà";
            }
        }

        /// <summary>
        /// Performs the duplication of selected properties
        /// </summary>
        private static void DuplicateSelectedProperties(GridView gridView, DataTable dataTable)
        {
            try
            {
                int[] selectedRows = gridView.GetSelectedRows();
                if (selectedRows.Length == 0)
                {
                    MessageBox.Show("Seleziona almeno una proprietà da duplicare.", 
                        "Nessuna Selezione", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    return;
                }

                string message;
                if (selectedRows.Length == 1)
                {
                    message = "Vuoi duplicare la proprietà selezionata?";
                }
                else
                {
                    message = $"Vuoi duplicare {selectedRows.Length} proprietà selezionate?";
                }

                var result = MessageBox.Show(message, 
                    "Conferma Duplicazione", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                gridView.BeginDataUpdate();
                try
                {
                    int duplicatedCount = 0;
                    foreach (int rowHandle in selectedRows)
                    {
                        DataRow sourceRow = gridView.GetDataRow(rowHandle);
                        if (sourceRow != null)
                        {
                            PropertyDuplicator.DuplicateProperty(sourceRow, dataTable);
                            duplicatedCount++;
                        }
                    }

                    // Refresh the grid
                    gridView.RefreshData();

                    // Show success message
                    string successMessage = duplicatedCount == 1 
                        ? "1 proprietà duplicata con successo."
                        : $"{duplicatedCount} proprietà duplicate con successo.";

                    MessageBox.Show(successMessage, 
                        "Duplicazione Completata", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    // Clear selection
                    gridView.ClearSelection();
                }
                finally
                {
                    gridView.EndDataUpdate();
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
        /// Gets the 16x16 duplicate icon
        /// </summary>
        private static Image GetDuplicateIcon16()
        {
            // Create a simple duplicate icon (two overlapping documents)
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                
                // Back document
                using (var pen = new Pen(Color.Gray, 1))
                using (var brush = new SolidBrush(Color.LightGray))
                {
                    g.FillRectangle(brush, 3, 3, 10, 12);
                    g.DrawRectangle(pen, 3, 3, 10, 12);
                }
                
                // Front document
                using (var pen = new Pen(Color.Black, 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brush, 1, 1, 10, 12);
                    g.DrawRectangle(pen, 1, 1, 10, 12);
                }
                
                // Lines on front document
                using (var pen = new Pen(Color.Gray, 1))
                {
                    g.DrawLine(pen, 3, 4, 9, 4);
                    g.DrawLine(pen, 3, 6, 9, 6);
                    g.DrawLine(pen, 3, 8, 9, 8);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Gets the 32x32 duplicate icon
        /// </summary>
        private static Image GetDuplicateIcon32()
        {
            // Create a simple duplicate icon (two overlapping documents)
            var bitmap = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Back document
                using (var pen = new Pen(Color.Gray, 2))
                using (var brush = new SolidBrush(Color.LightGray))
                {
                    g.FillRectangle(brush, 8, 8, 18, 22);
                    g.DrawRectangle(pen, 8, 8, 18, 22);
                }
                
                // Front document
                using (var pen = new Pen(Color.Black, 2))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brush, 4, 4, 18, 22);
                    g.DrawRectangle(pen, 4, 4, 18, 22);
                }
                
                // Lines on front document
                using (var pen = new Pen(Color.Gray, 1))
                {
                    g.DrawLine(pen, 7, 9, 19, 9);
                    g.DrawLine(pen, 7, 12, 19, 12);
                    g.DrawLine(pen, 7, 15, 19, 15);
                    g.DrawLine(pen, 7, 18, 19, 18);
                    g.DrawLine(pen, 7, 21, 19, 21);
                }
            }
            return bitmap;
        }
    }
}