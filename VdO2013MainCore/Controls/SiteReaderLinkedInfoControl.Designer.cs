namespace VdO2013MainCore
{
    public partial class SiteReaderLinkedInfoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteReaderLinkedInfoControl));
            this.layControl = new DevExpress.XtraLayout.LayoutControl();
            this.btnAdditionalInfo = new DevExpress.XtraEditors.SimpleButton();
            this.edtElementi = new DevExpress.XtraEditors.TextEdit();
            this.edtTipologia = new DevExpress.XtraEditors.TextEdit();
            this.edtRagioneSociale = new DevExpress.XtraEditors.TextEdit();
            this.edtAgenzia = new DevExpress.XtraEditors.TextEdit();
            this.layGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciAgenzia = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciRagioneSociale = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTipologia = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciElementi = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAdditionalInfo = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layControl)).BeginInit();
            this.layControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtElementi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTipologia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtRagioneSociale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtAgenzia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAgenzia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRagioneSociale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTipologia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciElementi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAdditionalInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // layControl
            // 
            this.layControl.AutoScroll = false;
            this.layControl.Controls.Add(this.btnAdditionalInfo);
            this.layControl.Controls.Add(this.edtElementi);
            this.layControl.Controls.Add(this.edtTipologia);
            this.layControl.Controls.Add(this.edtRagioneSociale);
            this.layControl.Controls.Add(this.edtAgenzia);
            this.layControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layControl.Location = new System.Drawing.Point(0, 0);
            this.layControl.Name = "layControl";
            this.layControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(634, 417, 1438, 1052);
            this.layControl.OptionsCustomizationForm.ShowPropertyGrid = true;
            this.layControl.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignMode.AutoSize;
            this.layControl.Root = this.layGroup;
            this.layControl.Size = new System.Drawing.Size(800, 48);
            this.layControl.TabIndex = 0;
            // 
            // btnAdditionalInfo
            // 
            this.btnAdditionalInfo.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdditionalInfo.ImageOptions.Image")));
            this.btnAdditionalInfo.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAdditionalInfo.Location = new System.Drawing.Point(768, 8);
            this.btnAdditionalInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdditionalInfo.Name = "btnAdditionalInfo";
            this.btnAdditionalInfo.Size = new System.Drawing.Size(24, 32);
            this.btnAdditionalInfo.StyleController = this.layControl;
            this.btnAdditionalInfo.TabIndex = 8;
            this.btnAdditionalInfo.ToolTip = "Impostazioni";
            // 
            // edtElementi
            // 
            this.edtElementi.Location = new System.Drawing.Point(645, 8);
            this.edtElementi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edtElementi.Name = "edtElementi";
            this.edtElementi.Size = new System.Drawing.Size(115, 28);
            this.edtElementi.StyleController = this.layControl;
            this.edtElementi.TabIndex = 7;
            // 
            // edtTipologia
            // 
            this.edtTipologia.Location = new System.Drawing.Point(466, 8);
            this.edtTipologia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edtTipologia.Name = "edtTipologia";
            this.edtTipologia.Size = new System.Drawing.Size(108, 28);
            this.edtTipologia.StyleController = this.layControl;
            this.edtTipologia.TabIndex = 6;
            // 
            // edtRagioneSociale
            // 
            this.edtRagioneSociale.Location = new System.Drawing.Point(313, 8);
            this.edtRagioneSociale.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edtRagioneSociale.Name = "edtRagioneSociale";
            this.edtRagioneSociale.Size = new System.Drawing.Size(75, 28);
            this.edtRagioneSociale.StyleController = this.layControl;
            this.edtRagioneSociale.TabIndex = 5;
            // 
            // edtAgenzia
            // 
            this.edtAgenzia.Location = new System.Drawing.Point(68, 8);
            this.edtAgenzia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edtAgenzia.Name = "edtAgenzia";
            this.edtAgenzia.Size = new System.Drawing.Size(122, 28);
            this.edtAgenzia.StyleController = this.layControl;
            this.edtAgenzia.TabIndex = 4;
            // 
            // layGroup
            // 
            this.layGroup.AllowHide = false;
            this.layGroup.CustomizationFormText = "Info";
            this.layGroup.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal;
            this.layGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layGroup.GroupBordersVisible = false;
            this.layGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciAgenzia,
            this.lciRagioneSociale,
            this.lciTipologia,
            this.lciElementi,
            this.lciAdditionalInfo});
            this.layGroup.Name = "Root";
            this.layGroup.OptionsItemText.TextToControlDistance = 5;
            this.layGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layGroup.Size = new System.Drawing.Size(800, 48);
            this.layGroup.TextLocation = DevExpress.Utils.Locations.Left;
            this.layGroup.TextVisible = false;
            // 
            // lciAgenzia
            // 
            this.lciAgenzia.BestFitWeight = 22;
            this.lciAgenzia.Control = this.edtAgenzia;
            this.lciAgenzia.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lciAgenzia.CustomizationFormText = "Agenzia";
            this.lciAgenzia.Location = new System.Drawing.Point(0, 0);
            this.lciAgenzia.Name = "lciAgenzia";
            this.lciAgenzia.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciAgenzia.Size = new System.Drawing.Size(190, 40);
            this.lciAgenzia.Text = "Agenzia";
            this.lciAgenzia.TextSize = new System.Drawing.Size(56, 19);
            // 
            // lciRagioneSociale
            // 
            this.lciRagioneSociale.BestFitWeight = 22;
            this.lciRagioneSociale.Control = this.edtRagioneSociale;
            this.lciRagioneSociale.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lciRagioneSociale.CustomizationFormText = "Ragione Sociale";
            this.lciRagioneSociale.Location = new System.Drawing.Point(190, 0);
            this.lciRagioneSociale.Name = "lciRagioneSociale";
            this.lciRagioneSociale.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciRagioneSociale.Size = new System.Drawing.Size(198, 40);
            this.lciRagioneSociale.Text = "Ragione Sociale";
            this.lciRagioneSociale.TextSize = new System.Drawing.Size(111, 19);
            // 
            // lciTipologia
            // 
            this.lciTipologia.BestFitWeight = 22;
            this.lciTipologia.Control = this.edtTipologia;
            this.lciTipologia.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lciTipologia.CustomizationFormText = "Tipologia";
            this.lciTipologia.Location = new System.Drawing.Point(388, 0);
            this.lciTipologia.Name = "lciTipologia";
            this.lciTipologia.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciTipologia.Size = new System.Drawing.Size(186, 40);
            this.lciTipologia.Text = "Tipologia";
            this.lciTipologia.TextSize = new System.Drawing.Size(66, 19);
            this.lciTipologia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciElementi
            // 
            this.lciElementi.BestFitWeight = 22;
            this.lciElementi.Control = this.edtElementi;
            this.lciElementi.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lciElementi.CustomizationFormText = "Elementi";
            this.lciElementi.Location = new System.Drawing.Point(574, 0);
            this.lciElementi.Name = "lciElementi";
            this.lciElementi.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciElementi.Size = new System.Drawing.Size(186, 40);
            this.lciElementi.Text = "Elementi";
            this.lciElementi.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciElementi.TextSize = new System.Drawing.Size(61, 19);
            this.lciElementi.TextToControlDistance = 2;
            // 
            // lciAdditionalInfo
            // 
            this.lciAdditionalInfo.BestFitWeight = 12;
            this.lciAdditionalInfo.Control = this.btnAdditionalInfo;
            this.lciAdditionalInfo.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lciAdditionalInfo.CustomizationFormText = "Impostazioni";
            this.lciAdditionalInfo.Location = new System.Drawing.Point(760, 0);
            this.lciAdditionalInfo.Name = "lciAdditionalInfo";
            this.lciAdditionalInfo.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciAdditionalInfo.Size = new System.Drawing.Size(32, 40);
            this.lciAdditionalInfo.Text = "Impostazioni";
            this.lciAdditionalInfo.TextSize = new System.Drawing.Size(0, 0);
            this.lciAdditionalInfo.TextVisible = false;
            this.lciAdditionalInfo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.OnlyInCustomization;
            // 
            // SiteReaderLinkedInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layControl);
            this.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.Name = "SiteReaderLinkedInfoControl";
            this.Size = new System.Drawing.Size(800, 48);
            this.SplitterCustomPane.Text = "CustomPane";
            this.SplitterFixedPane.Text = "FixedPane";
            this.Controls.SetChildIndex(this.layControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.layControl)).EndInit();
            this.layControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edtElementi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTipologia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtRagioneSociale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtAgenzia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAgenzia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRagioneSociale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTipologia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciElementi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAdditionalInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layControl;
        private DevExpress.XtraLayout.LayoutControlGroup layGroup;
        private DevExpress.XtraEditors.TextEdit edtElementi;
        private DevExpress.XtraEditors.TextEdit edtTipologia;
        private DevExpress.XtraEditors.TextEdit edtRagioneSociale;
        private DevExpress.XtraEditors.TextEdit edtAgenzia;
        private DevExpress.XtraLayout.LayoutControlItem lciAgenzia;
        private DevExpress.XtraLayout.LayoutControlItem lciRagioneSociale;
        private DevExpress.XtraLayout.LayoutControlItem lciTipologia;
        private DevExpress.XtraLayout.LayoutControlItem lciElementi;
        private DevExpress.XtraEditors.SimpleButton btnAdditionalInfo;
        private DevExpress.XtraLayout.LayoutControlItem lciAdditionalInfo;
    }
}
