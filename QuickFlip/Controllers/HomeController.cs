using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using QuickFlip.BusinessLayer;
using QuickFlip.Models;

namespace QuickFlip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }


        // yes, this shouldn't go here. but this is easier than fighting with 
        // simple membership and redirects under the account controller
        #region VerifyEmail

        public ActionResult AccountCreated(string id)
        {
            var userNameBytes = System.Convert.FromBase64String(id);
            string userName = System.Text.Encoding.UTF8.GetString(userNameBytes);

            ViewData["UserName"] = userName;
            ViewData["Password"] = TempData["Password"];
            ViewData["FirstAttempt"] = TempData["FirstAttempt"];

            return View();
        }

        public ActionResult VerifyEmail(string id)
        {
            var userNameBytes = System.Convert.FromBase64String(id);
            string userName = System.Text.Encoding.UTF8.GetString(userNameBytes);

            ViewData["UserName"] = userName;
            ViewData["Password"] = TempData["Password"];
            ViewData["FirstAttempt"] = TempData["FirstAttempt"];

            return View();
        }

        [HttpPost]
        public ActionResult SubmitNonce()
        {
            string nonce = Request.Form["Nonce"].ToString();
            string userName = Request.Form["UserName"].ToString();
            string password = Request.Form["Password"].ToString();

            if (BusinessLogic.CheckNonce(nonce, userName))
            {
                BusinessLogic.SetEmailAsVerified(userName);
                WebSecurity.Login(userName, password);

                return RedirectToAction("Index");
            }
            else
            {
                var userNameBytes = System.Text.Encoding.UTF8.GetBytes(userName);
                string b64EncodedUserName = Convert.ToBase64String(userNameBytes);

                // store password in clear text, for now...
                TempData["Password"] = password;
                TempData["FirstAttempt"] = false;

                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        #endregion
    }
}
