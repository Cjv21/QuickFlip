using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using QuickFlip.BusinessLayer;
using QuickFlip.Models;
using System.Transactions;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using QuickFlip.Filters;


namespace QuickFlip.Controllers
{
    public class BuyController : Controller
    {
        // GET: /Buy/id
        [InitializeSimpleMembership]
        public ActionResult Index(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            List<Post> buyPosts = BusinessLogic.GetPostsByPostTypeAndCommunity(PostType.Buy, id);
            buyPosts.RemoveAll(x => x.Settled == true);

            if (Request.Form["Filtered"] == "1")
            {
                // category filter
                List<string> categories = new List<string>();
                if (Request.Form["Categories"] == null) { buyPosts.RemoveAll(x => true); }
                else
                {
                    categories = Request.Form["Categories"].Split(',').Select(sValue => sValue.Trim()).ToList();
                    categories.RemoveAll(x => x == "Any");

                    List<Post> categoryFiltered = new List<Post>();
                    foreach (var category in categories)
                    {
                        foreach (var post in buyPosts)
                        {
                            if (post.Categories.Contains((Category)Enum.Parse(typeof(Category), category)))
                            {
                                categoryFiltered.Add(post);
                            }
                        }
                    }

                    buyPosts = categoryFiltered.GroupBy(x => x.PostId).Select(group => group.First()).ToList();
                }

                // max price filter
                if (Request.Form["MaxPriceFilter"] != String.Empty)
                {
                    buyPosts.RemoveAll(x => x.RequiredPrice < UInt32.Parse(Request.Form["MaxPriceFilter"]));
                }

                // will ship filter
                if (Request.Form["WillShip"] == "No")
                {
                    buyPosts.RemoveAll(x => x.TransactionType == TransactionType.Local);
                }

                // has photo filter
                if (Request.Form["HasPhoto"] == "Yes")
                {
                    buyPosts.RemoveAll(x => x.PostMedia == null);
                }

                // any offers filter
                if (Request.Form["AnyOffers"] == "No")
                {
                    buyPosts.RemoveAll(x => x.Offers.Count == 0);
                }

                // order by filter
                switch (Request.Form["OrderBy"])
                {
                    case "MostRecent":
                        buyPosts = buyPosts.OrderByDescending(x => x.CreateDate).ToList();
                        break;
                    case "LeastRecent":
                        buyPosts = buyPosts.OrderBy(x => x.CreateDate).ToList();
                        break;
                    case "AscendingPrice":
                        buyPosts = buyPosts.OrderBy(x => x.RequiredPrice).ToList();
                        break;
                    case "DescendingPrice":
                        buyPosts = buyPosts.OrderByDescending(x => x.RequiredPrice).ToList();
                        break;
                }

                // restore filter selections
                ViewData["OrderBy"] = Request.Form["OrderBy"];
                ViewData["Type"] = Request.Form["Type"];

                var categoryEnums = Enum.GetValues(typeof(Category)).Cast<Category>();
                foreach (var categoryEnum in categoryEnums)
                {
                    ViewData[categoryEnum.ToString()] = "0";
                }
                foreach (var category in categories)
                {
                    ViewData[category.ToString()] = "1";
                }
                ViewData["Any"] = (categories.Count == Enum.GetValues(typeof(Category)).Length) ? "1" : "0";
                
                ViewData["MaxPriceFilter"] = Request.Form["MaxPriceFilter"];
                ViewData["WillShip"] = Request.Form["WillShip"];
                ViewData["HasPhoto"] = Request.Form["HasPhoto"];
                ViewData["AnyOffers"] = Request.Form["AnyOffers"];
            }
            else
            {
                buyPosts = buyPosts.OrderByDescending(x => x.CreateDate).ToList();

                // default filter selections
                ViewData["OrderBy"] = "MostRecent";
                ViewData["Type"] = "DontCare";

                var categoryEnums = Enum.GetValues(typeof(Category)).Cast<Category>();
                foreach (var categoryEnum in categoryEnums)
                {
                    ViewData[categoryEnum.ToString()] = "1";
                }
                ViewData["Any"] = "1";

                ViewData["WillShip"] = "DontCare";
                ViewData["HasPhoto"] = "DontCare";
                ViewData["AnyOffers"] = "DontCare";
            
            }

            return View(buyPosts);
        }

        // GET: /Buy/Post/id
        [InitializeSimpleMembership]
        public ActionResult Post(int id)
        {
            Post post = BusinessLogic.GetPostByPostId(id);

            Community comm = BusinessLogic.GetCommunityByCommunityId(post.CommunityId);
            ViewBag.Community = comm;

            return View(post);
        }

        // GET: /Buy/MakeBuyPost
        public ActionResult MakeBuyPost(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            return View();
        }

