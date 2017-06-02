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
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Event()
        {
            return View();
        }

        public ActionResult Event2()
        {
            return View();
        }

        public ActionResult Event3()
        {
            return View();
        }

        public ActionResult Membership()
        {
            return View();
        }

        public ActionResult Testimonial()
        {
            return View();
        }

        public ActionResult Resources()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ContactModel model = new ContactModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            if (!EmailHelper.ValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid Email.");
            }

            if (ModelState.IsValid)
            {
                Email email = NewMethod();

                email.EmailAddress = "judydouca@hotmail.com";
                email.Replace("#Subject#", model.Subject);
                email.Replace("#Name#", model.Name);
                email.Replace("#Email#", model.Email);
                email.Replace("#Phone#", model.Phone);
                email.Replace("#Message#", model.Message.Replace("\r\n", "<br />"));

                EmailHelper.SendEmail(email, "JudyDou Website Contact", "contact.html");

                return RedirectToAction("MessageSent");
            }

            return View(model);
        }

        private static Email NewMethod()
        {
            return new Email();
        }

        public ActionResult MessageSent()
        {
            return View();
        }
    }
}
