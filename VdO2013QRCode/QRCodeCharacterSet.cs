using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013QRCode
{
    class QRCodeCharacterSet
    {
        public static Dictionary<String, String> CharacterSets =
                        new Dictionary<String, String>
                        {
                             { "Default UTF-8", "UTF-8"},
                             { "ISO-8859-1", "ISO-8859-1"},
                             { "SHIFT-JIS", "SHIFT-JIS"},
                             { "CP437", "CP437"},
                             { "ISO-8859-2", "ISO-8859-2"},
                             { "ISO-8859-3", "ISO-8859-3"},
                             { "ISO-8859-4", "ISO-8859-4"},
                             { "ISO-8859-5", "ISO-8859-5"},
                             { "ISO-8859-6", "ISO-8859-6"},
                             { "ISO-8859-7", "ISO-8859-7"},
                             { "ISO-8859-8", "ISO-8859-8"},
                             { "ISO-8859-9", "ISO-8859-9"},
                             { "ISO-8859-10", "ISO-8859-10"},
                             { "ISO-8859-11", "ISO-8859-11"},
                             { "ISO-8859-13", "ISO-8859-13"},
                             { "ISO-8859-14", "ISO-8859-14"},
                             { "ISO-8859-15", "ISO-8859-15"},
                             { "ISO-8859-16", "ISO-8859-16"},
                        };
    }
}
