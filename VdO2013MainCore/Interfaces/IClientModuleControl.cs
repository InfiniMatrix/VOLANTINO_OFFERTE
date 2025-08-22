using System;
using VdO2013Core;
using VdO2013SRCore;

namespace VdO2013MainCore
{
    public interface IClientModuleControl : ICommandReceiver, IRibbonSupported
    {
        void MakeVisible(System.Windows.Forms.DockStyle dock = System.Windows.Forms.DockStyle.Fill, System.Windows.Forms.Control parent = null);
        void MakeInvisible();
    }
}
