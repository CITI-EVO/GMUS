using System;
using System.Security.Cryptography;
using System.Text;

namespace CITI.EVO.CommonData.Web.Helpers
{
    /// <summary>
    /// Summary description for Cryptography
    /// </summary>
    public static class Cryptography
    {
        public static byte[] Md5(String text)
        {
            return Md5(text, Encoding.UTF8);
        }

        public static byte[] Md5(String text, Encoding encoding)
        {
            return Md5(encoding.GetBytes(text));
        }

        public static byte[] Md5(byte[] bytes)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(bytes);
            }
        }

    }
}