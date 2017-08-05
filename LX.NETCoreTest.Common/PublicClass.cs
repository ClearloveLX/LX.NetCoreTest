using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static LX.NETCoreTest.Common.EnumHelper;
using MimeKit;
using MailKit.Net.Smtp;

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

        #region 邮件
        /// <summary>
        /// 邮件
        /// </summary>
        /// <param name="dicToEmail"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        /// <param name="fromEmial"></param>
        /// <returns></returns>
        public static bool _SendEmail(
            Dictionary<string, string> dicToEmail,
            string title, string content,
            string name = "爱留图网", string fromEmail = "gk1213656215@qq.com") {
            var isOk = false;
            try {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content)) { return isOk; }

                //设置基本信息
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, fromEmail));
                foreach (var item in dicToEmail.Keys) {
                    message.To.Add(new MailboxAddress(item, dicToEmail[item]));
                }
                message.Subject = title;
                message.Body = new TextPart("html") {
                    Text = content
                };

                //链接发送
                using (var client = new SmtpClient()) {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //采用qq邮箱服务器发送邮件
                    client.Connect("smtp.qq.com", 587, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    //qq邮箱，密码(安全设置短信获取后的密码)
                    client.Authenticate("gk1213656215@qq.com", "ufiaszkkulbabejh");

                    client.Send(message);
                    client.Disconnect(true);
                }
                isOk = true;
            }
            catch (Exception ex) {

            }
            return isOk;
        }

        #endregion

        #region 读取HTML模版
        /// <summary>
        /// 读取HTML模版
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static async Task<string> _GetHtmlTpl(EmEmailTpl tpl, string folderPath = @"D:\ClearloveLX\LX.NetCoreTest\LX.NETCoreTest.Web\wwwroot\tpl") {
            var content = string.Empty;
            if (string.IsNullOrWhiteSpace(folderPath)) {
                return content;
            }

            var path = $"{folderPath}/{tpl}.html";
            try {
                using (var stream = File.OpenRead(path)) {
                    using (var reader = new StreamReader(stream)) {
                        content = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }
            return content;
        }

        #endregion
    }
}
