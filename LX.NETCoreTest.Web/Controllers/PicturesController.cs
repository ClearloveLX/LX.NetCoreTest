using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LX.NETCoreTest.Web.Controllers
{
    public class PicturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}