using BusinessLogic;
using JudyDou.Helper;
using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Controllers
{
    public class TeamController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Financing()
        {
            return View();
        }

        public ActionResult Inspection()
        {
            return View();
        }

        public ActionResult Evaluation()
        {
            EvaluationModel model = new EvaluationModel();

            model.Evaluation = new Evaluation();
            model.Evaluations = unitOfWork.EvaluationRepository.Get(0, int.MaxValue);

            return View(model);
        }

        [HttpPost]
        public ActionResult Evaluation(EvaluationModel model)
        {
            if (UserId > 0 && !model.Anonymous)
                model.Evaluation.UserId = UserId;
            else
                model.Evaluation.UserId = null;

            unitOfWork.EvaluationRepository.Update(model.Evaluation);

            return RedirectToAction("Evaluation");
        }
    }
}
