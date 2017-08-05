using System;
using System.Collections.Generic;

namespace LX.NETCoreTest.Model.Models
{
    public partial class ToContentFiles
    {
        public int Id { get; set; }
        public int? ContentId { get; set; }
        public string MinPic { get; set; }
        public string MaxPic { get; set; }
        public int? ZanNum { get; set; }

        public virtual ToContent Content { get; set; }
    }
}
