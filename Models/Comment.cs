using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name ="Tên người dùng")]
    
        public string UserId { get; set; }

        [Display(Name ="Quyển sách hiện tại")]
        [ForeignKey("Book")]
        public int BookID { get; set; }
        public Book Book { get; set; }

        [Display(Name ="Nội dung")]
        public string Content { get; set; }

        [Display(Name ="Thời gian")]
        public DateTime Time { get; set; }
    }
}