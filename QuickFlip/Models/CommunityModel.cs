using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace QuickFlip.Models
{
    public class Community
    {
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DefaultMeetingLocation { get; set; }
        public HtmlImage Logo { get; set; }

    }
}