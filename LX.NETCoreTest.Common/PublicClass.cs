using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LX.NETCoreTest.Common {
    public class PublicClass {

        #region Md5加密
        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="input">传入字符</param>
        /// <param name="key">默认字符</param>
        /// <returns></returns>
        public static string _Md5(string input, string key = "LX.Pystudio") {
            var hash = string.Empty;
            using (MD5 md5Hash = MD5.Create()) {
                hash = GetMd5Hash(md5Hash, input + key);
            }
            return hash;
        }

        static string GetMd5Hash(MD5 md5Hash, string input) {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString().ToUpper();
        } 
        #endregion
    }
}
