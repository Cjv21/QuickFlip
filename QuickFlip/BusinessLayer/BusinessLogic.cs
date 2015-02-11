using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.BusinessLayer
{
    public class BusinessLogic
    {
        public static string GetFullSchoolName(SchoolName school)
        {
            switch (school)
            {
                case SchoolName.IU: return "Indiana University";
                case SchoolName.MSU: return "Michigan State University";
                case SchoolName.NU: return "Northwestern University";
                case SchoolName.OSU: return "Ohio State University";
                case SchoolName.PSU: return "Penn State University";
                case SchoolName.PU: return "Purdue University";
                case SchoolName.UI: return "Indiana University";
                case SchoolName.UIUC: return "University of Illinois - Urbana Champaign";
                case SchoolName.UM: return "University of Michigan";
                case SchoolName.UMN: return "University of Minnesota";
                case SchoolName.UNL: return "University of Nebraska - Lincoln";
                case SchoolName.UWM: return "University of Western Mchigan";
                default: return String.Empty;
            }
        }
    }
}