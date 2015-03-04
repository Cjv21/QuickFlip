using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickFlip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // yes, this shouldn't go here. but this is easier than fighting with 
        // simple membership and redirects under the account controller
        #region VerifyEmail

        public ActionResult VerifyEmail(string id)
        {
            var userNameBytes = System.Convert.FromBase64String(id);
            string userName = System.Text.Encoding.UTF8.GetString(userNameBytes);

            TempData["UserName"] = userName;

            return View();
        }

        [HttpPost]
        public ActionResult SubmitNonce()
        {
            string nonce = Request.Form["Nonce"].ToString();

            var userName = TempData["UserName"].ToString();

            return View();
        }

        public ActionResult AccountCreated(string id)
        {
            var userNameBytes = System.Convert.FromBase64String(id);
            string userName = System.Text.Encoding.UTF8.GetString(userNameBytes);

            TempData["UserName"] = userName;

            return View();
        }

        public ActionResult EmailVerified()
        {
            return View();
        }

        #endregion
    }
}
