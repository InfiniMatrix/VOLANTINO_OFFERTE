using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013Products.Enums
{
    /// <summary>
    /// ATTENZIONE: Questo enum DEVE essere lo stesso utilizzato in VdO2013WSUtil.Enums
    /// </summary>
    public enum ProductPrivilege
    {
        Nessuna = 0,
        Amministrativa = 1,
        Standard = 2,
        Avanzata = 3,
        Demo = 4
    }
}
