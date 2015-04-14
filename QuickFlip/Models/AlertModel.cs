using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int OfferId { get; set; }
        public AlertType Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}