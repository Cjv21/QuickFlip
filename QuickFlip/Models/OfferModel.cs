using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Accepted { get; set; }
        public OfferMedia OfferMedia { get; set; }
    }
}