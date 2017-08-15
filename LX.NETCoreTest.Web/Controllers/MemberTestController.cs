using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LX.NETCoreTest.Model.Models;
using LX.NETCoreTest.Common;

namespace LX.NETCoreTest.Web.Controllers {
    public class MemberTestController : Controller {
        private readonly PyStudio_NetCoreContext _db;
        private readonly PyStudioClass _py = new PyStudioClass();
        public MemberTestController(PyStudio_NetCoreContext db) {
            _db = db;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult Create() {
            ViewData["Code"] = _py.TranCode(_py.GetCode(_db.InfoMember.Max(m => m.MemberCode)));
            return View();
        }

        public IActionResult BaiduMap() {
            return View();
        }
    }
}