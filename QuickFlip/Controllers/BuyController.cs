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

    }
}
