using System;

namespace IGT.Webjet.CommonUtil
{
    public static class EnvironmentUtil
    {
        public static string GetStringEnvironmentVarValue(string key)
        {
            try
            {
                return Environment.GetEnvironmentVariable(key);
            }
            catch { return string.Empty; }
        }

        public static int GetIntEnvironmentVarValue(string key)
        {
            string strVal;
            int intVal = -1;

            strVal = GetStringEnvironmentVarValue(key);
            int.TryParse(strVal, out intVal);

            return intVal;
        }
    }
}
