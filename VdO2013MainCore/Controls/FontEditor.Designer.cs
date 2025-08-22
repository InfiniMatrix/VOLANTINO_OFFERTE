namespace VdO2013MainCore.Controls
{
    partial class FontEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontEditor));
            this.edtFontFamily = new DevExpress.XtraEditors.FontEdit();
            this.edtFontColor = new DevExpress.XtraEditors.ColorPickEdit();
            this.edtPreview = new DevExpress.XtraEditors.MemoEdit();
            this.edtFontSize = new DevExpress.XtraEditors.SpinEdit();
            this.edtFontBold = new DevExpress.XtraEditors.CheckEdit();
            this.lblFontFamily = new DevExpress.XtraEditors.LabelControl();
            this.lblFontSize = new DevExpress.XtraEditors.LabelControl();
            this.lblFontColor = new DevExpress.XtraEditors.LabelControl();
            this.edtFontItalic = new DevExpress.XtraEditors.CheckEdit();
            this.edtFontUnderline = new DevExpress.XtraEditors.CheckEdit();
            this.edtFontStrikeout = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontFamily.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPreview.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontBold.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontItalic.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontUnderline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontStrikeout.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // edtFontFamily
            // 
            this.edtFontFamily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFontFamily.Location = new System.Drawing.Point(140, 5);
            this.edtFontFamily.Name = "edtFontFamily";
            this.edtFontFamily.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtFontFamily.Size = new System.Drawing.Size(330, 26);
            this.edtFontFamily.TabIndex = 0;
            // 
            // edtFontColor
            // 
            this.edtFontColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFontColor.EditValue = System.Drawing.Color.Empty;
            this.edtFontColor.Location = new System.Drawing.Point(140, 69);
            this.edtFontColor.Name = "edtFontColor";
            this.edtFontColor.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.edtFontColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtFontColor.Size = new System.Drawing.Size(330, 26);
            this.edtFontColor.TabIndex = 1;
            // 
            // edtPreview
            // 
            this.edtPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtPreview.EditValue = resources.GetString("edtPreview.EditValue");
            this.edtPreview.Location = new System.Drawing.Point(9, 126);
            this.edtPreview.Name = "edtPreview";
            this.edtPreview.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.edtPreview.Properties.Appearance.Options.UseBackColor = true;
            this.edtPreview.Properties.ReadOnly = true;
            this.edtPreview.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.edtPreview.Size = new System.Drawing.Size(461, 159);
            this.edtPreview.TabIndex = 2;
            // 
            // edtFontSize
            // 
            this.edtFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtFontSize.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.edtFontSize.Location = new System.Drawing.Point(140, 37);
            this.edtFontSize.Name = "edtFontSize";
            this.edtFontSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtFontSize.Size = new System.Drawing.Size(330, 26);
            this.edtFontSize.TabIndex = 3;
            // 
            // edtFontBold
            // 
            this.edtFontBold.Location = new System.Drawing.Point(9, 97);
            this.edtFontBold.Name = "edtFontBold";
            this.edtFontBold.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.edtFontBold.Properties.Appearance.Options.UseFont = true;
            this.edtFontBold.Properties.Caption = "Grassetto";
            this.edtFontBold.Size = new System.Drawing.Size(107, 23);
            this.edtFontBold.TabIndex = 4;
            // 
            // lblFontFamily
            // 
            this.lblFontFamily.Location = new System.Drawing.Point(9, 8);
            this.lblFontFamily.Name = "lblFontFamily";
            this.lblFontFamily.Size = new System.Drawing.Size(31, 19);
            this.lblFontFamily.TabIndex = 5;
            this.lblFontFamily.Text = "Font";
            // 
            // lblFontSize
            // 
            this.lblFontSize.Location = new System.Drawing.Point(9, 44);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(73, 19);
            this.lblFontSize.TabIndex = 6;
            this.lblFontSize.Text = "Grandezza";
            // 
            // lblFontColor
            // 
            this.lblFontColor.Location = new System.Drawing.Point(9, 72);
            this.lblFontColor.Name = "lblFontColor";
            this.lblFontColor.Size = new System.Drawing.Size(46, 19);
            this.lblFontColor.TabIndex = 7;
            this.lblFontColor.Text = "Colore";
            // 
            // edtFontItalic
            // 
            this.edtFontItalic.Location = new System.Drawing.Point(140, 97);
            this.edtFontItalic.Name = "edtFontItalic";
            this.edtFontItalic.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Italic);
            this.edtFontItalic.Properties.Appearance.Options.UseFont = true;
            this.edtFontItalic.Properties.Caption = "Corsivo";
            this.edtFontItalic.Size = new System.Drawing.Size(79, 23);
            this.edtFontItalic.TabIndex = 8;
            // 
            // edtFontUnderline
            // 
            this.edtFontUnderline.Location = new System.Drawing.Point(254, 97);
            this.edtFontUnderline.Name = "edtFontUnderline";
            this.edtFontUnderline.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.edtFontUnderline.Properties.Appearance.Options.UseFont = true;
            this.edtFontUnderline.Properties.Caption = "Sottolineato";
            this.edtFontUnderline.Size = new System.Drawing.Size(107, 23);
            this.edtFontUnderline.TabIndex = 9;
            // 
            // edtFontStrikeout
            // 
            this.edtFontStrikeout.Location = new System.Drawing.Point(397, 97);
            this.edtFontStrikeout.Name = "edtFontStrikeout";
            this.edtFontStrikeout.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Strikeout);
            this.edtFontStrikeout.Properties.Appearance.Options.UseFont = true;
            this.edtFontStrikeout.Properties.Caption = "Barrato";
            this.edtFontStrikeout.Size = new System.Drawing.Size(73, 23);
            this.edtFontStrikeout.TabIndex = 10;
            // 
            // FontEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.edtFontStrikeout);
            this.Controls.Add(this.edtFontUnderline);
            this.Controls.Add(this.edtFontItalic);
            this.Controls.Add(this.lblFontColor);
            this.Controls.Add(this.lblFontSize);
            this.Controls.Add(this.lblFontFamily);
            this.Controls.Add(this.edtFontBold);
            this.Controls.Add(this.edtFontSize);
            this.Controls.Add(this.edtPreview);
            this.Controls.Add(this.edtFontColor);
            this.Controls.Add(this.edtFontFamily);
            this.MinimumSize = new System.Drawing.Size(480, 290);
            this.Name = "FontEditor";
            this.Size = new System.Drawing.Size(480, 290);
            ((System.ComponentModel.ISupportInitialize)(this.edtFontFamily.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPreview.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontBold.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontItalic.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontUnderline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFontStrikeout.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.FontEdit edtFontFamily;
        private DevExpress.XtraEditors.ColorPickEdit edtFontColor;
        private DevExpress.XtraEditors.MemoEdit edtPreview;
        private DevExpress.XtraEditors.SpinEdit edtFontSize;
        private DevExpress.XtraEditors.CheckEdit edtFontBold;
        private DevExpress.XtraEditors.LabelControl lblFontFamily;
        private DevExpress.XtraEditors.LabelControl lblFontSize;
        private DevExpress.XtraEditors.LabelControl lblFontColor;
        private DevExpress.XtraEditors.CheckEdit edtFontItalic;
        private DevExpress.XtraEditors.CheckEdit edtFontUnderline;
        private DevExpress.XtraEditors.CheckEdit edtFontStrikeout;
    }
}
