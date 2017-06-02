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
    public class FormController : BaseController
    {
        public ActionResult Buyer()
        {
            BuyerFormModel model = new BuyerFormModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Buyer(BuyerFormModel model, FormCollection collection)
        {
            if (!EmailHelper.ValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid Email.");
            }

            if (ModelState.IsValid)
            {
                string environment = collection["Properties[11].Property"].Replace("false,", "").Replace(",false", "").Replace("false", "N/A");
                string mind = collection["Properties[17].Property"].Replace("false,", "").Replace(",false", "").Replace("false", "N/A");
                string factors = "";

                foreach (var item in model.Properties)
                {
                    switch (item.Id)
                    {
                        case 8:
                            if (!environment.Equals("N/A"))
                                factors += "<tr><td style='width: 10%;'>" + item.Name + ":</td><td>" + environment + "</td></tr>";
                            break;

                        case 14:
                            if (!mind.Equals("N/A"))
                                factors += "<tr><td style='width: 10%;'>" + item.Name + ":</td><td>" + mind + "</td></tr>";
                            break;

                        default:
                            if (item.Property != null && !item.Property.Equals("N/A"))
                                factors += "<tr><td style='width: 10%;'>" + item.Name + ":</td><td>" + item.Property + "</td></tr>";
                            break;
                    }
                }

                Email email = new Email();

                email.EmailAddress = "judydouca@hotmail.com";
                email.Replace("#Name#", model.Name);
                email.Replace("#Email#", model.Email);
                email.Replace("#Phone#", model.Phone);
                email.Replace("#Factors#", factors);

                EmailHelper.SendEmail(email, "JudyDou Website Buyer Form", "buyer.html");

                return RedirectToAction("MessageSent", "Home");
            }

            return View(model);
        }

        public ActionResult Seller()
        {
            SellerFormModel model = new SellerFormModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Seller(SellerFormModel model)
        {
            if (!EmailHelper.ValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Please enter a valid Email.");
            }

            if (ModelState.IsValid)
            {
                Email email = new Email();

                email.EmailAddress = "judydouca@hotmail.com";
                email.Replace("#Name#", model.Name);
                email.Replace("#Email#", model.Email);
                email.Replace("#Phone#", model.Phone);
                email.Replace("#Property#", model.Property.Property);
                email.Replace("#Address#", model.Address);

                EmailHelper.SendEmail(email, "JudyDou Website Seller Form", "seller.html");

                return RedirectToAction("MessageSent", "Home");
            }

            return View(model);
        }
    }
}
