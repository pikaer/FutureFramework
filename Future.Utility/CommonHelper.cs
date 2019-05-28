using Infrastructure;

namespace Future.Utility
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取动态路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetImgPath(this string shortPath)
        {
            string rtn = string.Empty;
            if (string.IsNullOrEmpty(shortPath))
            {
                return rtn;
            }

            //默认前缀
            string defaultPath = JsonSettingHelper.AppSettings["GetImgPath"];

            string host = JsonSettingHelper.AppSettings["CommonApiHost"];

            if (!string.IsNullOrEmpty(defaultPath)&& !string.IsNullOrEmpty(host))
            {
                rtn = string.Format("{0}{1}{2}", host, defaultPath, shortPath);
            }
            return rtn;
        }
    }
}
