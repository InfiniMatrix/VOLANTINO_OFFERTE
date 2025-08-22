namespace VdO2013QRCode
{
    partial class ctQRCodeDecode
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlImage = new DevExpress.XtraEditors.PanelControl();
            this.picDecode = new DevExpress.XtraEditors.PictureEdit();
            this.pnlButtons = new DevExpress.XtraEditors.PanelControl();
            this.btnOpen = new DevExpress.XtraEditors.SimpleButton();
            this.btnDecode = new DevExpress.XtraEditors.SimpleButton();
            this.pnlText = new DevExpress.XtraEditors.PanelControl();
            this.txtDecodedData = new DevExpress.XtraEditors.MemoEdit();
            this.lblDecodedData = new DevExpress.XtraEditors.LabelControl();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.barMgr = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barMenu = new DevExpress.XtraBars.Bar();
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.stsInfo = new DevExpress.XtraBars.BarStaticItem();
            this.bmiFile = new DevExpress.XtraBars.BarSubItem();
            this.bmbOpen = new DevExpress.XtraBars.BarButtonItem();
            this.bmbDecode = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImage)).BeginInit();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDecode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlText)).BeginInit();
            this.pnlText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDecodedData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.picDecode);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 22);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(480, 353);
            this.pnlImage.TabIndex = 2;
            // 
            // picDecode
            // 
            this.picDecode.Location = new System.Drawing.Point(5, 5);
            this.picDecode.Name = "picDecode";
            this.picDecode.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.picDecode.Properties.Appearance.Options.UseBackColor = true;
            this.picDecode.Size = new System.Drawing.Size(200, 200);
            this.picDecode.TabIndex = 8;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOpen);
            this.pnlButtons.Controls.Add(this.btnDecode);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 337);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(480, 38);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(266, 6);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(94, 27);
            this.btnOpen.TabIndex = 14;
            this.btnOpen.Text = "Apri";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(372, 6);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(94, 27);
            this.btnDecode.TabIndex = 13;
            this.btnDecode.Text = "Decodifica";
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // pnlText
            // 
            this.pnlText.Controls.Add(this.txtDecodedData);
            this.pnlText.Controls.Add(this.lblDecodedData);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlText.Location = new System.Drawing.Point(0, 275);
            this.pnlText.Name = "pnlText";
            this.pnlText.Size = new System.Drawing.Size(480, 62);
            this.pnlText.TabIndex = 4;
            // 
            // txtDecodedData
            // 
            this.txtDecodedData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDecodedData.Location = new System.Drawing.Point(78, 5);
            this.txtDecodedData.Name = "txtDecodedData";
            this.txtDecodedData.Size = new System.Drawing.Size(388, 53);
            this.txtDecodedData.TabIndex = 13;
            // 
            // lblDecodedData
            // 
            this.lblDecodedData.Location = new System.Drawing.Point(3, 5);
            this.lblDecodedData.Name = "lblDecodedData";
            this.lblDecodedData.Size = new System.Drawing.Size(27, 13);
            this.lblDecodedData.TabIndex = 12;
            this.lblDecodedData.Text = "Testo";
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            // 
            // barMgr
            // 
            this.barMgr.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMenu,
            this.barStatus});
            this.barMgr.DockControls.Add(this.barDockControlTop);
            this.barMgr.DockControls.Add(this.barDockControlBottom);
            this.barMgr.DockControls.Add(this.barDockControlLeft);
            this.barMgr.DockControls.Add(this.barDockControlRight);
            this.barMgr.Form = this;
            this.barMgr.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.stsInfo,
            this.bmiFile,
            this.bmbOpen,
            this.bmbDecode});
            this.barMgr.MainMenu = this.barMenu;
            this.barMgr.MaxItemId = 4;
            this.barMgr.StatusBar = this.barStatus;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(480, 22);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 375);
            this.barDockControlBottom.Size = new System.Drawing.Size(480, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 353);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(480, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 353);
            // 
            // barMenu
            // 
            this.barMenu.BarName = "Main menu";
            this.barMenu.DockCol = 0;
            this.barMenu.DockRow = 0;
            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bmiFile)});
            this.barMenu.OptionsBar.MultiLine = true;
            this.barMenu.OptionsBar.UseWholeRow = true;
            this.barMenu.Text = "Main menu";
            // 
            // barStatus
            // 
            this.barStatus.BarName = "Status bar";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.stsInfo)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status bar";
            // 
            // stsInfo
            // 
            this.stsInfo.Caption = "Info";
            this.stsInfo.Id = 0;
            this.stsInfo.Name = "stsInfo";
            this.stsInfo.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bmiFile
            // 
            this.bmiFile.Caption = "File";
            this.bmiFile.Id = 1;
            this.bmiFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bmbOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.bmbDecode)});
            this.bmiFile.Name = "bmiFile";
            // 
            // bmbOpen
            // 
            this.bmbOpen.Caption = "Apri";
            this.bmbOpen.Id = 2;
            this.bmbOpen.Name = "bmbOpen";
            // 
            // bmbDecode
            // 
            this.bmbDecode.Caption = "Decodifica";
            this.bmbDecode.Id = 3;
            this.bmbDecode.Name = "bmbDecode";
            // 
            // ctQRCodeDecode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlText);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ctQRCodeDecode";
            this.Size = new System.Drawing.Size(480, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pnlImage)).EndInit();
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDecode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlText)).EndInit();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDecodedData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlImage;
        private DevExpress.XtraEditors.PictureEdit picDecode;
        private DevExpress.XtraEditors.PanelControl pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnOpen;
        private DevExpress.XtraEditors.SimpleButton btnDecode;
        private DevExpress.XtraEditors.PanelControl pnlText;
        private DevExpress.XtraEditors.MemoEdit txtDecodedData;
        private DevExpress.XtraEditors.LabelControl lblDecodedData;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private DevExpress.XtraBars.BarManager barMgr;
        private DevExpress.XtraBars.Bar barMenu;
        private DevExpress.XtraBars.BarSubItem bmiFile;
        private DevExpress.XtraBars.BarButtonItem bmbOpen;
        private DevExpress.XtraBars.BarButtonItem bmbDecode;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarStaticItem stsInfo;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}
