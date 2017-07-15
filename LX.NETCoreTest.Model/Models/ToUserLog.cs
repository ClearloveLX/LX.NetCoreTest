using System;
using System.Collections.Generic;

namespace LX.NETCoreTest.Model.Models
{
    public partial class ToUserLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Des { get; set; }
        public int? CodeId { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual ToUserInfo User { get; set; }
    }
}
