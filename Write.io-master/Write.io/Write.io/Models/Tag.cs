using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Write.io.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Posts {get; set; }
    }
}