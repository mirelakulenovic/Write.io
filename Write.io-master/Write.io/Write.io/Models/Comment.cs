using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Write.io.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string body { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime created { get; set; }

        public virtual ApplicationUser User {get; set; }
        public virtual Post Post {get; set; }
     
    }

    public class CreateCommentViewModel
    {
        public Comment Comment { get; set; }
        public string Message { get; set; }
    }
}