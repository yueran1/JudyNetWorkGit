using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class ContactModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "姓名 (必填)")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email (必填)")]
        public string Email { get; set; }

        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Display(Name = "标题")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "内容 (必填)")]
        public string Message { get; set; }
    }
}
