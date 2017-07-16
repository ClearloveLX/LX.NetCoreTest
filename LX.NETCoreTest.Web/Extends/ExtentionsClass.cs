using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.NETCoreTest.Web.Extends
{
    public static class ExtentionsClass {
        #region Controller扩展
        public static void MsgBox(this Controller controller, string msg, string key = "msgbox") {
            controller.ViewData[key] = msg;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserIp(this Controller controller) {
            return controller.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        #endregion

        #region ISession扩展
        public static string SessionKey(this ISession session) {
            return "MySession";
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool Set<T>(this ISession session, string key, T val) {
            if (string.IsNullOrWhiteSpace(key) || val == null) { return false; }

            var strVal = JsonConvert.SerializeObject(val);
            var bb = Encoding.UTF8.GetBytes(strVal);
            session.Set(key, bb);
            return true;
        }

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key) {
            var t = default(T);
            if (string.IsNullOrWhiteSpace(key)) { return t; }

            if (session.TryGetValue(key, out byte[] val)) {
                var strVal = Encoding.UTF8.GetString(val);
                t = JsonConvert.DeserializeObject<T>(strVal);
            }
            return t;
        }
        #endregion
    }
}