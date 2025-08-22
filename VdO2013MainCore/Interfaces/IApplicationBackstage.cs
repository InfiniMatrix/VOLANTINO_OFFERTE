using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.XtraLayout;

using VdO2013Core;
using VdO2013DataCore;

namespace VdO2013MainCore.Controls.Ribbon
{
    public interface IApplicationBackstage : IDevExpressLayoutSupported, IDataAware
    {
        LayoutControl LayoutControl { get; }
        LayoutGroup RootGroup { get; }

        bool LoadDataAndLayout();
        bool SaveDataAndLayout();
    }
}
