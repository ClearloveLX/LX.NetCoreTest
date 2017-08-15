using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LX.NETCoreTest.Web.Views {
    /// <summary>
    /// 测试功能
    /// </summary>
    public class PyStudioController : Controller {
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// 区域选择
        /// </summary>
        /// <returns></returns>
        public IActionResult Area() {

            return View();
        }
    }
}