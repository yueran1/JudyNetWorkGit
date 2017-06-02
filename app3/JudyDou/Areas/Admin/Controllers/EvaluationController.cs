using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Areas.Admin.Controllers
{
    public class EvaluationController : AdminController
    {
        //
        // GET: /Admin/Evaluation/

        public ActionResult Index()
        {
            EvaluationModel model = new EvaluationModel();

            model.Evaluations = unitOfWork.EvaluationRepository.Get(0, int.MaxValue);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            unitOfWork.EvaluationRepository.Delete(id);

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
