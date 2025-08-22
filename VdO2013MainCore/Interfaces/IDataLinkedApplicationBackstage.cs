using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

using DevExpress.XtraEditors;

using VdO2013Data;
using VdO2013DataCore;

namespace VdO2013MainCore.Controls.Ribbon
{
    public interface IDataLinkedApplicationBackstage : IApplicationBackstage, IVDataViewLinked
    {
    }
}
