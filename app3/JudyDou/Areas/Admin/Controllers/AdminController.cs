using BusinessLogic;
using JudyDou.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        [HttpPost]
        public ActionResult WebSiteExpiry()
        {
            return Json(unitOfWork.ListingRepository.GetSeting("Expiry Date").Date.Value.ToString("yyyy MMM dd"), JsonRequestBehavior.AllowGet);
        }
    }
}
