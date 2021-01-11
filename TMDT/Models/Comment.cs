using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMDT.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Display(Name = "CommentID")]
        public int BookID { get; set; }
        [Display(Name = "BookID")]
        public string UserId { get; set; }
        [Display(Name = "UserID")] 
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public ICollection<Book> Book { get; set; }
    }
}