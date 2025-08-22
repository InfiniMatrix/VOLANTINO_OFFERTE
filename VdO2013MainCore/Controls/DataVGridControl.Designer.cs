namespace VdO2013MainCore.Controls
{
    partial class DataVGridControl
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
            this.ctlReaderInfos = new VdO2013MainCore.SiteReaderLinkedInfoControl();
            this.ctlGrid = new DevExpress.XtraVerticalGrid.VGridControl();
            this.ctlNavigator = new DevExpress.XtraEditors.ControlNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.ctlGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ctlReaderInfos
            // 
            this.ctlReaderInfos.AddInfoVisible = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            this.ctlReaderInfos.Agenzia = null;
            this.ctlReaderInfos.AgenziaNullText = "?";
            this.ctlReaderInfos.SplitterCustomPane.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.SplitterCustomPane.Name = "";
            this.ctlReaderInfos.SplitterCustomPane.TabIndex = 1;
            this.ctlReaderInfos.SplitterCustomPane.Visible = false;
            this.ctlReaderInfos.DataLinkChanged = null;
            this.ctlReaderInfos.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal;
            this.ctlReaderInfos.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlReaderInfos.Elementi = null;
            this.ctlReaderInfos.ElementiNullText = "?";
            this.ctlReaderInfos.SplitterFixedPane.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.SplitterFixedPane.Name = "";
            this.ctlReaderInfos.SplitterFixedPane.Size = new System.Drawing.Size(1167, 53);
            this.ctlReaderInfos.SplitterFixedPane.TabIndex = 0;
            this.ctlReaderInfos.IsDataFiltered = false;
            this.ctlReaderInfos.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.Margin = new System.Windows.Forms.Padding(17, 19, 17, 19);
            this.ctlReaderInfos.Name = "ctlReaderInfos";
            this.ctlReaderInfos.RagioneSociale = null;
            this.ctlReaderInfos.RagioneSocialeNullText = "?";
            this.ctlReaderInfos.Reader = null;
            this.ctlReaderInfos.ReaderChanged = null;
            this.ctlReaderInfos.ReaderChanging = null;
            this.ctlReaderInfos.ReaderType = null;
            this.ctlReaderInfos.ReadOnly = true;
            this.ctlReaderInfos.RestoreDataOnReaderChanged = false;
            this.ctlReaderInfos.RestoreLayoutOnReaderChanged = false;
            this.ctlReaderInfos.Size = new System.Drawing.Size(1167, 53);
            this.ctlReaderInfos.SplitterVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.ctlReaderInfos.TabIndex = 0;
            this.ctlReaderInfos.Tipologia = null;
            this.ctlReaderInfos.TipologiaNullText = "?";
            this.ctlReaderInfos.Visible = false;
            // 
            // ctlGrid
            // 
            this.ctlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlGrid.Location = new System.Drawing.Point(0, 53);
            this.ctlGrid.Margin = new System.Windows.Forms.Padding(5);
            this.ctlGrid.Name = "ctlGrid";
            this.ctlGrid.Size = new System.Drawing.Size(1167, 436);
            this.ctlGrid.TabIndex = 1;
            // 
            // ctlNavigator
            // 
            this.ctlNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctlNavigator.Location = new System.Drawing.Point(0, 489);
            this.ctlNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.ctlNavigator.Name = "ctlNavigator";
            this.ctlNavigator.NavigatableControl = this.ctlGrid;
            this.ctlNavigator.Size = new System.Drawing.Size(1167, 42);
            this.ctlNavigator.TabIndex = 2;
            this.ctlNavigator.Text = "controlNavigator1";
            // 
            // DataVGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.Controls.Add(this.ctlGrid);
            this.Controls.Add(this.ctlReaderInfos);
            this.Controls.Add(this.ctlNavigator);
            this.Margin = new System.Windows.Forms.Padding(8, 11, 8, 11);
            this.Name = "DataVGridControl";
            this.Size = new System.Drawing.Size(1167, 531);
            this.Controls.SetChildIndex(this.ctlNavigator, 0);
            this.Controls.SetChildIndex(this.ctlReaderInfos, 0);
            this.Controls.SetChildIndex(this.ctlGrid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ctlGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SiteReaderLinkedInfoControl ctlReaderInfos;
        private DevExpress.XtraVerticalGrid.VGridControl ctlGrid;
        private DevExpress.XtraEditors.ControlNavigator ctlNavigator;

    }
}
