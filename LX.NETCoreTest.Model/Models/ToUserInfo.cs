using System;
using System.Collections.Generic;

namespace LX.NETCoreTest.Model.Models
{
    public partial class ToUserInfo
    {
        public ToUserInfo()
        {
            ToUserLog = new HashSet<ToUserLog>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
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
        public DateTime? LoginTime { get; set; }
        public string Ips { get; set; }
        public int LevelNum { get; set; }

        public virtual ICollection<ToUserLog> ToUserLog { get; set; }
    }
}
