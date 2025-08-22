using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    /// <summary>
    /// Dialog for modifying property values during duplication
    /// </summary>
    public partial class PropertyModificationDialog : XtraForm
    {
        private Dictionary<string, object> _modifications;
        private DataRow _sourceRow;
        private DataColumnCollection _columns;
        private LayoutControl _layoutControl;
        private SimpleButton _okButton;
        private SimpleButton _cancelButton;
        private Dictionary<string, BaseEdit> _editors;

        public Dictionary<string, object> Modifications => _modifications;

        public PropertyModificationDialog(DataRow sourceRow, DataColumnCollection columns)
        {
            _sourceRow = sourceRow ?? throw new ArgumentNullException(nameof(sourceRow));
            _columns = columns ?? throw new ArgumentNullException(nameof(columns));
            _modifications = new Dictionary<string, object>();
            _editors = new Dictionary<string, BaseEdit>();

            InitializeComponent();
            CreateControls();
        }

        private void InitializeComponent()
        {
            this.Text = "Duplica e Modifica Proprietà";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            _layoutControl = new LayoutControl();
            _layoutControl.Dock = DockStyle.Fill;
            this.Controls.Add(_layoutControl);

            // Create buttons panel
            var buttonPanel = new Panel();
            buttonPanel.Height = 40;
            buttonPanel.Dock = DockStyle.Bottom;
            this.Controls.Add(buttonPanel);

            _okButton = new SimpleButton();
            _okButton.Text = "OK";
            _okButton.Size = new Size(75, 25);
            _okButton.Location = new Point(this.Width - 170, 8);
            _okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            _okButton.Click += OkButton_Click;
            buttonPanel.Controls.Add(_okButton);

            _cancelButton = new SimpleButton();
            _cancelButton.Text = "Annulla";
            _cancelButton.Size = new Size(75, 25);
            _cancelButton.Location = new Point(this.Width - 85, 8);
            _cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            _cancelButton.Click += CancelButton_Click;
            buttonPanel.Controls.Add(_cancelButton);
        }

        private void CreateControls()
        {
            _layoutControl.BeginUpdate();
            try
            {
                var group = _layoutControl.AddGroup();
                group.Text = "Modifica valori per la nuova proprietà";

                // Create editors for important fields
                var fieldsToShow = new[] 
                { 
                    "Codice", "Riferimento", "Descrizione", "Zona", 
                    "Tipologia", "Mq", "Locali", "Prezzo", "Proposta",
                    "Indirizzo", "nBagni", "ClasseEnergetica"
                };

                foreach (var fieldName in fieldsToShow)
                {
                    if (!_columns.Contains(fieldName))
                        continue;

                    var column = _columns[fieldName];
                    if (column.ReadOnly || column.AutoIncrement)
                        continue;

                    BaseEdit editor = CreateEditor(column);
                    if (editor != null)
                    {
                        editor.Name = "editor_" + fieldName;
                        
                        // Set current value
                        if (_sourceRow[fieldName] != DBNull.Value)
                        {
                            if (fieldName == "Codice")
                            {
                                // Generate new code suggestion
                                editor.EditValue = GenerateNewCodeSuggestion(_sourceRow[fieldName].ToString());
                            }
                            else if (fieldName == "Riferimento")
                            {
                                // Add COPIA prefix
                                editor.EditValue = "COPIA-" + _sourceRow[fieldName].ToString();
                            }
                            else
                            {
                                editor.EditValue = _sourceRow[fieldName];
                            }
                        }

                        var item = group.AddItem(fieldName, editor);
                        item.TextVisible = true;
                        _editors[fieldName] = editor;
                    }
                }

                // Add note
                var noteLabel = new LabelControl();
                noteLabel.Text = "Nota: Lascia vuoti i campi che non vuoi modificare.";
                noteLabel.Appearance.ForeColor = Color.Gray;
                var noteItem = group.AddItem("", noteLabel);
                noteItem.TextVisible = false;
            }
            finally
            {
                _layoutControl.EndUpdate();
            }
        }

        private BaseEdit CreateEditor(DataColumn column)
        {
            BaseEdit editor = null;

            // Create appropriate editor based on data type
            if (column.DataType == typeof(string))
            {
                if (column.ColumnName.Contains("Descrizione") || 
                    column.MaxLength > 100)
                {
                    editor = new MemoEdit();
                    ((MemoEdit)editor).Properties.MaxLength = column.MaxLength;
                }
                else
                {
                    editor = new TextEdit();
                    ((TextEdit)editor).Properties.MaxLength = column.MaxLength;
                }
            }
            else if (column.DataType == typeof(int) || 
                     column.DataType == typeof(long))
            {
                editor = new SpinEdit();
                ((SpinEdit)editor).Properties.IsFloatValue = false;
                ((SpinEdit)editor).Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                ((SpinEdit)editor).Properties.Mask.EditMask = "d";
            }
            else if (column.DataType == typeof(decimal) || 
                     column.DataType == typeof(double) || 
                     column.DataType == typeof(float))
            {
                editor = new SpinEdit();
                ((SpinEdit)editor).Properties.IsFloatValue = true;
                
                if (column.ColumnName.Contains("Prezzo"))
                {
                    ((SpinEdit)editor).Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    ((SpinEdit)editor).Properties.Mask.EditMask = "c0";
                    ((SpinEdit)editor).Properties.Mask.UseMaskAsDisplayFormat = true;
                }
            }
            else if (column.DataType == typeof(DateTime))
            {
                editor = new DateEdit();
            }
            else if (column.DataType == typeof(bool))
            {
                editor = new CheckEdit();
            }
            else
            {
                editor = new TextEdit();
            }

            return editor;
        }

        private string GenerateNewCodeSuggestion(string originalCode)
        {
            if (string.IsNullOrEmpty(originalCode))
                return "PROP_COPY1";

            // Simple suggestion - append _COPY1
            var match = System.Text.RegularExpressions.Regex.Match(originalCode, @"(.+?)_COPY(\d+)$");
            if (match.Success)
            {
                var baseCode = match.Groups[1].Value;
                var number = int.Parse(match.Groups[2].Value) + 1;
                return baseCode + "_COPY" + number;
            }
            else
            {
                return originalCode + "_COPY1";
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect modifications
                _modifications.Clear();

                foreach (var kvp in _editors)
                {
                    var fieldName = kvp.Key;
                    var editor = kvp.Value;

                    if (editor.EditValue != null && 
                        editor.EditValue != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(editor.EditValue.ToString()))
                    {
                        // Check if value is different from original
                        bool isDifferent = true;
                        if (_sourceRow[fieldName] != DBNull.Value)
                        {
                            isDifferent = !editor.EditValue.Equals(_sourceRow[fieldName]);
                        }

                        if (isDifferent || fieldName == "Codice" || fieldName == "Riferimento")
                        {
                            _modifications[fieldName] = editor.EditValue;
                        }
                    }
                }

                // Validate required fields
                if (_columns.Contains("Codice") && !_modifications.ContainsKey("Codice"))
                {
                    MessageBox.Show("Il campo Codice è obbligatorio.", "Validazione", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _editors["Codice"].Focus();
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la raccolta delle modifiche: {ex.Message}", 
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _layoutControl?.Dispose();
                _okButton?.Dispose();
                _cancelButton?.Dispose();
                
                foreach (var editor in _editors.Values)
                {
                    editor?.Dispose();
                }
                _editors.Clear();
            }
            base.Dispose(disposing);
        }
    }
}