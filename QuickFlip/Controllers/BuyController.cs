using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using QuickFlip.BusinessLayer;
using QuickFlip.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using QuickFlip.Filters;
using QuickFlip.Models;


namespace QuickFlip.Controllers
{
    public class BuyController : Controller
    {
        [InitializeSimpleMembership]
        public ActionResult Index(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            List<Post> buyPosts = BusinessLogic.GetPostsByPostType(PostType.Buy);

            return View(buyPosts);
        }

        public ActionResult MakeBuyPost(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            return View();
        }

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

            Post newPost = new Post()
            {
                CommunityId = communityId,
                UserId = WebSecurity.CurrentUserId,
                CreateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(3),
                Title = title,
                Description = description,
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
    }
}
