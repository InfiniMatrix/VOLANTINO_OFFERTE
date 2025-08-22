namespace VdO2013MainCore
{
    partial class LayoutSupportedControl
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
            this.ctlSplit = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.ctlSplit)).BeginInit();
            this.ctlSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlSplit
            // 
            this.ctlSplit.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2;
            this.ctlSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlSplit.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel1;
            this.ctlSplit.Location = new System.Drawing.Point(0, 0);
            this.ctlSplit.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ctlSplit.Name = "ctlSplit";
            this.ctlSplit.Panel1.Text = "FixedPane";
            this.ctlSplit.Panel2.Text = "CustomPane";
            this.ctlSplit.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.ctlSplit.Size = new System.Drawing.Size(667, 531);
            this.ctlSplit.SplitterPosition = 40;
            this.ctlSplit.TabIndex = 0;
            // 
            // LayoutSupportedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlSplit);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "LayoutSupportedControl";
            this.Size = new System.Drawing.Size(667, 531);
            ((System.ComponentModel.ISupportInitialize)(this.ctlSplit)).EndInit();
            this.ctlSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl ctlSplit;
    }
}
