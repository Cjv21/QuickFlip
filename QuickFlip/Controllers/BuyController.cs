using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuickFlip.BusinessLayer;
using QuickFlip.Models;

namespace QuickFlip.Controllers
{
    public class BuyController : Controller
    {
        public ActionResult Index(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            return View(comm);
        }

        public ActionResult MakeBuyPost(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            return View(comm);
        }

        [HttpPost]
        public ActionResult SubmitPost()
        {
            string postTitle = String.Format("{0}", Request.Form["PostTitle"]);

            int? maxPrice = (String.IsNullOrWhiteSpace(Request.Form["MaxPrice"]) ? (int?)null : Convert.ToInt32(Request.Form["MaxPrice"]));

            string postDescription = String.Format("{0}", Request.Form["PostDescription"]);

            // BusinessLogic.CreatePost(postTitle, postDescription, WebSecurity.CurrentUserId, maxPrice, PostType.Buy);

            return View();
        }
    }
}
