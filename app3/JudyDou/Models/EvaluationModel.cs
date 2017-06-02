using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JudyDou.Models
{
    public class EvaluationModel : PagerModel
    {
        public List<Evaluation> Evaluations { get; set; }
        public Evaluation Evaluation { get; set; }

        [Display(Name = "匿名发表")]
        public bool Anonymous { get; set; }
    }
}
