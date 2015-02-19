using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class PostMedia
    {
        public int PostMediaId { get; set; }
		public int PostId { get; set; }
        public string B64EncodedImage { get; set; }
    }
}