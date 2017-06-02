using BusinessLogic;
using JudyDou.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class FormModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "姓名 (必填)")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email (必填)")]
        public string Email { get; set; }

        [Display(Name = "联系电话")]
        public string Phone { get; set; }
    }

    public class BuyerFormModel : FormModel
    {
        public List<FormProperty> Properties { get; set; }

        public BuyerFormModel()
        {
            Properties = new List<FormProperty>();

            Properties.Add(new FormProperty(-1, "第一关注因素"));
            Properties.Add(new FormProperty(-1, "第二关注因素"));
            Properties.Add(new FormProperty(-1, "第三关注因素"));

            for (int i = 0; i < Constants.PropertyFactors.Length; i++)
                Properties.Add(new FormProperty(i, Constants.PropertyFactors[i]));
        }
    }

    public class FormProperty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [UIHint("PropertyFactor")]
        public string Property { get; set; }

        public FormProperty()
        {
        }

        public FormProperty(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class SellerFormModel : FormModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "出售房屋地址 (必填)")]
        public string Address { get; set; }

        public FormProperty Property { get; set; }

        public SellerFormModel()
        {
            Property = new FormProperty(3, Constants.PropertyFactors[3]);
        }
    }
}
