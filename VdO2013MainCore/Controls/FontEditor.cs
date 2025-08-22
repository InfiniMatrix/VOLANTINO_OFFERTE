using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views;
using MPUtils;

namespace VdO2013MainCore.Controls
{
    public partial class FontEditor : UserControl
    {
        public FontHelper Helper { get; private set; }
        private int changed = 0;
        public bool Changed => changed > 0;

        public FontEditor()
        {
            InitializeComponent();
            Init(new FontHelper());
        }

        public FontEditor(FontHelper helper, IEnumerable<string> readOnlyProperties = null)
        {
            InitializeComponent();
            Init(helper, readOnlyProperties);
        }

        public FontEditor(Font helperFont, Color helperColor, IEnumerable<string> readOnlyProperties = null)
        {
            InitializeComponent();
            Init(new FontHelper(helperFont, helperColor), readOnlyProperties);
        }

        private void Init(FontHelper helper, IEnumerable<string> readOnlyProperties = null)
        {
            Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            Helper.PropertyChanging += Helper_PropertyChanging;
            Helper.PropertyChanged += Helper_PropertyChanged;

            //edtFontFamily.DataBindings.Add(nameof(edtFontFamily.EditValue), Helper, nameof(Helper.FontFamilyName), true);
            edtFontFamily.EditValue = Helper.FontFamilyName;
            edtFontFamily.Tag = FontHelper.FontFamilyProperty;
            edtFontFamily.EditValueChanged += (s, e) => { Helper.FontFamilyName = edtFontFamily.Text; changed++; };

            //edtFontSize.DataBindings.Add(nameof(edtFontSize.EditValue), Helper, nameof(Helper.Size), true);
            edtFontSize.EditValue = Helper.Size;
            edtFontSize.Tag = FontHelper.FontSizeProperty;
            edtFontSize.EditValueChanged += (s, e) => { Helper.Size = (int)edtFontFamily.EditValue; changed++; };

            //edtFontColor.DataBindings.Add(nameof(edtFontColor.EditValue), Helper, nameof(Helper.Color), true);
            edtFontColor.EditValue = Helper.Color;
            edtFontColor.Tag = FontHelper.FontColorProperty;
            edtFontColor.EditValueChanged += (s, e) => { Helper.Color = (Color)edtFontColor.EditValue; changed++; };

            //edtFontBold.DataBindings.Add(nameof(edtFontBold.EditValue), Helper, nameof(Helper.Bold), true);
            edtFontBold.EditValue = Helper.Bold;
            edtFontBold.Tag = FontHelper.FontStyleProperty;
            edtFontBold.EditValueChanged += (s, e) => { Helper.Bold = (bool)edtFontBold.EditValue; changed++; };

            //edtFontItalic.DataBindings.Add(nameof(edtFontItalic.EditValue), Helper, nameof(Helper.Italic), true);
            edtFontItalic.EditValue = Helper.Italic;
            edtFontItalic.Tag = FontHelper.FontStyleProperty;
            edtFontItalic.EditValueChanged += (s, e) => { Helper.Italic = (bool)edtFontItalic.EditValue; changed++; };

            //edtFontUnderline.DataBindings.Add(nameof(edtFontUnderline.EditValue), Helper, nameof(Helper.Underline), true);
            edtFontUnderline.EditValue = Helper.Underline;
            edtFontUnderline.Tag = FontHelper.FontStyleProperty;
            edtFontUnderline.EditValueChanged += (s, e) => { Helper.Underline = (bool)edtFontUnderline.EditValue; changed++; };

            //edtFontStrikeout.DataBindings.Add(nameof(edtFontStrikeout.EditValue), Helper, nameof(Helper.Strikeout), true);
            edtFontStrikeout.EditValue = Helper.Strikeout;
            edtFontStrikeout.Tag = FontHelper.FontStyleProperty;
            edtFontStrikeout.EditValueChanged += (s, e) => { Helper.Strikeout = (bool)edtFontStrikeout.EditValue; changed++; };

            if (readOnlyProperties != null)
                Lock(readOnlyProperties);
            else
                Helper_PropertyChanged(this, new PropertyChangedEventArgs(nameof(Helper.ReadOnlyProperties)));
        }

        private void Helper_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void Helper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Helper.ReadOnlyProperties))
            {
                foreach (Control c in Controls)
                {
                    c.Enabled = !Helper.ReadOnlyProperties.Contains($"{c.Tag}");
                }
            }
            edtPreview.Font = Helper.CreateFont();
            edtPreview.ForeColor = Helper.Color;
        }


        public void Lock(IEnumerable<string> readOnlyProperties = null) => Helper.Lock(readOnlyProperties);
        public void Unlock() => Helper.Unlock();

        public string PreviewText
        {
            get => edtPreview.Text;
            set => edtPreview.Text = value;
        }

        public override string ToString()
        {
            return Helper.ToString();
        }

        public Font GetFont() => Helper.CreateFont();
    }
}
