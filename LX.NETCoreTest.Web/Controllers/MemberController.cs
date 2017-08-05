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
        #endregion

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
    }
}