using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        //
        // GET: /Admin/User/

        public ActionResult Index()
        {
            UserModel model = new UserModel();

            model.Users = unitOfWork.UserProfileRepository.Get(0, int.MaxValue, true);

            return View(model);
        }

        [HttpPost]
        public ActionResult Disable(int id, int status)
        {
            var user = unitOfWork.UserProfileRepository.Get(id, true);

            user.Status = status;

            unitOfWork.UserProfileRepository.Update(user);

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
