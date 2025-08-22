using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdO2013Core;
using VdO2013Data;

namespace VdO2013MainCore
{
    interface IDataLinkedControl : IDevExpressLayoutSupported, IVDataViewGridLinkCollectionSupported
    {
        bool LoadDataAndLayout();
        bool SaveDataAndLayout();
    }
}
