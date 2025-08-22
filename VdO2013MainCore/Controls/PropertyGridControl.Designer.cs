namespace VdO2013MainCore
{
    partial class PropertyGridControl
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
            this.ctlSplitProperties = new DevExpress.XtraEditors.SplitContainerControl();
            this.ctlProperties = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.cmbComponents = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
            this.colReferenceName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colReference = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.txtPropertySelectedObject = new DevExpress.XtraEditors.TextEdit();
            this.ctlPropertyDescription = new DevExpress.XtraVerticalGrid.PropertyDescriptionControl();
            ((System.ComponentModel.ISupportInitialize)(this.ctlSplitProperties)).BeginInit();
            this.ctlSplitProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbComponents.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPropertySelectedObject.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ctlSplitProperties
            // 
            this.ctlSplitProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlSplitProperties.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.ctlSplitProperties.Horizontal = false;
            this.ctlSplitProperties.Location = new System.Drawing.Point(0, 0);
            this.ctlSplitProperties.Name = "ctlSplitProperties";
            this.ctlSplitProperties.Panel1.Controls.Add(this.ctlProperties);
            this.ctlSplitProperties.Panel1.Controls.Add(this.cmbComponents);
            this.ctlSplitProperties.Panel1.Controls.Add(this.txtPropertySelectedObject);
            this.ctlSplitProperties.Panel1.Text = "Panel1";
            this.ctlSplitProperties.Panel2.Controls.Add(this.ctlPropertyDescription);
            this.ctlSplitProperties.Panel2.Text = "Panel2";
            this.ctlSplitProperties.Size = new System.Drawing.Size(262, 400);
            this.ctlSplitProperties.SplitterPosition = 60;
            this.ctlSplitProperties.TabIndex = 1;
            this.ctlSplitProperties.Text = "splitContainerControl1";
            // 
            // ctlProperties
            // 
            this.ctlProperties.AutoGenerateRows = true;
            this.ctlProperties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ctlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlProperties.Location = new System.Drawing.Point(0, 40);
            this.ctlProperties.Name = "ctlProperties";
            this.ctlProperties.OptionsBehavior.UseDefaultEditorsCollection = false;
            this.ctlProperties.OptionsMenu.EnableContextMenu = true;
            this.ctlProperties.Size = new System.Drawing.Size(262, 295);
            this.ctlProperties.TabIndex = 12;
            // 
            // cmbComponents
            // 
            this.cmbComponents.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbComponents.Location = new System.Drawing.Point(0, 20);
            this.cmbComponents.Name = "cmbComponents";
            this.cmbComponents.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.cmbComponents.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbComponents.Properties.TreeList = this.treeListLookUpEdit1TreeList;
            this.cmbComponents.Size = new System.Drawing.Size(262, 20);
            this.cmbComponents.TabIndex = 15;
            this.cmbComponents.EditValueChanged += new System.EventHandler(this.cmbComponents_EditValueChanged);
            this.cmbComponents.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cmbComponents_CustomDisplayText);
            // 
            // treeListLookUpEdit1TreeList
            // 
            this.treeListLookUpEdit1TreeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colReferenceName,
            this.colType,
            this.colReference});
            this.treeListLookUpEdit1TreeList.Location = new System.Drawing.Point(0, 0);
            this.treeListLookUpEdit1TreeList.Name = "treeListLookUpEdit1TreeList";
            this.treeListLookUpEdit1TreeList.OptionsBehavior.EnableFiltering = true;
            this.treeListLookUpEdit1TreeList.OptionsView.ShowIndentAsRowStyle = true;
            this.treeListLookUpEdit1TreeList.Size = new System.Drawing.Size(400, 200);
            this.treeListLookUpEdit1TreeList.TabIndex = 0;
            // 
            // colReferenceName
            // 
            this.colReferenceName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colReferenceName.AppearanceCell.Options.UseFont = true;
            this.colReferenceName.Caption = "Reference";
            this.colReferenceName.FieldName = "ReferenceName";
            this.colReferenceName.Name = "colReferenceName";
            this.colReferenceName.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
            this.colReferenceName.Visible = true;
            this.colReferenceName.VisibleIndex = 0;
            // 
            // colType
            // 
            this.colType.Caption = "Type";
            this.colType.FieldName = "Type";
            this.colType.Name = "colType";
            this.colType.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 1;
            // 
            // colReference
            // 
            this.colReference.Caption = "Object";
            this.colReference.FieldName = "Object";
            this.colReference.Name = "colReference";
            this.colReference.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.Object;
            // 
            // txtPropertySelectedObject
            // 
            this.txtPropertySelectedObject.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPropertySelectedObject.Location = new System.Drawing.Point(0, 0);
            this.txtPropertySelectedObject.Name = "txtPropertySelectedObject";
            this.txtPropertySelectedObject.Properties.ReadOnly = true;
            this.txtPropertySelectedObject.Size = new System.Drawing.Size(262, 20);
            this.txtPropertySelectedObject.TabIndex = 14;
            this.txtPropertySelectedObject.Visible = false;
            // 
            // ctlPropertyDescription
            // 
            this.ctlPropertyDescription.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ctlPropertyDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlPropertyDescription.Location = new System.Drawing.Point(0, 0);
            this.ctlPropertyDescription.Name = "ctlPropertyDescription";
            this.ctlPropertyDescription.PropertyGrid = this.ctlProperties;
            this.ctlPropertyDescription.Size = new System.Drawing.Size(262, 60);
            this.ctlPropertyDescription.TabIndex = 13;
            this.ctlPropertyDescription.TabStop = false;
            // 
            // PropertyGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlSplitProperties);
            this.Name = "PropertyGridControl";
            this.Size = new System.Drawing.Size(262, 400);
            ((System.ComponentModel.ISupportInitialize)(this.ctlSplitProperties)).EndInit();
            this.ctlSplitProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctlProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbComponents.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListLookUpEdit1TreeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPropertySelectedObject.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl ctlSplitProperties;
        private DevExpress.XtraVerticalGrid.PropertyGridControl ctlProperties;
        private DevExpress.XtraEditors.TextEdit txtPropertySelectedObject;
        private DevExpress.XtraVerticalGrid.PropertyDescriptionControl ctlPropertyDescription;
        private DevExpress.XtraEditors.TreeListLookUpEdit cmbComponents;
        private DevExpress.XtraTreeList.TreeList treeListLookUpEdit1TreeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colReferenceName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colReference;
    }
}
