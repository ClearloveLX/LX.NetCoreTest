using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LX.NETCoreTest.Model.PyClass {
    /// <summary>
    /// 自定义配置
    /// </summary>
    public class PySelfSetting {
        /// <summary>
        /// 头像图片保存地址 
        /// </summary>
        public string UpHeadPhotoPath { get; set; }

        /// <summary>
        /// 头像图片访问地址 
        /// </summary>
        public string ViewHeadPhotoPath { get; set; }

        /// <summary>
        /// 内容图片保存地址 
        /// </summary>
        public string UpContentPhotoPath { get; set; }

        /// <summary>
        /// 查看内容图片保存地址 
        /// </summary>
        public string ViewContentPhotoPath { get; set; }

        /// <summary>
        /// 邮件模板文件夹路径 
        /// </summary>
        public string EmailTplPath { get; set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public string DbLink { get; set; }
    }

    /// <summary>
    /// 注册实体
    /// </summary>
    public class PyRegisterUser {
        [Required(AllowEmptyStrings = false, ErrorMessage = "账号长度范围6-30字符！")]
        [Display(Prompt = "邮箱/手机号/6-30字符")]
        [RegularExpression(@"[^\s]{6,30}", ErrorMessage = "账号长度范围6-30字符。")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "密码长度范围6-20字符！")]
        [DataType(DataType.Password)]
        [Display(Prompt = "密码长度范围6-20字符！")]
        [RegularExpression(@"[^\s]{6,20}", ErrorMessage = "密码长度范围6-20字符。")]
        public string UserPwd { get; set; }

        [Compare("UserPwd", ErrorMessage = "密码与确认密码不相同！")]
        [DataType(DataType.Password)]
        [Display(Prompt = "必须与密码相同")]
        public string ComfirmPwd { get; set; }
    }

    /// <summary>
    /// 登录实体
    /// </summary>
    public class PyLoginUser {
        [Required(AllowEmptyStrings = false, ErrorMessage = "账号长度范围6-30字符！")]
        [Display(Prompt = "邮箱/手机号/6-30字符")]
        [RegularExpression(@"[^\s]{6,30}", ErrorMessage = "账号长度范围6-30字符。")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "密码长度范围6-20字符！")]
        [DataType(DataType.Password)]
        [Display(Prompt = "密码长度范围6-20字符！")]
        [RegularExpression(@"[^\s]{6,20}", ErrorMessage = "密码长度范围6-20字符。")]
        public string UserPwd { get; set; }

        /// <summary>
        /// 回跳地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class PyUserInfo {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Tel { get; set; }
        public bool Sex { get; set; }
        public string Introduce { get; set; }
        public string HeadPhoto { get; set; }
        public string Birthday { get; set; }
        public string Addr { get; set; }
        public string Blog { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LoginTime { get; set; }
        public string Ips { get; set; }
        public int LevelNum { get; set; }
        public int LevelNumVal {
            get {
                var val = LevelNum / 50;

                return val >= 1 ? val : 1;
            }
        }
    }

    /// <summary>
    /// 接口暴露实体
    /// </summary>
    public class PyStudioData {
        public bool IsOk { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
