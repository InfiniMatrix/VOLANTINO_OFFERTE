namespace VdO2013LicenseProviderUtil
{
    partial class fmLogin
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.edtContext = new DevExpress.XtraEditors.TextEdit();
            this.edtPassword = new DevExpress.XtraEditors.TextEdit();
            this.edtLogin = new DevExpress.XtraEditors.TextEdit();
            this.lcgMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciLogin = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciGroup = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtContext.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnOk);
            this.layoutControl1.Controls.Add(this.btnCancel);
            this.layoutControl1.Controls.Add(this.edtContext);
            this.layoutControl1.Controls.Add(this.edtPassword);
            this.layoutControl1.Controls.Add(this.edtLogin);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.lcgMain;
            this.layoutControl1.Size = new System.Drawing.Size(252, 107);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "ctlLayout";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(128, 78);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(118, 22);
            this.btnOk.StyleController = this.layoutControl1;
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Accedi";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(6, 78);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 22);
            this.btnCancel.StyleController = this.layoutControl1;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Annulla";
            // 
            // edtContext
            // 
            this.edtContext.Location = new System.Drawing.Point(55, 54);
            this.edtContext.Name = "edtContext";
            this.edtContext.Size = new System.Drawing.Size(191, 20);
            this.edtContext.StyleController = this.layoutControl1;
            this.edtContext.TabIndex = 6;
            this.edtContext.EditValueChanged += new System.EventHandler(this.EditsValueChanged);
            // 
            // edtPassword
            // 
            this.edtPassword.Location = new System.Drawing.Point(55, 30);
            this.edtPassword.Name = "edtPassword";
            this.edtPassword.Properties.UseSystemPasswordChar = true;
            this.edtPassword.Size = new System.Drawing.Size(191, 20);
            this.edtPassword.StyleController = this.layoutControl1;
            this.edtPassword.TabIndex = 5;
            this.edtPassword.EditValueChanged += new System.EventHandler(this.EditsValueChanged);
            // 
            // edtLogin
            // 
            this.edtLogin.Location = new System.Drawing.Point(55, 6);
            this.edtLogin.Name = "edtLogin";
            this.edtLogin.Size = new System.Drawing.Size(191, 20);
            this.edtLogin.StyleController = this.layoutControl1;
            this.edtLogin.TabIndex = 4;
            this.edtLogin.EditValueChanged += new System.EventHandler(this.EditsValueChanged);
            // 
            // lcgMain
            // 
            this.lcgMain.CustomizationFormText = "lcgMain";
            this.lcgMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgMain.GroupBordersVisible = false;
            this.lcgMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciLogin,
            this.lciPassword,
            this.lciGroup,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.lcgMain.Location = new System.Drawing.Point(0, 0);
            this.lcgMain.Name = "lcgMain";
            this.lcgMain.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.lcgMain.Size = new System.Drawing.Size(252, 107);
            this.lcgMain.Text = "lcgMain";
            this.lcgMain.TextVisible = false;
            // 
            // lciLogin
            // 
            this.lciLogin.Control = this.edtLogin;
            this.lciLogin.CustomizationFormText = "Login";
            this.lciLogin.Location = new System.Drawing.Point(0, 0);
            this.lciLogin.Name = "lciLogin";
            this.lciLogin.Size = new System.Drawing.Size(244, 24);
            this.lciLogin.Text = "Login";
            this.lciLogin.TextSize = new System.Drawing.Size(46, 13);
            // 
            // lciPassword
            // 
            this.lciPassword.Control = this.edtPassword;
            this.lciPassword.CustomizationFormText = "Password";
            this.lciPassword.Location = new System.Drawing.Point(0, 24);
            this.lciPassword.Name = "lciPassword";
            this.lciPassword.Size = new System.Drawing.Size(244, 24);
            this.lciPassword.Text = "Password";
            this.lciPassword.TextSize = new System.Drawing.Size(46, 13);
            // 
            // lciGroup
            // 
            this.lciGroup.Control = this.edtContext;
            this.lciGroup.CustomizationFormText = "Group";
            this.lciGroup.Location = new System.Drawing.Point(0, 48);
            this.lciGroup.Name = "lciGroup";
            this.lciGroup.Size = new System.Drawing.Size(244, 24);
            this.lciGroup.Text = "Group";
            this.lciGroup.TextSize = new System.Drawing.Size(46, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnCancel;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(122, 27);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnOk;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(122, 72);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(122, 27);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // fmLogin
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(252, 107);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fmLogin";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.fmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edtContext.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup lcgMain;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit edtContext;
        private DevExpress.XtraEditors.TextEdit edtPassword;
        private DevExpress.XtraEditors.TextEdit edtLogin;
        private DevExpress.XtraLayout.LayoutControlItem lciLogin;
        private DevExpress.XtraLayout.LayoutControlItem lciPassword;
        private DevExpress.XtraLayout.LayoutControlItem lciGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}