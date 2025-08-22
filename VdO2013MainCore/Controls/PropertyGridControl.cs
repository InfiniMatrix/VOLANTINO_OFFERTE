using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;

using MPExtensionMethods;

namespace VdO2013MainCore
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PropertyGridControl : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyGridControl()
        {
            InitializeComponent();
        }

        private TreeListNode AddNode(TreeListNodes parent, Object nodeData)
        {
            if (parent == null || nodeData == null) return null;
            var rn = nodeData.GetDesignerDisplayName();
            var rt = nodeData.GetType().FullName;
            return parent.Add(rn, rt, nodeData);
        }

        private TreeListNode AddNode(TreeListNode parent, Object nodeData)
        {
            return AddNode(parent.Nodes, nodeData);
        }

        private TreeListNode FindNodeByReference(Object reference)
        {
            if (reference == null) return null;
            return this.cmbComponents.Properties.TreeList.FindNode(n => n.GetValue(this.colReference).Equals(reference));
        }

        private Int32 InternalGetChildren(Object rootObject, out IEnumerable<Object> all)
        {
            try
            {
                IEnumerable<Object> controls = null;
                IEnumerable<Object> components = null;
                if (rootObject is Form)
                {
                    var f = rootObject as Form;
                    controls = f.Controls.Cast<Component>();
                    components = f.EnumerateComponents(true);
                }
                else if (rootObject is ContainerControl)
                {
                    var cc = rootObject as ContainerControl;
                    controls = cc.Controls.Cast<Component>();
                    components = cc.EnumerateComponents(true);
                }
                else if (rootObject is Control)
                {
                    var c = rootObject as Control;
                    controls = c.Controls.Cast<Component>();
                    components = c.EnumerateComponents(true);
                }
                else if (rootObject is IContainer)
                {
                    controls = new List<Component>();
                    components = (rootObject as IContainer).Components.OfType<Component>();
                }
                else if (rootObject is IComponent)
                {
                    controls = new List<Component>();
                    components = (rootObject as IComponent).EnumerateComponents<IComponent>();
                }

                all = controls.Concat(from component in components where !controls.Contains(component) select component);
                return all.Count();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                all = null;
                return -1;
            }
        }

        private Boolean AddChildrenNode(Object parentObject, TreeListNode parentNode)
        {
            try
            {
                if (InternalGetChildren(parentObject, out IEnumerable<object> all) <= 0) return false;

                foreach (var o in all)
                {
                    TreeListNode n = FindNodeByReference(o);
                    if (n == null)
                    {
                        n = AddNode(parentNode, o);
                        if (n != null)
                            AddChildrenNode(o, n);
                    }
                }// foreach (var o in all)

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void FillTree(Object rootObject)
        {
            if (this.cmbComponents.Properties.TreeList == null) return;
            this.cmbComponents.Properties.TreeList.BeginUnboundLoad();
            TreeListNode rootNode = null;
            try
            {
                this.cmbComponents.Properties.TreeList.ClearNodes();
                if (rootObject == null) return;

                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    rootNode = AddNode(this.cmbComponents.Properties.TreeList.Nodes, Application.OpenForms[i]);
                    AddChildrenNode(Application.OpenForms[i], rootNode);
                }

                rootNode = AddNode(this.cmbComponents.Properties.TreeList.Nodes, rootObject);
                AddChildrenNode(rootObject, rootNode);
            }
            finally
            {
                this.cmbComponents.Properties.TreeList.EndUnboundLoad();

                if (this.cmbComponents.Properties.TreeList.AllNodesCount > 0)
                {
                    this.cmbComponents.Properties.TreeList.SetFocusedNode(rootNode);
                    this.cmbComponents.EditValue = this.cmbComponents.Properties.TreeList.FocusedNode;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetSelectedObject(Object value)
        {
            if (this.ctlProperties.SelectedObject == value) return;

            this.ctlProperties.SelectedObject = value;
            this.txtPropertySelectedObject.EditValue = value;
            FillTree(value);
        }
        /// <summary>
        /// 
        /// </summary>
        public Object SelectedObject
        {
            get { return this.ctlProperties.SelectedObject; }
            set { SetSelectedObject(value); }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //public Object[] SelectedObjects
        //{
        //    get { return this.ctlProperties.SelectedObjects; }
        //    set { this.ctlProperties.SelectedObjects = value; this.txtPropertySelectedObject.EditValue = value; }
        //}

#pragma warning disable IDE1006 // Naming Styles
        private void cmbComponents_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (e.Value is System.Collections.ArrayList v && v.Count == 3)
            {
                var s0 = "{0}".FormatWith(v[this.colReferenceName.VisibleIndex]);
                var s1 = "{0}".FormatWith(v[this.colType.VisibleIndex]);
                if (s0.Equals(s1))
                    e.DisplayText = s0;
                else
                    e.DisplayText = "{0}\t{1}".FormatWith(s0, s1);
            }
            else if (e.Value is TreeListNode)
            {
                var n = e.Value as TreeListNode;
                var s0 = "{0}".FormatWith(n.GetValue(this.colReferenceName));
                var s1 = "{0}".FormatWith(n.GetValue(this.colType));
                if (s0.Equals(s1))
                    e.DisplayText = s0;
                else
                    e.DisplayText = "{0}\t{1}".FormatWith(s0, s1);
            }
            else
                e.DisplayText = "{0}".FormatWith(e.Value);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void cmbComponents_EditValueChanged(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            var o = cmbComponents.Properties.TreeList.FocusedNode.GetValue(colReference);
            if (o != null)
                ctlProperties.SelectedObject = o;
        }
    }
}
