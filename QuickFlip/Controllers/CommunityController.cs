using QuickFlip.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickFlip.Controllers
{
    public class CommunityController : Controller
    {
        public ActionResult Home(SchoolName id)
        {
            ViewBag.School = BusinessLogic.GetFullSchoolName(id);
            return View();
        }
    }
}
