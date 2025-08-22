using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using VdO2013Core;

namespace VdO2013MainCore
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ClientModuleControl : XtraUserControl, IClientModuleControl
    {
        #region Commands support
        private Dictionary<string, ParameterInfo[]> _commands = new Dictionary<string, ParameterInfo[]>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected bool RegisterCommand(string name, IEnumerable<ParameterInfo> parameters)
        {
            if (_commands.Keys.Contains(name))
                return false;

            _commands.Add(name, parameters.ToArray());
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool FindCommand(string name)
        {
            return _commands.Keys.Contains(name);
        }
        #endregion Commands support

        /// <summary>
        /// 
        /// </summary>
        public ClientModuleControl()
        {
            InitializeComponent();
        }

        #region Membri di IClientModuleControl
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetSupportedCommandNames()
        {
            return _commands.Keys.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public ParameterInfo[] GetSupportedCommandArgs(string commandName)
        {
            return FindCommand(commandName) ? _commands[commandName].ToArray() : new ParameterInfo[] { };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public Type[] GetSupportedCommandArgsSimple(string commandName)
        {
            var args = GetSupportedCommandArgs(commandName);
            if (args != null)
            {
                return (from a in args select a.ParameterType).ToArray();
            }
            return new Type[] { };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public virtual int SendCommand(string commandName, object[] args, out Exception error)
        {
            error = null;
            return 0;
        }
        public virtual object[] SendCommandNoArgs { get { return new object[] { }; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dock"></param>
        /// <param name="parent"></param>
        public virtual void MakeVisible(DockStyle dock = DockStyle.Fill, Control parent = null)
        {
            if (parent != null)
                this.Parent = parent;
            this.Dock = dock;
            this.Visible = true;
            this.BringToFront();

            this.MergeRibbon(RibbonPage, out _preMergeMainRibbonPage);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void MakeInvisible()
        {
            this.UnMergeRibbon(_preMergeMainRibbonPage);

            this.Dock = DockStyle.None;
            this.SendToBack();
            this.Visible = false;
        }

        private RibbonPage _preMergeMainRibbonPage;
        protected RibbonPage PreMergeMainRibbonPage { get => _preMergeMainRibbonPage; }

        /// <summary>
        /// 
        /// </summary>
        public RibbonControl MainRibbon { get; set; }
        public virtual RibbonControl Ribbon { get; }
        public virtual RibbonPage RibbonPage { get; }
        public virtual BarButtonItem RibbonCloseButton { get; }
        public bool CanMergeRibbons => MainRibbon != null && Ribbon != null;


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public bool MergeRibbon(RibbonPage selectPage)
        //{
        //    if (!CanMergeRibbons) return false;
        //    MainRibbon.BeginInit();
        //    PreMergeMainRibbonPage = MainRibbon?.SelectedPage;
        //    MainRibbon.MergeRibbon(Ribbon);
        //    MainRibbon.EndInit();
        //    MainRibbon.SelectedPage = selectPage;
        //    return true;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public bool UnMergeRibbon(RibbonPage selectPage)
        //{

        //    if (!CanMergeRibbons) return false;
        //    MainRibbon.BeginInit();
        //    MainRibbon.UnMergeRibbon();
        //    MainRibbon.EndInit();
        //    MainRibbon.SelectedPage = selectPage;
        //    return true;
        //}
        #endregion Membri di IClientModuleControl
    }
}
