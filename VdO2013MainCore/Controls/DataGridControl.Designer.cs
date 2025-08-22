namespace VdO2013MainCore
{
    public partial class DataGridControl
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
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

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGridControl));
            this.ctlGrid = new DevExpress.XtraGrid.GridControl();
            this.viwGrid = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.img11x11 = new System.Windows.Forms.ImageList(this.components);
            this.ctlReaderInfos = new VdO2013MainCore.SiteReaderLinkedInfoControl();
            this.BehaviorManager = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.viwGridDragDropEvents = new DevExpress.Utils.DragDrop.DragDropEvents(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ctlGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viwGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BehaviorManager)).BeginInit();
            this.SuspendLayout();
            // 
            // ctlGrid
            // 
            this.ctlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctlGrid.Location = new System.Drawing.Point(0, 44);
            this.ctlGrid.MainView = this.viwGrid;
            this.ctlGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctlGrid.Name = "ctlGrid";
            this.ctlGrid.Size = new System.Drawing.Size(1050, 395);
            this.ctlGrid.TabIndex = 2;
            this.ctlGrid.UseEmbeddedNavigator = true;
            this.ctlGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viwGrid});
            // 
            // viwGrid
            // 
            this.BehaviorManager.SetBehaviors(this.viwGrid, new DevExpress.Utils.Behaviors.Behavior[] {
            ((DevExpress.Utils.Behaviors.Behavior)(DevExpress.Utils.DragDrop.DragDropBehavior.Create(typeof(DevExpress.XtraGrid.Extensions.ColumnViewDragDropSource), true, true, true, true, this.viwGridDragDropEvents)))});
            this.viwGrid.DetailHeight = 289;
            this.viwGrid.GridControl = this.ctlGrid;
            this.viwGrid.Images = this.img11x11;
            this.viwGrid.Name = "viwGrid";
            this.viwGrid.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this.viwGrid.OptionsLayout.StoreAllOptions = true;
            this.viwGrid.OptionsLayout.StoreAppearance = true;
            this.viwGrid.OptionsView.ShowDetailButtons = false;
            this.viwGrid.OptionsView.ShowGroupPanel = false;
            // 
            // img11x11
            // 
            this.img11x11.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img11x11.ImageStream")));
            this.img11x11.TransparentColor = System.Drawing.Color.Transparent;
            this.img11x11.Images.SetKeyName(0, "Cartello 11x11.png");
            this.img11x11.Images.SetKeyName(1, "Ordina 11x11.png");
            this.img11x11.Images.SetKeyName(2, "Print 11x11.png");
            // 
            // ctlReaderInfos
            // 
            this.ctlReaderInfos.AddInfoVisible = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            this.ctlReaderInfos.Agenzia = null;
            this.ctlReaderInfos.AgenziaNullText = "?";
            this.ctlReaderInfos.DataLinkChanged = null;
            this.ctlReaderInfos.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal;
            this.ctlReaderInfos.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlReaderInfos.Elementi = null;
            this.ctlReaderInfos.ElementiNullText = "0";
            this.ctlReaderInfos.IsDataFiltered = false;
            this.ctlReaderInfos.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.ctlReaderInfos.Name = "ctlReaderInfos";
            this.ctlReaderInfos.Numero = "Not Implemented";
            this.ctlReaderInfos.RagioneSociale = null;
            this.ctlReaderInfos.RagioneSocialeNullText = "?";
            this.ctlReaderInfos.Reader = null;
            this.ctlReaderInfos.ReaderChanged = null;
            this.ctlReaderInfos.ReaderChanging = null;
            this.ctlReaderInfos.ReaderType = null;
            this.ctlReaderInfos.ReadOnly = true;
            this.ctlReaderInfos.RestoreDataOnReaderChanged = false;
            this.ctlReaderInfos.RestoreLayoutOnReaderChanged = false;
            this.ctlReaderInfos.Size = new System.Drawing.Size(1050, 44);
            this.ctlReaderInfos.SplitterCustomPane.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.SplitterCustomPane.Name = "";
            this.ctlReaderInfos.SplitterCustomPane.TabIndex = 1;
            this.ctlReaderInfos.SplitterCustomPane.Text = "CustomPane";
            this.ctlReaderInfos.SplitterCustomPane.Visible = false;
            this.ctlReaderInfos.SplitterFixedPane.Location = new System.Drawing.Point(0, 0);
            this.ctlReaderInfos.SplitterFixedPane.Name = "";
            this.ctlReaderInfos.SplitterFixedPane.Size = new System.Drawing.Size(1050, 44);
            this.ctlReaderInfos.SplitterFixedPane.TabIndex = 0;
            this.ctlReaderInfos.SplitterFixedPane.Text = "FixedPane";
            this.ctlReaderInfos.SplitterHorizontal = true;
            this.ctlReaderInfos.SplitterPosition = 36;
            this.ctlReaderInfos.SplitterVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.ctlReaderInfos.TabIndex = 4;
            this.ctlReaderInfos.Tipologia = null;
            this.ctlReaderInfos.TipologiaNullText = "?";
            this.ctlReaderInfos.Visible = false;
            // 
            // DataGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.Controls.Add(this.ctlGrid);
            this.Controls.Add(this.ctlReaderInfos);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.Name = "DataGridControl";
            this.Size = new System.Drawing.Size(1050, 439);
            this.SplitterCustomPane.Text = "CustomPane";
            this.SplitterFixedPane.Text = "FixedPane";
            this.Controls.SetChildIndex(this.ctlReaderInfos, 0);
            this.Controls.SetChildIndex(this.ctlGrid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ctlGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viwGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BehaviorManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        protected DevExpress.XtraGrid.GridControl ctlGrid;
        /// <summary>
        /// 
        /// </summary>
        protected DevExpress.XtraGrid.Views.Grid.GridView viwGrid;
        public SiteReaderLinkedInfoControl ctlReaderInfos;
        internal System.Windows.Forms.ImageList img11x11;
        private DevExpress.Utils.Behaviors.BehaviorManager BehaviorManager;
        private DevExpress.Utils.DragDrop.DragDropEvents viwGridDragDropEvents;
    }// partial class DataGridControl
}// namespace VdO2013Main.Controls.Base
