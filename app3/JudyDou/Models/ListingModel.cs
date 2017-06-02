using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class ListingModel : PagerModel
    {
        public List<Listing> Properties { get; set; }
        public Listing Property { get; set; }
        public ListingCN PropertyCN { get; set; }

        public string Title { get; set; }
        public List<string> Photo { get; set; }

        [UIHint("ListingStatus")]
        public int Status { get; set; }

        public bool Chinese { get; set; }

        public ListingModel()
        {
            Chinese = true;
        }
    }
}
