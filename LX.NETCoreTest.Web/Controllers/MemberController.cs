using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LX.NETCoreTest.Model.Models;
using static LX.NETCoreTest.Common.EnumHelper;
using LX.NETCoreTest.Model.PyClass;
using LX.NETCoreTest.Common;
using LX.NETCoreTest.Web.Extends;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace LX.NETCoreTest.Web.Controllers {
    public class MemberController : Controller {
        private readonly PyStudio_NetCoreContext _context;
        private readonly PySelfSetting _selfSetting;
        private readonly IMemoryCache _cache;


        public MemberController(PyStudio_NetCoreContext context, IOptions<PySelfSetting> selfSetting, IMemoryCache cache) {
            _context = context;
            _selfSetting = selfSetting.Value;
            _cache = cache;
        }

        #region Resister

        /// <summary>
        /// 注册GET
        /// </summary>
        /// <returns></returns>
        public IActionResult Register() {
            return View();
        }

        /// <summary>
        /// 注册POST
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,UserPwd,ComfirmPwd")] PyRegisterUser loginUser) {
            if (ModelState.IsValid) {
                #region 验证
                if (_context.ToUserInfo.Any(b => b.UserName.ToUpper() == loginUser.UserName.Trim().ToUpper())) {
                    this.MsgBox("已经存在相同的账号！");
                    return View(loginUser);
                }
                #endregion

                #region Create
                ToUserInfo userInfo = new ToUserInfo();
                userInfo.UserName = loginUser.UserName.Trim();
                userInfo.UserPwd = PublicClass._Md5(loginUser.UserPwd.Trim());
                userInfo.NickName = userInfo.UserName;
                userInfo.Status = (int)EmUserStatus.启用;
                userInfo.CreateTime = DateTime.Now;
                userInfo.LevelNum = (int)EmLevelNum.注册;
                userInfo.Ips = this.GetUserIp();
                userInfo.HeadPhoto = "/images/pystudio.png";
                userInfo.Sex = false;
                _context.Add(userInfo);

                var result = await _context.SaveChangesAsync();

                if (result > 0) {
                    var pyUserInfo = new PyUserInfo {
                        Id = userInfo.Id,
                        UserName = userInfo.UserName,
                        NickName = userInfo.NickName,
                        Addr = userInfo.Addr,
                        Birthday = userInfo.Birthday,
                        Blog = userInfo.Blog,
                        CreateTime = userInfo.CreateTime,
                        Email = userInfo.Email,
                        HeadPhoto = userInfo.HeadPhoto,
                        Introduce = userInfo.Introduce,
                        Ips = userInfo.Ips,
                        LevelNum = userInfo.LevelNum,
                        Sex = userInfo.Sex,
                        Tel = userInfo.Tel,
                        Status = userInfo.Status,
                        LoginTime = DateTime.Now
                    };
                    HttpContext.Session.Set<PyUserInfo>(HttpContext.Session.SessionKey(), pyUserInfo);

                    if (!string.IsNullOrWhiteSpace(pyUserInfo.Ips)) {
                        _context.ToUserLog.Add(new ToUserLog {
                            CodeId = (int)EmLogCode.登录,
                            CreateTime = DateTime.Now,
                            Des = $"IP:{pyUserInfo.Ips},登录时间:{pyUserInfo.LoginTime.ToString("yyyy-MM-dd HH:mm:ss")}",
                            UserId = userInfo.Id
                        });
                    }

                    _context.ToUserLog.Add(new ToUserLog {
                        CodeId = (int)EmLogCode.积分,
                        CreateTime = DateTime.Now,
                        Des = $"【注册】+{ (int)EmLevelNum.注册}",
                        UserId = userInfo.Id
                    });

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                #endregion

                this.MsgBox("注册失败，请稍后重试。");
                return View(loginUser);
            }
            return View(loginUser);
        }
        #endregion

        #region Login

        /// <summary>
        /// 登录GET
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>

        public IActionResult Login(string returnUrl = null) {
            //获取Session
            var userInfo = HttpContext.Session.Get<PyUserInfo>(HttpContext.Session.SessionKey());
            if (userInfo != null) {
                if (string.IsNullOrWhiteSpace(returnUrl)) {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else {
                    Redirect(returnUrl);
                }
                this.MsgBox(returnUrl, "returnUrl");
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,UserPwd,ReturnUrl")] PyLoginUser loginUser) {
            if (ModelState.IsValid) {
                #region Valid
                var md5Pwd = PublicClass._Md5(loginUser.UserPwd.Trim());
                var userInfo = await _context.ToUserInfo.SingleOrDefaultAsync(b => b.UserName.Equals(loginUser.UserName, StringComparison.CurrentCultureIgnoreCase) && b.UserPwd.Equals(md5Pwd));
                if (userInfo == null) {
                    this.MsgBox("账号或密码错误！");
                    return View(loginUser);
                }
                else if (userInfo.Status == (int)EmUserStatus.禁用) {
                    this.MsgBox("该账号已被禁用，或许你可以重新注册一个账号或者联系管理员！");
                    return View(loginUser);
                }
                #endregion

                #region 更新登录信息
                userInfo.Ips = this.GetUserIp();
                userInfo.LoginTime = DateTime.Now;
                userInfo.LevelNum += (int)EmLevelNum.登录;

                //记录Session
                var pyUserInfo = new PyUserInfo {
                    Id = userInfo.Id,
                    UserName = userInfo.UserName,
                    NickName = userInfo.NickName,
                    Addr = userInfo.Addr,
                    Birthday = userInfo.Birthday,

                    Blog = userInfo.Blog,
                    CreateTime = userInfo.CreateTime,
                    Email = userInfo.Email,
                    HeadPhoto = userInfo.HeadPhoto,
                    Introduce = userInfo.Introduce,

                    Ips = userInfo.Ips,
                    LevelNum = userInfo.LevelNum,
                    Sex = userInfo.Sex,
                    Tel = userInfo.Tel,
                    Status = userInfo.Status,

                    LoginTime = Convert.ToDateTime(userInfo.LoginTime)
                };

                HttpContext.Session.Set<PyUserInfo>(HttpContext.Session.SessionKey(), pyUserInfo);

                if (!string.IsNullOrWhiteSpace(pyUserInfo.Ips)) {
                    _context.ToUserLog.Add(new ToUserLog {
                        CodeId = (int)EmLogCode.登录,
                        CreateTime = DateTime.Now,
                        Des = $"IP:{pyUserInfo.Ips},登录时间:{pyUserInfo.LoginTime.ToString("yyyy-MM-dd hh:mm:ss")}",
                        UserId = userInfo.Id
                    });
                }

                _context.ToUserLog.Add(new ToUserLog {
                    CodeId = (int)EmLogCode.积分,
                    CreateTime = DateTime.Now,
                    Des = $"【登录】  +{(int)EmLevelNum.登录}",
                    UserId = userInfo.Id
                });
                await _context.SaveChangesAsync();

                if (string.IsNullOrWhiteSpace(loginUser.ReturnUrl)) {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else {
                    return Redirect(loginUser.ReturnUrl);
                }
                #endregion
            }
            return View(loginUser);
        }


        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLoginInfo() {
            var data = new PyStudioData();
            var userInfo = HttpContext.Session.Get<PyUserInfo>(HttpContext.Session.SessionKey());

            if (userInfo != null) {
                data.Data = userInfo;
                data.IsOk = true;
            }
            data.Msg = data.IsOk ? "已登录" : "未登录";
            return Json(data);
        }
        #endregion

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns>登陆页面</returns>
        [HttpGet]
        public IActionResult LoginOut() {
            HttpContext.Session.Remove(HttpContext.Session.SessionKey());
            return RedirectToAction(nameof(MemberController.Login));
        }

        #region 忘记密码

        public IActionResult ForgetPassword() {
            return View();
        }

        /// <summary>
        /// 提交忘记密码内容
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                this.MsgBox("邮箱必填！");
                return View();
            }

            email = email.Trim().ToLower();

            if (email.Length >= 50 || email.Length <= 3) {
                this.MsgBox("邮箱长度不符！");
                return View();
            }
            else if (!email.Contains("@")) {
                this.MsgBox("邮箱格式不正确！");
                return View();
            }

            var user = await _context.ToUserInfo.SingleOrDefaultAsync(b => b.Email.ToLower() == email);
            if (user == null) {
                this.MsgBox("找不到绑定该邮箱的账号！");
                return View();
            }
            else if (user.Status == (int)EmUserStatus.禁用) {
                this.MsgBox("该绑定邮箱的账号已被禁用，可以通过发送邮件至：gk1213656215@outlook.com 联系客服");
                return View();
            }

            var timeOut = 10;
            var now = DateTime.Now.AddMinutes(timeOut);
            var expires = now.ToString("yyyy-MM-dd hh:mm");
            var token = PublicClass._Md5($"{expires}-{email}-{Request.Host.Host}");
            var appUrl = $"http://{Request.Host.Host}:{Request.Host.Port}";
            var comfirmUrl = $"{appUrl}/Member/ConfirmPassword?expire={expires}&token={token}&={email}&t=0.{now.ToString("ssfff")}";

            //读取模版
            var tpl = await PublicClass._GetHtmlTpl(EmEmailTpl.MsgBox, _selfSetting.EmailTplPath);
            if (string.IsNullOrWhiteSpace(tpl)) {
                this.MsgBox("发送绑定邮箱失败，请稍后重试。");
                return View();
            }

            tpl = tpl.Replace("{name}", "尊敬的用户").Replace("{content}", $"您正在使用<a href='{appUrl}'>爱留图网</a>邮箱重置密码功能，请点击以下链接确认绑定邮箱<a href='{comfirmUrl}'>{comfirmUrl}</a>；注意该地址有效时间{timeOut}分钟。");

            //发送
            var isOk = PublicClass._SendEmail(
               new Dictionary<string, string> {
                 { "尊敬的用户",email}
               },
               "爱留图 - 重置密码",
               tpl);

            this.MsgBox(isOk ? "已给您邮箱发送了重置密码邮件，请收件后点击重置密码链接地址。" : "发送绑定邮件失败，请稍后重试！");

            return View();
        }

        /// <summary>
        /// 接受重置密码通知
        /// </summary>
        /// <returns></returns>
        public IActionResult ConfirmPassword(string expire, string token, string email, string t) {
            if (string.IsNullOrWhiteSpace(expire) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email) || !email.Contains("@") || string.IsNullOrWhiteSpace(t)) {
                return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "无效的请求。" });
            }
            else if (t.Length != 7) {
                return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "无效的请求。" });
            }

            email = email.Trim().ToLower();
            if (!DateTime.TryParse(expire, out var expires)) { return RedirectToAction(nameof(HomeController.Error), "home", new { msg = "无效的请求！" }); }
            else if (expires.AddMinutes(30) > DateTime.Now) {
                return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "请求已过期，重新操作！" });
            }

            var compareToken = PublicClass._Md5($"{expire}-{email}-{Request.Host.Host}");
            if (!token.Equals(compareToken)) { return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "验证失败，无效的请求！" }); }

            var user = _context.ToUserInfo.SingleOrDefault(b => b.Email.ToLower() == email);
            if (user == null) { return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "不存在该绑定邮箱的账号！" }); }
            else if (user.Status == (int)EnumHelper.EmUserStatus.禁用) {
                return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "该绑定邮箱的账号已被禁用，可以通过发送邮件至：gk1213656215@outlook.com联系客服！" });
            }

            var key = $"checkConfirmPwd{email}";
            if (!_cache.TryGetValue<PyUserInfo>(key, out var result)) {
                _cache.Set<PyUserInfo>(key, new PyUserInfo { Id = user.Id, Email = email }, TimeSpan.FromMinutes(10));
            }

            return View(new PyRegisterUser { UserName = email });
        }

        /// <summary>
        /// 提交重置的密码
        /// </summary>
        /// <param name="registUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPassword([Bind("UserName", "UserPwd", "ComfirmPwd")]PyRegisterUser registUser) {
            if (ModelState.IsValid) {
                if (string.IsNullOrWhiteSpace(registUser.UserPwd)) {
                    this.MsgBox("密码不能为空！");
                    return View(registUser);
                }
                else if (string.IsNullOrWhiteSpace(registUser.ComfirmPwd)) {
                    this.MsgBox("确认密码不能为空！");
                    return View(registUser);
                }
                else if (registUser.UserPwd != registUser.ComfirmPwd) {
                    this.MsgBox("密码和确认密码不相同！");
                    return View(registUser);
                }

                var key = $"checkConfirmPwd{registUser.UserName}";
                if (!_cache.TryGetValue<PyUserInfo>(key, out var checkUser)) {
                    return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "请求已过期，重新操作！" });
                }

                var user = _context.ToUserInfo.Where(b => b.Id == checkUser.Id && b.Email == checkUser.Email).SingleOrDefault();
                if (user == null) {
                    _cache.Remove(key);
                    return RedirectToAction(nameof(HomeController.Error), "Home", new { msg = "重置密码失败，请稍后重试！" });
                }

                if (user.UserPwd==PublicClass._Md5(registUser.UserPwd.Trim())) {
                    this.MsgBox("新密码与旧密码不能相同！请确认。");
                    return View(registUser);
                }

                user.UserPwd = PublicClass._Md5(registUser.UserPwd.Trim());
                var result = await _context.SaveChangesAsync();
                if (result > 0) {
                    _cache.Remove(key);
                    this.MsgBox("重置密码成功！");
                }
                else {
                    this.MsgBox("重置密码失败！");
                }
            }
            return View(registUser);
        }
        #endregion
    }
}