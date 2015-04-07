using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickFlip.Models
{
    public class Post
    {		
        public int PostId { get; set; }
        public int UserId { get; set; }
		public int CommunityId { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
        public List<string> Tags { get; set; }
		public int? RequiredPrice { get; set; }
		public PostType PostType  { get; set; }
		public AuctionType AuctionType { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool Settled { get; set; }
        public List<Category> Categories { get; set; }
        public List<Offer> Offers { get; set; }
        public Offer BestOffer { get; set; }
        public PostMedia PostMedia { get; set; }
    }
}