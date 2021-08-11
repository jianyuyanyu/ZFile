using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Component.Common.Helpers
{
   public class MD5Helper
    {

        public static string CreateFileMd5(FileStream fst)
        {
            //创建MD5
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(fst);
            byte[] hash = md5.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte byt in hash)
                sb.Append(String.Format("{0:X1}", byt));
            return sb.ToString();
        }

        public static string GetMd5(string text)
        {
            var md5Bytes = MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));

            return GetMd5Text(md5Bytes);
        }
        public static string GetMd5(byte[] buffer)
        {
            var md5Bytes = MD5.Create().ComputeHash(buffer);

            return GetMd5Text(md5Bytes);
        }
        private static string GetMd5Text(byte[] md5Bytes)
        {
            return BitConverter.ToString(md5Bytes).Replace("-", "").ToLower();
        }
    }
}
