using System;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure
{
    public static class Md5Helper
    {
        /// <summary>
        /// MD5 16位加密 加密后密码为小写
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5Str16(string ConvertString)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(ConvertString)), 4, 8);
                    return t2.Replace("-", "").ToLower();
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// MD5 32位加密 加密后密码为小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5Str32(string text)
        {
            try
            {
                using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
                {
                    byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    return sBuilder.ToString().ToLower();
                }
            }
            catch { return null; }
        }
    }
}
