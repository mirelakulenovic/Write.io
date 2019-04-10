using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Write.io.Models
{
    public class Theme
    {
        [Key]
        public string Name { get; set; }
    }
}