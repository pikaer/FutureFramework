using System;

namespace Infrastructure
{
    /// <summary>
    /// 地理位置帮助类
    /// </summary>
    public static class LocationHelper
    {
        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;

        public static string GetDistanceDesc(double lat1, double lng1, double lat2, double lng2)
        {
            double distance = GetDistance(lat1, lng1, lat2, lng2);
            if (distance <= 0)
            {
                return "0米";
            }
            if (distance < 1000)
            {
                return string.Format("{0}米", distance);
            }
            return string.Format("{0}公里",(distance / 1000).ToString("0.00"));
        }

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns>两点的距离，单位 米</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        private static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }
    }
}
