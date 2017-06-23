using BusinessLogic;
using JudyDou.Helper;
using JudyDou.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JudyDou.Controllers
{
    public class ListingController : BaseController
    {
        public ActionResult Active()
        {
            UpdateListing();

            ListingModel model = new ListingModel();

            model.Title = "Active Listing";
            model.Status = (int)Constants.ListingStatus.Active;

            return Listing_Active(model);
        }

        public ActionResult Sold()
        {
            ListingModel model = new ListingModel();

            model.Title = "Sold Listing";
            model.Status = (int)Constants.ListingStatus.Sold;

            return Listing_Sold(model);
        }

        /* [HttpPost]
         public ActionResult Listing(ListingModel model)
         {
             model.Properties = GetListings(model.Status, model.Chinese);

             return View("Listing", model);
         }*/

        public ActionResult Listing_Active(ListingModel model)
        {
            model.Properties = GetListings(model.Status, model.Chinese);

            return View("Listing_Active", model);
        }

        public ActionResult Listing_Sold(ListingModel model)
        {
            model.Properties = GetListings(model.Status, model.Chinese);

            return View("Listing_Sold", model);
        }

        private List<Listing> GetListings(int status, bool chinese = true)
        {
            List<Listing> model = unitOfWork.ListingRepository.Get(0, int.MaxValue, status);

            if (chinese)
            {
                foreach (var item in model)
                {
                    var data = item.ListingCN;
                    if (data != null)
                    {
                        item.Description = data.Description == null ? item.Description : data.Description;
                        item.Features = data.Features == null ? item.Features : data.Features;
                        item.City = data.City == null ? item.City : data.City;
                    }
                }
            }

            return model;
        }

        public ActionResult Details(int id)
        {
            ListingModel model = new ListingModel();

            model.Property = GetListing(id);

            return GetImages(model);
        }

        [HttpPost]
        public ActionResult Details(ListingModel model)
        {
            model.Property = GetListing(model.Property.Id, model.Chinese);

            return GetImages(model);
        }

        private ActionResult GetImages(ListingModel model)
        {
            try
            {
                if (!model.Property.Image.Contains("default.jpg"))
                {
                    string path = "/Images/Listing/" + model.Property.Id + "/";
                    model.Photo = Directory.GetFiles(Server.MapPath("~" + path)).Select(i => path + Path.GetFileName(i)).ToList();
                }
            }
            catch
            {
                model.Photo = null;
            }

            return View("Details", model);
        }

        private Listing GetListing(int id, bool chinese = true)
        {
            Listing model = unitOfWork.ListingRepository.Get(id, false);

            if (chinese)
            {
                var data = model.ListingCN;
                if (data != null)
                {
                    model.Description = data.Description == null ? model.Description : data.Description;
                    model.Features = data.Features == null ? model.Features : data.Features;
                    model.City = data.City == null ? model.City : data.City;
                }
            }

            return model;
        }

        private void UpdateListing()
        {
            SiteSetting seting = unitOfWork.ListingRepository.GetSeting("Last Updated");
            CREAHelper.GetUpdated(unitOfWork, Server, seting.Date.Value);

            seting.Date = DateTime.Now;

            unitOfWork.ListingRepository.SetSeting(seting);
        }
    }
}
