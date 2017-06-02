using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class UserModel : PagerModel
    {
        public List<UserProfile> Users { get; set; }
        public UserProfile User { get; set; }

        [UIHint("UserStatus")]
        public int Status { get; set; }
    }
}
