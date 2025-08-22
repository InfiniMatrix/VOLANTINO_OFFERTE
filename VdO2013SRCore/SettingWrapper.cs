using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VdO2013Core;
using VdO2013Core.Config;

namespace VdO2013SRCore
{
    public struct SettingWrapper
    {
        private String _key;
        private ConfigSettingValue _value;

        public SettingWrapper(String key, Object value, Boolean isSettingValue)
        {
            if (isSettingValue)
            {
                _key = key;
                _value = value as ConfigSettingValue;
            }
            else
            {
                _key = key;
                _value = new ConfigSettingValue(value);
            }
        }
        public SettingWrapper(String key, ConfigSettingValue value)
            : this(key, value, true)
        {
        }

        public String Key { get { return _key; } }
        public ConfigSettingValue Value { get { return _value; } }
    }
}
