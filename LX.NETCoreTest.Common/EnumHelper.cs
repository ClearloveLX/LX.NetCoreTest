using System;
using System.Collections.Generic;
using System.Text;

namespace LX.NETCoreTest.Common {
    /// <summary>
    /// 枚举支援类
    /// </summary>
    public class EnumHelper {
        /// <summary>
        /// 会员状态
        /// </summary>
        public enum EmUserStatus {
            禁用 = 0, 启用 = 1
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum EmLogCode {
            普通 = 0,
            登录 = 1,
            积分 = 2
        }

        /// <summary>
        /// 积分增加规则
        /// </summary>
        public enum EmLevelNum {
            登录 = 1,
            注册 = 2,
            修改头像 = 3,
            点赞 = 4,
            上传图片 = 5,

            绑定邮箱 = 6,
            绑定手机号码 = 7
        }
    }
}
