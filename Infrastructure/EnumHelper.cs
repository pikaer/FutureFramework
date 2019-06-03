using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Infrastructure
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举描述值
        /// </summary>
        /// <returns></returns>
        public static string ToDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        public static Dictionary<int, string> ToDictionary(Type enumType)
        {
            Dictionary<int, string> listitem = new Dictionary<int, string>();
            Array vals = Enum.GetValues(enumType);
            foreach (Enum enu in vals)
            {
                listitem.Add(Convert.ToInt32(enu), enu.ToDescription());
            }
            return listitem;
        }

        public static List<SelectPicker> ToSelectPicker(Type enumType)
        {
            var listitem = new List<SelectPicker>();
            Array vals = Enum.GetValues(enumType);
            foreach (Enum enu in vals)
            {
                var selectPicker = new SelectPicker()
                {
                    SelectKey = Convert.ToInt32(enu),
                    SelectValue = enu.ToDescription()
                };
                listitem.Add(selectPicker);
            }
            return listitem;
        }
    }

    /// <summary>
    /// 下拉列表框帮助类
    /// </summary>
    public class SelectPicker
    {
        /// <summary>
        /// 键
        /// </summary>
        public int SelectKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string SelectValue { get; set; }
    }
}
