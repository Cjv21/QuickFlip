using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class Post
    {		
        public int PostId { get; set; }
		public int CommunityId { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string PostTitle { get; set; }
		public string Description { get; set; }
		public int RequiredPrice { get; set; }
		public PostType PostType  { get; set; }
		public AuctionType AuctionType { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<Category> Categories { get; set; }
        public PostMedia PostMedia { get; set; }
    }
}