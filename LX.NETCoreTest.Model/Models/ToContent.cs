using System;
using System.Collections.Generic;

namespace LX.NETCoreTest.Model.Models
{
    public partial class ToContent
    {
        public ToContent()
        {
            ToContentFiles = new HashSet<ToContentFiles>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ModuleId { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public string MinPic { get; set; }
        public string MaxPic { get; set; }
        public int? ReadNum { get; set; }
        public int? ZanNum { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual ICollection<ToContentFiles> ToContentFiles { get; set; }
        public virtual ToModule Module { get; set; }
        public virtual ToUserInfo User { get; set; }
    }
}
