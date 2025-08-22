using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace VdO2013SRCore
{
    public interface ISiteReader2Factory
    {
        ISiteReader2 CreateReader(String assemblyQualifiedName, params Object[] args);
    }

}
