using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013SR;
using DevExpress.Utils.Serializing;

namespace VdO2013MainCore
{
    public partial class SiteReaderLinkedInfoControl : SiteReaderLinkedControl
    {
        private Type _ReaderType = null;
        private List<LayoutControlItem> _CustomItems = new List<LayoutControlItem>();

        public SiteReaderLinkedInfoControl()
        {
            InitializeComponent();

            this.Controls.Remove(this.layControl);
            this.SplitterFixedPane.Controls.Add(this.layControl);

            AgenziaNullText = "?";
            RagioneSocialeNullText = "?";
            TipologiaNullText = "?";
            ElementiNullText = "?";

            ReadOnly = true;
            this.btnAdditionalInfo.Click += (s, e) => { AdditionalInfoButtonClick?.Invoke(s, e); };

            //var i = AddLayout(layControl);
            var i = LayoutCentral.Default.FindByReference(layControl);
            if (i != null)
            {
                i.BeforeRestore += (sender, e) => { e.Cancel = this.Reader == null; };
                i.BeforeSave += (sender, e) => { e.Cancel = this.Reader == null; };
            }
        }

        public override IEnumerable<ISupportXtraSerializer> GetLayoutReferences()
        {
            try
            {
                return new ISupportXtraSerializer[] { layControl };
            }
            catch
            {
                return new ISupportXtraSerializer[] { };
            }
        }

        public override string GetLayoutName(ISupportXtraSerializer reference)
        {
            return Reader == null ? null : Path.Combine(Global.ConfigPath, Reader.ReaderName + "_" + TextRes.readerResultInfoLayoutFileName);
        }

        public Type ReaderType
        { 
            get { return _ReaderType; }
            set { if (value != null && !value.IsSubclassOf(typeof(SiteReader2))) return; _ReaderType = value; }
        }

        protected override void InternalReaderChanged()
        {
            base.InternalReaderChanged();
            if (this.Reader != null)
            {
                this.Agenzia = this.Reader.Agenzia;
                this.Numero = this.Reader.Numero;
                this.RagioneSociale = this.Reader.RagioneSociale;
                this.Tipologia = this.Reader.Tipologia;
                this.Elementi = this.Reader.Elementi;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DevExpress.XtraLayout.LayoutControl LayoutControl { get => this.layControl; }

        public object Agenzia { get { return edtAgenzia.EditValue; } set { edtAgenzia.EditValue = value; } }
        public object Numero { get { return "Not Implemented"; } set { } }
        public object RagioneSociale { get { return edtRagioneSociale.EditValue; } set { edtRagioneSociale.EditValue = value; } }
        public object Tipologia { get { return edtTipologia.EditValue; } set { edtTipologia.EditValue = value; } }
        public object Elementi { get { return edtElementi.EditValue; } set { edtElementi.EditValue = value; } }

        public string AgenziaNullText { get { return edtAgenzia.Properties.NullText; } set { edtAgenzia.Properties.NullText = value; } }
        public string RagioneSocialeNullText { get { return edtRagioneSociale.Properties.NullText; } set { edtRagioneSociale.Properties.NullText = value; } }
        public string TipologiaNullText { get { return edtTipologia.Properties.NullText; } set { edtTipologia.Properties.NullText = value; } }
        public string ElementiNullText { get { return edtElementi.Properties.NullText; } set { edtElementi.Properties.NullText = value; } }

        public string AgenziaText { get { return edtAgenzia.Text; } }
        public string RagioneSocialeText { get { return edtRagioneSociale.Text; } }
        public string TipologiaText { get { return edtTipologia.Text; } }
        public string ElementiText { get { return edtElementi.Text; } }

        public bool ReadOnly
        {
            get { return edtAgenzia.Properties.ReadOnly; }
            set
            {
                edtAgenzia.Properties.ReadOnly = value;
                edtRagioneSociale.Properties.ReadOnly = value;
                edtTipologia.Properties.ReadOnly = value;
                edtElementi.Properties.ReadOnly = value;
            }
        }

        public void Set(object agenzia, object ragioneSociale, object tipologia, object elementi)
        {
            Agenzia = agenzia?.ToString().NullIf(AgenziaNullText);
            RagioneSociale = ragioneSociale?.ToString().NullIf(RagioneSocialeNullText);
            Tipologia = tipologia?.ToString().NullIf(TipologiaNullText);
            Elementi = elementi?.ToString().NullIf(ElementiNullText);
        }
        
        public LayoutVisibility AddInfoVisible
        {
            get { return lciAdditionalInfo.Visibility; }
            set { lciAdditionalInfo.Visibility = value; }
        }

        private int AddInfoButtonInvocationCount()
        {
            if (AdditionalInfoButtonClick == null) return -1;
            Delegate[] il = AdditionalInfoButtonClick.GetInvocationList();
            return il != null ? il.Length : -1;
        }
        
        private LayoutVisibility GetInfoButtonInvocationVisibility()
        {
            if (lciAdditionalInfo.Visibility == LayoutVisibility.Always)
                return lciAdditionalInfo.Visibility;
            if (lciAdditionalInfo.Visibility == LayoutVisibility.Never && AddInfoButtonInvocationCount() > 0)
                return LayoutVisibility.OnlyInCustomization;
            return lciAdditionalInfo.Visibility;
        }

        public event EventHandler AdditionalInfoButtonClick;
        public void AdditionalInfoInvoke() { AdditionalInfoButtonClick?.Invoke(btnAdditionalInfo, EventArgs.Empty); }

        public LayoutType DefaultLayoutType
        {
            get { return layGroup.DefaultLayoutType; }
            set { layGroup.DefaultLayoutType = value; }
        }

        public IEnumerable<LayoutControlItem> CustomItems
        {
            get
            {
                return _CustomItems;
            }
        }
        public void AddCustomItem(string text, Control control, object tag, bool asLast = true)
        {
            InsertType it = DefaultLayoutType == LayoutType.Horizontal ? (asLast ? InsertType.Right : InsertType.Left) : (asLast ? InsertType.Bottom : InsertType.Top);
            _CustomItems.Add(new LayoutControlItem(layControl, control) { Text = text, Tag = tag, TextVisible = !string.IsNullOrEmpty(text) });
            layGroup.AddItem(text, _CustomItems[_CustomItems.Count - 1], it);
        }
        public void RemoveCustomItem(LayoutControlItem item)
        {
            if (_CustomItems.Contains(item))
            {
                layGroup.Remove(item);
                _CustomItems.Remove(item);
                item.Dispose();
                item = null;
            }
        }

        internal ISiteReader2Info GetReaderInfo()
        {
#if SiteReader2Factory
            return ReaderType != null ? VdO2013SR.SiteReader2Factory.GetInfo(ReaderType) : default(ISiteReader2Info);
#else
            throw new NotImplementedException();
#endif
        }
    }
}
