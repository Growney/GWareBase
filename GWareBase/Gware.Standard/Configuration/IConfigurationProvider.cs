using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Configuration
{
    public interface IConfigurationProvider<T> where T : Enum
    {
        bool GetBoolean(T settingID);
        int GetInt(T settingID);
        long GetLong(T settingID);
        string GetString(T settingID);
        DateTime GetDateTime(T settingID);
        K GetConfigurationType<K>(T settingID) where K : IConfigurationType;

        void SetValue(T settingID, bool value);
        void SetValue(T settingID, int value);
        void SetValue(T settingID, long value);
        void SetValue(T settingID, string value);
        void SetValue(T settingID, DateTime value);
        void SetValue(T settingID, IConfigurationType configurationType);
    }
}
