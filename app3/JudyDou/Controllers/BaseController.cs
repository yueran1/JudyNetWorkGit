using BusinessLogic;
using JudyDou.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace JudyDou.Controllers
{
    public abstract class BaseController : Controller
    {
        protected UnitOfWork unitOfWork = new UnitOfWork();

        protected void SetSession(UserProfile user)
        {
            if (user.Status == (int)Constants.UserStatus.Disabled)
            {
                // user disabled, log out.
                WebSecurity.Logout();
                Session.Abandon();
                return;
            }

            UserId = user.Id;
            Username = user.UserName;
        }

        private void RenewSession()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    UserProfile user = unitOfWork.UserProfileRepository.GetByUserName(User.Identity.Name);
                    if (user != null && user.Status != (int)Constants.UserStatus.Disabled)
                    {
                        SetSession(user);
                    }
                }
            }
            catch
            {
            }
        }

        protected int UserId
        {
            get
            {
                if (Session["UserId"] == null)
                {
                    RenewSession();
                }

                try
                {
                    return int.Parse(Session["UserId"].ToString());
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                Session["UserId"] = value;
            }
        }

        protected string Username
        {
            get
            {
                if (Session["UserName"] == null)
                {
                    RenewSession();
                }

                try
                {
                    return Session["UserName"].ToString();
                }
                catch
                {
                    return "";
                }
            }

            set
            {
                Session["UserName"] = value;
            }
        }
    }
}
