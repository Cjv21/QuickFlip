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
    public class SearchController : Controller
    {
        // GET: /Search/Id
        public ActionResult Index(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            if (Request.Form["Searched"] == "1")
            {
                List<Post> posts = BusinessLogic.GetAllPosts();
                posts.RemoveAll(x => x.Settled == true);

                // category filter
                if (Request.Form["Categories"] == null) { posts.RemoveAll(x => true); }
                else
                {
                    List<string> categories = new List<string>();
                    categories = Request.Form["Categories"].Split(',').Select(sValue => sValue.Trim()).ToList();
                    categories.RemoveAll(x => x == "Any");

                    List<Post> categoryFiltered = new List<Post>();
                    foreach (var category in categories)
                    {
                        foreach (var post in posts)
                        {
                            if (post.Categories.Contains((Category)Enum.Parse(typeof(Category), category)))
                            {
                                categoryFiltered.Add(post);
                            }
                        }
                    }

                    posts = categoryFiltered.GroupBy(x => x.PostId).Select(group => group.First()).ToList();
                }


                // community filter
                if (Request.Form["Communities"] == null) { posts.RemoveAll(x => true); }
                else
                {
                    List<string> communitiesStr = new List<string>();
                    communitiesStr = Request.Form["Communities"].Split(',').Select(sValue => sValue.Trim()).ToList();
                    communitiesStr.RemoveAll(x => x == "Any");

                    // convert to enum
                    List<CommunityAbbrev> communities = new List<CommunityAbbrev>();
                    foreach (var communityStr in communitiesStr)
                    {
                        communities.Add((CommunityAbbrev)Enum.Parse(typeof(CommunityAbbrev), communityStr));
                    }

                    List<Post> communityFiltered = new List<Post>();
                    foreach (var community in communities)
                    {
                        foreach (var post in posts)
                        {
                            if (post.CommunityId == (int)community)
                            {
                                communityFiltered.Add(post);
                            }
                        }
                    }

                    posts = communityFiltered;
                }

                // post type filter
                if (Request.Form["PostType"] == "WantToBuy")
                {
                    posts.RemoveAll(x => x.PostType == PostType.Sell);
                }
                else if (Request.Form["PostType"] == "ForSale")
                {
                    posts.RemoveAll(x => x.PostType == PostType.Buy);
                }

                // auction type filter
                if (Request.Form["AuctionType"] == "Auction")
                {
                    posts.RemoveAll(x => x.AuctionType == AuctionType.FavoriteOffer);
                }
                else if (Request.Form["AuctionType"] == "FavoriteOffer")
                {
                    posts.RemoveAll(x => x.AuctionType == AuctionType.Auction);
                }

                // has photo filter
                if (Request.Form["HasPhoto"] == "Yes")
                {
                    posts.RemoveAll(x => x.PostMedia == null);
                }

                // will ship filter
                if (Request.Form["WillShip"] == "Yes")
                {
                    posts.RemoveAll(x => x.TransactionType == TransactionType.Local);
                }
                else if (Request.Form["WillShip"] == "No")
                {
                    posts.RemoveAll(x => x.TransactionType == TransactionType.LocalOrLongDistance);
                }

                // any offers filter
                if (Request.Form["AnyOffers"] == "Yes")
                {
                    posts.RemoveAll(x => x.Offers.Count == 0);
                }
                else if (Request.Form["AnyOffers"] == "No")
                {
                    posts.RemoveAll(x => x.Offers.Count > 0);
                }

                // price filter
                if (Request.Form["PostType"] == "WantToBuy")
                {
                    var willingToPay = UInt32.Parse(Request.Form["MaxPriceFilter"]);
                    posts.RemoveAll(x => x.RequiredPrice < willingToPay);

                }
                else if (Request.Form["PostType"] == "ForSale")
                {
                    var maxPrice = UInt32.Parse(Request.Form["MaxPriceFilter"]);
                    posts.RemoveAll(x => x.RequiredPrice > maxPrice);
                }
                

                // keywords filter
                if (Request.Form["Keywords"] != String.Empty)
                {
                    List<string> keywords = Request.Form["Keywords"].Split(' ').Select(sValue => sValue.Trim()).ToList();
                    List<Post> keywordFiltered = new List<Post>();
                    foreach (var post in posts)
                    {
                        foreach (var word in keywords)
                        {
                            if (post.Title.ToLower().Contains(word.ToLower()) || post.Description.ToLower().Contains(word.ToLower()))
                            {
                                keywordFiltered.Add(post);
                            }
                        }
                    }

                    posts = keywordFiltered;
                }

                // order by filter
                switch (Request.Form["OrderBy"])
                {
                    case "MostRecent":
                        posts = posts.OrderByDescending(x => x.CreateDate).ToList();
                        break;
                    case "LeastRecent":
                        posts = posts.OrderBy(x => x.CreateDate).ToList();
                        break;
                    case "AscendingPrice":
                        posts = posts.OrderBy(x => x.RequiredPrice).ToList();
                        break;
                    case "DescendingPrice":
                        posts = posts.OrderByDescending(x => x.RequiredPrice).ToList();
                        break;
                }

                ViewData["ResultsReturned"] = "1";

                return View(posts);
            }

            return View();
        }

    }
}
