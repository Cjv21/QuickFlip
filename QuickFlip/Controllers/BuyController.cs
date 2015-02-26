﻿using System;
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

            List<Post> buyPosts = BusinessLogic.GetPostsByPostType(PostType.Buy);
            buyPosts.RemoveAll(x => x.CommunityId != comm.CommunityId);

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

        // POST: /Buy/MakeOffer
        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult MakeOffer()
        {
            int amount = Convert.ToInt32(Request.Form["OfferAmount"]);

            int postId = Convert.ToInt32(Request.Form["PostId"]);

            Post post = BusinessLogic.GetPostByPostId(postId);

            // delete old offers from the same user
            if (post.Offers != null)
            {
                foreach (var offer in post.Offers)
                {
                    if (offer.UserId == WebSecurity.CurrentUserId)
                    {
                        // BusinessLogic.DeleteOffer(offer.OfferId);
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

            newOffer = BusinessLogic.CreateOffer(newOffer);

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

            // send previous highest bidder an alert saying they were outbid

            // send pos owner an alert saying a new offer arrived

            return RedirectToAction("Index", new { id = (CommunityAbbrev)post.CommunityId });
        }
    }
}
