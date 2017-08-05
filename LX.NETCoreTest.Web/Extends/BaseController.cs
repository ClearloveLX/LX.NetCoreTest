using LX.NETCoreTest.Model.PyClass;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using LX.NETCoreTest.Web.Controllers;

namespace LX.NETCoreTest.Web.Extends {
    public class BaseController : Controller {
        public PyUserInfo _MyUserInfo;

        public override void OnActionExecuting(ActionExecutingContext context) {
            _MyUserInfo = context.HttpContext.Session.Get<PyUserInfo>(context.HttpContext.Session.SessionKey());
            if (_MyUserInfo == null) {
                context.Result = new RedirectToActionResult(nameof(MemberController.Login), "Member", new { ReturnUrl = context.HttpContext.Request.Path });
            }
            ViewData["MyUserInfo"] = _MyUserInfo;
            base.OnActionExecuting(context);
        }
    }
}
