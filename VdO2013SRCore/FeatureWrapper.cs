using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VdO2013Core.Config;

namespace VdO2013SRCore
{
    [Serializable]
    public struct FeatureWrapper
    {
        public String Name;
        public String XPath;
        public String Mapping;
        public String TypeName;
        public int MaxSize;
        public int MinSize;
        public Boolean Nullable;
        public String IfMissingUseFeature;

        [Serializable]
        public struct ActionsWrapper
        {
            public String IsNull;
            public String NullIf;
            public Boolean TrimLeft;
            public Boolean TrimRight;
            public Boolean ToUpperCase;
            public Boolean ToLowerCase;

            [Serializable]
            public struct ReplacementWrapper
            {
                public String Text;
                public String NewText;
                public Boolean CaseSensitive;
                public int RepeatCount;
            }

            public ReplacementWrapper[] Replacements;

            [Serializable]
            public struct SubStringWrapper
            {
                public String Start;
                public Boolean StartIsSearchFor;
                public String End;
                public Boolean EndIsSearchFor;
                public Boolean FromLeft;
            }

            public SubStringWrapper[] SubStrings;
        }

        public ActionsWrapper Actions;
    }
}
