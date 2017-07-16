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

namespace LX.NETCoreTest.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly PyStudio_NetCoreContext _context;

        /// <summary>
        /// ×¢²á
        /// </summary>
        /// <returns></returns>
        public IActionResult Register() {
            return View();
        }

        /// <summary>
        /// ×¢²áPOST
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,UserPwd,ComfirmPwd")] PyRegisterUser loginUser) {
            if (ModelState.IsValid) {
                #region ÑéÖ¤
                if (_context.ToUserInfo.Any(b => b.UserName.ToUpper() == loginUser.UserName.Trim().ToUpper())) {
                    this.MsgBox("ÒÑ¾­´æÔÚÏàÍ¬µÄÕËºÅ£¡");
                    return View(loginUser);
                }
                #endregion

                #region Create
                ToUserInfo userInfo = new ToUserInfo();
                userInfo.UserName = loginUser.UserName.Trim();
                userInfo.UserPwd = PublicClass._Md5(loginUser.UserPwd.Trim());
                userInfo.NickName = userInfo.UserName;
                userInfo.Status = (int)EmUserStatus.ÆôÓÃ;
                userInfo.CreateTime = DateTime.Now;
                userInfo.LevelNum = (int)EmLevelNum.×¢²á;
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
                            CodeId = (int)EmLogCode.µÇÂ¼,
                            CreateTime = DateTime.Now,
                            Des = $"IP:{pyUserInfo.Ips},µÇÂ¼Ê±¼ä:{pyUserInfo.LoginTime.ToString("yyyy-MM-dd HH:mm:ss")}",
                            UserId = userInfo.Id
                        });
                    }

                    _context.ToUserLog.Add(new ToUserLog {
                        CodeId = (int)EmLogCode.»ý·Ö,
                        CreateTime = DateTime.Now,
                        Des = $"¡¾×¢²á¡¿+{ (int)EmLevelNum.×¢²á}",
                        UserId = userInfo.Id
                    });

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                #endregion

                this.MsgBox("×¢²áÊ§°Ü£¬ÇëÉÔºóÖØÊÔ¡£");
                return View(loginUser);
            }
            return View(loginUser);
        }
    }
}