using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class PagerModel
    {
        public int StartAt { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
    }
}
