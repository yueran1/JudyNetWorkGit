using BusinessLogic;
using JudyDou.Helper;
using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Areas.Admin.Controllers
{
    public class ListingController : AdminController
    {
        //
        // GET: /Admin/Listing/

        public ActionResult Index()
        {
            ListingModel model = new ListingModel();

            model.Properties = unitOfWork.ListingRepository.Get(0, int.MaxValue);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            ListingModel model = new ListingModel();

            model.Property = unitOfWork.ListingRepository.Get(id, true);
            model.Status = model.Property.Status;

            return View(model);
        }

        [HttpPost]
        public ActionResult Details(ListingModel model)
        {
            Listing property = unitOfWork.ListingRepository.Get(model.Property.Id, true);

            property.Status = model.Status;

            unitOfWork.ListingRepository.Update(property);

            return RedirectToAction("Index");
        }

        public ActionResult Update(int id = 0)
        {
            ListingModel model = new ListingModel();

            model.Property = unitOfWork.ListingRepository.Get(id, true);
            model.PropertyCN = model.Property == null ? new ListingCN() : model.Property.ListingCN;
            model.Status = model.Property == null ? 0 : model.Property.Status;

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ListingModel model, IEnumerable<HttpPostedFileBase> files)
        {
            model.Property.Id = unitOfWork.ListingRepository.Update(model.Property);

            string imagePath = "/Images/Listing/" + model.Property.Id + "/";

            model.Property.Status = model.Status;
            model.Property.Image = imagePath + "1.jpg";

            unitOfWork.ListingRepository.Update(model.Property);

            if (!string.IsNullOrEmpty(model.PropertyCN.Description) || !string.IsNullOrEmpty(model.PropertyCN.Features) || !string.IsNullOrEmpty(model.PropertyCN.City))
            {
                model.PropertyCN.ListingId = model.Property.Id;
                unitOfWork.ListingRepository.UpdateCN(model.PropertyCN);
            }

            if (files != null && files.Count() > 0 && files.First() != null)
            {
                string path = Server.MapPath("~" + imagePath);

                for (int i = 0; i < files.Count(); i++)
                {
                    var file = files.ElementAt(i);
                    if (file != null)
                    {
                        string error = UploadHelper.ValidUploadFile(file);

                        if (error == null)
                        {
                            UploadHelper.UploadFile(path, file, (i + 1).ToString());
                        }
                        else
                        {
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
