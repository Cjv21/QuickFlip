using QuickFlip.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QuickFlip.Models;

namespace QuickFlip.Controllers
{
    public class CommunityController : Controller
    {
        public ActionResult Home(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            return View(comm);
        }

    }
}
