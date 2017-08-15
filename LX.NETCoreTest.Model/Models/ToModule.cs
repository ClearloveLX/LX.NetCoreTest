using System;
using System.Collections.Generic;

namespace LX.NETCoreTest.Model.Models
{
    public partial class ToModule
    {
        public ToModule()
        {
            ToContent = new HashSet<ToContent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; }
        public int? SortNum { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual ICollection<ToContent> ToContent { get; set; }
    }
}
