using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class OfferMedia
    {
        public int OfferMediaId { get; set; }
        public int OfferId { get; set; }
        public string B64EncodedImage { get; set; }
    }
}