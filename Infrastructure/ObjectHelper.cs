using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T JsonToObject<T>(this object value)
        {
            T rtn = default;
            try
            {
                rtn = JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch
            {
                _ = ("Json值:" + value.ToString() + "转换失败");
            }
            return rtn;
        }

        /// <summary>
        /// 将实体类序列化为JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SerializeToString<T>(this T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);
            return serializedResult;
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object Bytes2Object(this byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<object>(json);
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T Bytes2Object<T>(this byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NotEmpty<T>(this List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return true;
            }
            return !list.Any();
        }

        public static bool NotEmpty<T>(this IEnumerable<T> list)
        {
            if (list != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
