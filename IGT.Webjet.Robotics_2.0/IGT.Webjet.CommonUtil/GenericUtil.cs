using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace IGT.Webjet.CommonUtil
{
    public static class GenericUtil
    {
        public static string GetJsonString<T>(this T _object)
        {
            string strJson = string.Empty;
            strJson = JsonConvert.SerializeObject(_object);
            return strJson;
        }

        public static T GetObjectFromJson<T>(string jsonMsg)
        {
            return JsonConvert.DeserializeObject<T>(jsonMsg);
        }

        public static T ToEnum<T>(this string value)
        {
            object returnEnum = default(T);

            if (Enum.TryParse(typeof(T), value, true, out returnEnum))
                return (T)returnEnum;
            else
                return default(T);
        }

        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
