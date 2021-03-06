﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? CommunityId { get; set; }
        public string Email { get; set; }
        public Int64? Phone { get; set; }
        public Carrier? Carrier { get; set; }
        public AlertMode AlertMode { get; set; }
        public string B64EncodedImage { get; set; }
    }
}