using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA.BLL
{
    public static class AppConfig
    {
        public static string CacheType
        {
            get { return GetStringValue("CacheType"); }
        }
        public static int MaxAllowedFailedAttempt
        {
            get { return GetIntValue("MaxAllowedFailedAttempt"); }
        }

        private static int GetIntValue(string key)
        {
            string val;
            try
            {
                val = System.Configuration.ConfigurationManager.AppSettings[key];
                return (val == null ? 0 : Convert.ToInt32(val));
            }
            finally { val = null; }
        }
        private static string GetStringValue(string key)
        {
              return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
