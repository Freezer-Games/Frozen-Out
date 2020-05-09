using System;

namespace Scripts.Level.Dialogue.Instagram.Utils
{
    public class DataReaderUtils
    {
        public static int GetInt(object data)
        {
            if(IsNull(data))
            {
                return 0;
            }

            return Convert.ToInt32(data);
        }

        public static DateTime GetDateTime(object data)
        {
            if (IsNull(data))
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(data);
        }

        private static bool IsNull(object data)
        {
            return data == null || string.IsNullOrEmpty(data.ToString());
        }
    }
}