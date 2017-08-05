using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LX.NETCoreTest.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(string msg = null) {
            this.MsgBox(msg ?? "访问出问题了，开发人员正从火星赶回来修复，请耐心等待！");
            return View();
        }
    }
}