        // POST: /Buy/SubmitPost
        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult SubmitPost(HttpPostedFileBase postImage)
        {
            AuctionType auctionType = (AuctionType)Enum.Parse(
                typeof(AuctionType), Request.Form["Type"].ToString());

            TransactionType transactionType = Request.Form["LocalOnly"].ToString() == "Yes"
                ? TransactionType.Local : TransactionType.LocalOrLongDistance;

            string title = Request.Form["PostTitle"].ToString();

            int communityId = Int32.Parse(Request.Form["CommunityId"]);

            int? maxPrice = (String.IsNullOrWhiteSpace(Request.Form["MaxPrice"]) 
                ? (int?)null : Convert.ToInt32(Request.Form["MaxPrice"]));

            string description = Request.Form["PostDescription"].ToString();

            List<Category> categories = Request.Form["Categories"].ToString().Split(',').Select(
                x => (Category)Enum.Parse(typeof(Category), x)).ToList();

            List<string> tags = new List<string>();
            if (Request.Form["Tags"] != String.Empty)
            {
                tags = Request.Form["Tags"].ToString().Split(',').Select(sValue => sValue.Trim().ToLower()).ToList();
            }

            Post newPost = new Post()
            {
                CommunityId = communityId,
                UserId = WebSecurity.CurrentUserId,
                CreateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(3),
                Title = title,
                Description = description,
                Tags = tags,
                RequiredPrice = maxPrice,
                PostType = PostType.Buy,
                AuctionType = auctionType,
                TransactionType = transactionType,
                Categories = categories
            };

            newPost = BusinessLogic.CreatePost(newPost);

            if (postImage != null)
            {
                byte[] imageBytes = new byte[postImage.InputStream.Length];
                long bytesRead = postImage.InputStream.Read(imageBytes, 0, (int)postImage.InputStream.Length);
                postImage.InputStream.Close();
                string b64EncodedImage = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);

                PostMedia newPostMedia = new PostMedia()
                {
                    PostId = newPost.PostId,
                    B64EncodedImage = b64EncodedImage
                };
        
                newPostMedia = BusinessLogic.CreatePostMedia(newPostMedia);
            }

            return RedirectToAction("Index", new { id = (CommunityAbbrev)newPost.CommunityId });
        }

        // POST: /Buy/MakeOffer
        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult MakeOffer()
        {
            int amount = Convert.ToInt32(Request.Form["OfferAmount"]);

            int postId = Convert.ToInt32(Request.Form["PostId"]);

            Post post = BusinessLogic.GetPostByPostId(postId);

            // delete old offers from the same user
            if (post.Offers.Count != 0)
            {
                foreach (var offer in post.Offers)
                {
                    if (offer.UserId == WebSecurity.CurrentUserId)
                    {
                        BusinessLogic.DeleteOffer(offer.OfferId);
                    }
                }
            }
            
            // create new offer
            Offer newOffer = new Offer()
            {
                PostId = postId,
                UserId = WebSecurity.CurrentUserId,
                Amount = amount,
                //Description = description,
                CreateDate = DateTime.Now,
                Accepted = false
            };


            //if (offerImage != null)
            //{
            //    byte[] imageBytes = new byte[offerImage.InputStream.Length];
            //    long bytesRead = offerImage.InputStream.Read(imageBytes, 0, (int)offerImage.InputStream.Length);
            //    offerImage.InputStream.Close();
            //    string b64EncodedImage = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);

            //    OfferMedia newOfferMedia = new OfferMedia()
            //    {
            //        OfferId = newOffer.OfferId,
            //        B64EncodedImage = b64EncodedImage
            //    };

            //    newOfferMedia = BusinessLogic.CreateOfferMedia(newOfferMedia);
            //}

            // delete old new offer and outbid alerts for this post
            List<Alert> oldAlerts = BusinessLogic.GetAlertsByPostId(postId);

            // delete old new offer and outbid for this post
            foreach (var alert in oldAlerts)
            {
                if (alert.Type == AlertType.NewOffer || 
                    (alert.Type == AlertType.Outbid && alert.UserId == newOffer.UserId))
                {
                    BusinessLogic.DeleteAlert(alert.AlertId); 
                }
            }

            // send previous highest bidder an alert saying they were outbid
            if (post.AuctionType == AuctionType.Auction)
            {
                Offer oldBestOffer = post.BestOffer;
                
                if (oldBestOffer != null && oldBestOffer.UserId != newOffer.UserId)
                {
                    BusinessLogic.CreateAlert(postId, oldBestOffer.UserId, oldBestOffer.OfferId, AlertType.Outbid);
                }
            }

            // make new offer
            newOffer = BusinessLogic.CreateOffer(newOffer);

            // send post owner an alert saying a new offer arrived
            BusinessLogic.CreateAlert(postId, post.UserId, newOffer.OfferId, AlertType.NewOffer);

            return RedirectToAction("Index", new { id = (CommunityAbbrev)post.CommunityId });
        }

        // POST: /Buy/AcceptOffer
        [HttpPost]
        public ActionResult AcceptOffer()
        {
            int postId = Convert.ToInt32(Request.Form["PostId"]);

            Post postToSettle = BusinessLogic.GetPostByPostId(postId);

            BusinessLogic.SettlePost(postId);

            BusinessLogic.AcceptOffer(postToSettle.BestOffer.OfferId);

            return RedirectToAction("AcceptOffer", new { id = postToSettle.PostId });
        }

        public ActionResult AcceptOffer(int id)
        {
            Post post = BusinessLogic.GetPostByPostId(id);

            return View(post);
        }

        [HttpPost]
        public ActionResult SendAcceptMessage()
        {
            string subject = "Your offer was accepted!";

            string body = Request.Form["AcceptMessage"].ToString();

            string recipient = BusinessLogic.GetUserByUserId(
                BusinessLogic.GetPostByPostId(Convert.ToInt32(Request.Form["PostId"])).UserId).Email;

            BusinessLogic.SendEmail(recipient, subject, body);

            return RedirectToAction("MessageSent");
        }

        public ActionResult MessageSent()
        {
            return View();
        }
    }
}
