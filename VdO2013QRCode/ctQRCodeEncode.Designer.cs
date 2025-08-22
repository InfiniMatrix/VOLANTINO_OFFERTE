namespace VdO2013QRCode
{
    partial class ctQRCodeEncode
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
            this.components = new System.ComponentModel.Container();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.dlgPrint = new System.Windows.Forms.PrintDialog();
            this.docPrint = new System.Drawing.Printing.PrintDocument();
            this.pnlButtons = new DevExpress.XtraEditors.PanelControl();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnEncode = new DevExpress.XtraEditors.SimpleButton();
            this.pnlText = new DevExpress.XtraEditors.PanelControl();
            this.txtEncodeData = new DevExpress.XtraEditors.MemoEdit();
            this.lblEncodeData = new DevExpress.XtraEditors.LabelControl();
            this.pnlOptions = new DevExpress.XtraEditors.PanelControl();
            this.btnQRCodeBackColor = new DevExpress.XtraEditors.SimpleButton();
            this.lblQRCodeBackColor = new DevExpress.XtraEditors.LabelControl();
            this.btnQRCodeForeColor = new DevExpress.XtraEditors.SimpleButton();
            this.lblQRCodeForeColor = new DevExpress.XtraEditors.LabelControl();
            this.cboCharacterSet = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblCharacterSet = new DevExpress.XtraEditors.LabelControl();
            this.cboCorrectionLevel = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblCorrectionLevel = new DevExpress.XtraEditors.LabelControl();
            this.txtSize = new DevExpress.XtraEditors.SpinEdit();
            this.lblSize = new DevExpress.XtraEditors.LabelControl();
            this.cboVersion = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblVersion = new DevExpress.XtraEditors.LabelControl();
            this.cboEncoding = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblEncoding = new DevExpress.XtraEditors.LabelControl();
            this.pnlImage = new DevExpress.XtraEditors.PanelControl();
            this.picEncode = new DevExpress.XtraEditors.PictureEdit();
            this.barMgr = new DevExpress.XtraBars.BarManager(this.components);
            this.barMenu = new DevExpress.XtraBars.Bar();
            this.bmiFile = new DevExpress.XtraBars.BarSubItem();
            this.bmbEncode = new DevExpress.XtraBars.BarButtonItem();
            this.bmbSave = new DevExpress.XtraBars.BarButtonItem();
            this.bmbPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.stsInfo = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlText)).BeginInit();
            this.pnlText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncodeData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlOptions)).BeginInit();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboCharacterSet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCorrectionLevel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEncoding.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImage)).BeginInit();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEncode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).BeginInit();
            this.SuspendLayout();
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "openFileDialog1";
            // 
            // dlgPrint
            // 
            this.dlgPrint.UseEXDialog = true;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnPrint);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnEncode);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 311);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(480, 38);
            this.pnlButtons.TabIndex = 64;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(375, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 27);
            this.btnPrint.TabIndex = 50;
            this.btnPrint.Text = "Stampa";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(275, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 27);
            this.btnSave.TabIndex = 49;
            this.btnSave.Text = "Salva";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncode.Location = new System.Drawing.Point(174, 6);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(94, 27);
            this.btnEncode.TabIndex = 48;
            this.btnEncode.Text = "Codifica";
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // pnlText
            // 
            this.pnlText.Controls.Add(this.txtEncodeData);
            this.pnlText.Controls.Add(this.lblEncodeData);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlText.Location = new System.Drawing.Point(0, 249);
            this.pnlText.Name = "pnlText";
            this.pnlText.Size = new System.Drawing.Size(480, 62);
            this.pnlText.TabIndex = 65;
            // 
            // txtEncodeData
            // 
            this.txtEncodeData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncodeData.EditValue = "...";
            this.txtEncodeData.Location = new System.Drawing.Point(81, 5);
            this.txtEncodeData.Name = "txtEncodeData";
            this.txtEncodeData.Size = new System.Drawing.Size(388, 53);
            this.txtEncodeData.TabIndex = 48;
            // 
            // lblEncodeData
            // 
            this.lblEncodeData.Location = new System.Drawing.Point(3, 5);
            this.lblEncodeData.Name = "lblEncodeData";
            this.lblEncodeData.Size = new System.Drawing.Size(27, 13);
            this.lblEncodeData.TabIndex = 47;
            this.lblEncodeData.Text = "Testo";
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.btnQRCodeBackColor);
            this.pnlOptions.Controls.Add(this.lblQRCodeBackColor);
            this.pnlOptions.Controls.Add(this.btnQRCodeForeColor);
            this.pnlOptions.Controls.Add(this.lblQRCodeForeColor);
            this.pnlOptions.Controls.Add(this.cboCharacterSet);
            this.pnlOptions.Controls.Add(this.lblCharacterSet);
            this.pnlOptions.Controls.Add(this.cboCorrectionLevel);
            this.pnlOptions.Controls.Add(this.lblCorrectionLevel);
            this.pnlOptions.Controls.Add(this.txtSize);
            this.pnlOptions.Controls.Add(this.lblSize);
            this.pnlOptions.Controls.Add(this.cboVersion);
            this.pnlOptions.Controls.Add(this.lblVersion);
            this.pnlOptions.Controls.Add(this.cboEncoding);
            this.pnlOptions.Controls.Add(this.lblEncoding);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlOptions.Location = new System.Drawing.Point(232, 22);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(248, 227);
            this.pnlOptions.TabIndex = 66;
            // 
            // btnQRCodeBackColor
            // 
            this.btnQRCodeBackColor.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnQRCodeBackColor.Appearance.Options.UseForeColor = true;
            this.btnQRCodeBackColor.Location = new System.Drawing.Point(216, 86);
            this.btnQRCodeBackColor.Name = "btnQRCodeBackColor";
            this.btnQRCodeBackColor.Size = new System.Drawing.Size(23, 23);
            this.btnQRCodeBackColor.TabIndex = 73;
            // 
            // lblQRCodeBackColor
            // 
            this.lblQRCodeBackColor.Location = new System.Drawing.Point(176, 96);
            this.lblQRCodeBackColor.Name = "lblQRCodeBackColor";
            this.lblQRCodeBackColor.Size = new System.Drawing.Size(34, 13);
            this.lblQRCodeBackColor.TabIndex = 75;
            this.lblQRCodeBackColor.Text = "Sfondo";
            // 
            // btnQRCodeForeColor
            // 
            this.btnQRCodeForeColor.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnQRCodeForeColor.Appearance.Options.UseForeColor = true;
            this.btnQRCodeForeColor.Location = new System.Drawing.Point(81, 87);
            this.btnQRCodeForeColor.Name = "btnQRCodeForeColor";
            this.btnQRCodeForeColor.Size = new System.Drawing.Size(23, 23);
            this.btnQRCodeForeColor.TabIndex = 72;
            // 
            // lblQRCodeForeColor
            // 
            this.lblQRCodeForeColor.Location = new System.Drawing.Point(0, 96);
            this.lblQRCodeForeColor.Name = "lblQRCodeForeColor";
            this.lblQRCodeForeColor.Size = new System.Drawing.Size(31, 13);
            this.lblQRCodeForeColor.TabIndex = 74;
            this.lblQRCodeForeColor.Text = "Colore";
            // 
            // cboCharacterSet
            // 
            this.cboCharacterSet.Location = new System.Drawing.Point(81, 58);
            this.cboCharacterSet.Name = "cboCharacterSet";
            this.cboCharacterSet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCharacterSet.Size = new System.Drawing.Size(157, 20);
            this.cboCharacterSet.TabIndex = 71;
            // 
            // lblCharacterSet
            // 
            this.lblCharacterSet.Location = new System.Drawing.Point(3, 61);
            this.lblCharacterSet.Name = "lblCharacterSet";
            this.lblCharacterSet.Size = new System.Drawing.Size(67, 13);
            this.lblCharacterSet.TabIndex = 70;
            this.lblCharacterSet.Text = "Character Set";
            // 
            // cboCorrectionLevel
            // 
            this.cboCorrectionLevel.Location = new System.Drawing.Point(81, 116);
            this.cboCorrectionLevel.Name = "cboCorrectionLevel";
            this.cboCorrectionLevel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCorrectionLevel.Size = new System.Drawing.Size(95, 20);
            this.cboCorrectionLevel.TabIndex = 69;
            // 
            // lblCorrectionLevel
            // 
            this.lblCorrectionLevel.Location = new System.Drawing.Point(3, 119);
            this.lblCorrectionLevel.Name = "lblCorrectionLevel";
            this.lblCorrectionLevel.Size = new System.Drawing.Size(52, 13);
            this.lblCorrectionLevel.TabIndex = 68;
            this.lblCorrectionLevel.Text = "Correzione";
            // 
            // txtSize
            // 
            this.txtSize.EditValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.txtSize.Location = new System.Drawing.Point(81, 143);
            this.txtSize.Name = "txtSize";
            this.txtSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSize.Properties.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtSize.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSize.Size = new System.Drawing.Size(95, 20);
            this.txtSize.TabIndex = 76;
            // 
            // lblSize
            // 
            this.lblSize.Location = new System.Drawing.Point(3, 146);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(54, 13);
            this.lblSize.TabIndex = 66;
            this.lblSize.Text = "Dimensione";
            // 
            // cboVersion
            // 
            this.cboVersion.Location = new System.Drawing.Point(81, 31);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboVersion.Size = new System.Drawing.Size(157, 20);
            this.cboVersion.TabIndex = 65;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(3, 35);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(35, 13);
            this.lblVersion.TabIndex = 64;
            this.lblVersion.Text = "Version";
            // 
            // cboEncoding
            // 
            this.cboEncoding.Location = new System.Drawing.Point(81, 4);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboEncoding.Size = new System.Drawing.Size(157, 20);
            this.cboEncoding.TabIndex = 63;
            // 
            // lblEncoding
            // 
            this.lblEncoding.Location = new System.Drawing.Point(3, 8);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(43, 13);
            this.lblEncoding.TabIndex = 62;
            this.lblEncoding.Text = "Encoding";
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.picEncode);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 22);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(232, 227);
            this.pnlImage.TabIndex = 67;
            // 
            // picEncode
            // 
            this.picEncode.Location = new System.Drawing.Point(6, 4);
            this.picEncode.Name = "picEncode";
            this.picEncode.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.picEncode.Properties.Appearance.Options.UseBackColor = true;
            this.picEncode.Size = new System.Drawing.Size(220, 220);
            this.picEncode.TabIndex = 43;
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
            this.bmiFile,
            this.bmbEncode,
            this.bmbSave,
            this.bmbPrint,
            this.stsInfo});
            this.barMgr.MainMenu = this.barMenu;
            this.barMgr.MaxItemId = 6;
            this.barMgr.StatusBar = this.barStatus;
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
            // bmiFile
            // 
            this.bmiFile.Caption = "File";
            this.bmiFile.Id = 1;
            this.bmiFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bmbEncode),
            new DevExpress.XtraBars.LinkPersistInfo(this.bmbSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.bmbPrint)});
            this.bmiFile.Name = "bmiFile";
            // 
            // bmbEncode
            // 
            this.bmbEncode.Caption = "Codifica";
            this.bmbEncode.Id = 2;
            this.bmbEncode.Name = "bmbEncode";
            this.bmbEncode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bmbEncode_ItemClick);
            // 
            // bmbSave
            // 
            this.bmbSave.Caption = "Salva";
            this.bmbSave.Id = 3;
            this.bmbSave.Name = "bmbSave";
            this.bmbSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bmbSave_ItemClick);
            // 
            // bmbPrint
            // 
            this.bmbPrint.Caption = "Stampa";
            this.bmbPrint.Id = 4;
            this.bmbPrint.Name = "bmbPrint";
            this.bmbPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bmbPrint_ItemClick);
            // 
            // barStatus
            // 
            this.barStatus.BarName = "Status bar";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockRow = 0;
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
            this.stsInfo.Id = 5;
            this.stsInfo.Name = "stsInfo";
            this.stsInfo.TextAlignment = System.Drawing.StringAlignment.Near;
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 349);
            this.barDockControlBottom.Size = new System.Drawing.Size(480, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 327);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(480, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 327);
            // 
            // ctQRCodeEncode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.pnlText);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ctQRCodeEncode";
            this.Size = new System.Drawing.Size(480, 374);
            this.Load += new System.EventHandler(this.ctQRCodeEncode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlText)).EndInit();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEncodeData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlOptions)).EndInit();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboCharacterSet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCorrectionLevel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEncoding.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImage)).EndInit();
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEncode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMgr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.ColorDialog dlgColor;
        private System.Windows.Forms.PrintDialog dlgPrint;
        private System.Drawing.Printing.PrintDocument docPrint;
        private DevExpress.XtraEditors.PanelControl pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnEncode;
        private DevExpress.XtraEditors.PanelControl pnlText;
        private DevExpress.XtraEditors.MemoEdit txtEncodeData;
        private DevExpress.XtraEditors.LabelControl lblEncodeData;
        private DevExpress.XtraEditors.PanelControl pnlOptions;
        private DevExpress.XtraEditors.LabelControl lblQRCodeBackColor;
        private DevExpress.XtraEditors.LabelControl lblQRCodeForeColor;
        private DevExpress.XtraEditors.SimpleButton btnQRCodeBackColor;
        private DevExpress.XtraEditors.SimpleButton btnQRCodeForeColor;
        private DevExpress.XtraEditors.ComboBoxEdit cboCharacterSet;
        private DevExpress.XtraEditors.LabelControl lblCharacterSet;
        private DevExpress.XtraEditors.ComboBoxEdit cboCorrectionLevel;
        private DevExpress.XtraEditors.LabelControl lblCorrectionLevel;
        private DevExpress.XtraEditors.LabelControl lblSize;
        private DevExpress.XtraEditors.ComboBoxEdit cboVersion;
        private DevExpress.XtraEditors.LabelControl lblVersion;
        private DevExpress.XtraEditors.ComboBoxEdit cboEncoding;
        private DevExpress.XtraEditors.LabelControl lblEncoding;
        private DevExpress.XtraEditors.PanelControl pnlImage;
        private DevExpress.XtraEditors.PictureEdit picEncode;
        private DevExpress.XtraEditors.SpinEdit txtSize;
        private DevExpress.XtraBars.BarManager barMgr;
        private DevExpress.XtraBars.Bar barMenu;
        private DevExpress.XtraBars.BarSubItem bmiFile;
        private DevExpress.XtraBars.BarButtonItem bmbEncode;
        private DevExpress.XtraBars.BarButtonItem bmbSave;
        private DevExpress.XtraBars.BarButtonItem bmbPrint;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarStaticItem stsInfo;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}
