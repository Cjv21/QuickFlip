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
    public class SellController : Controller
    {
        // GET: /Sell/id
        [InitializeSimpleMembership]
        public ActionResult Index(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            List<Post> sellPosts = BusinessLogic.GetPostsByPostTypeAndCommunity(PostType.Sell, id);
            sellPosts.RemoveAll(x => x.Settled == true);

            if (Request.Form["Filtered"] == "1")
            {
                // category filter
                List<string> categories = new List<string>();
                if (Request.Form["Categories"] == null) { sellPosts.RemoveAll(x => true); }
                else
                {
                    categories = Request.Form["Categories"].Split(',').Select(sValue => sValue.Trim()).ToList();
                    categories.RemoveAll(x => x == "Any");

                    List<Post> categoryFiltered = new List<Post>();
                    foreach (var category in categories)
                    {
                        foreach (var post in sellPosts)
                        {
                            if (post.Categories.Contains((Category)Enum.Parse(typeof(Category), category)))
                            {
                                categoryFiltered.Add(post);
                            }
                        }
                    }

                    sellPosts = categoryFiltered.GroupBy(x => x.PostId).Select(group => group.First()).ToList();
                }

                // max price filter
                if (Request.Form["MaxPriceFilter"] != String.Empty)
                {
                    sellPosts.RemoveAll(x => x.RequiredPrice > UInt32.Parse(Request.Form["MaxPriceFilter"]));
                }

                // will ship filter
                if (Request.Form["WillShip"] == "No")
                {
                    sellPosts.RemoveAll(x => x.TransactionType == TransactionType.Local);
                }

                // has photo filter
                if (Request.Form["HasPhoto"] == "Yes")
                {
                    sellPosts.RemoveAll(x => x.PostMedia == null);
                }

                // any offers filter
                if (Request.Form["AnyOffers"] == "No")
                {
                    sellPosts.RemoveAll(x => x.Offers.Count == 0);
                }

                // order by filter
                switch (Request.Form["OrderBy"])
                {
                    case "MostRecent":
                        sellPosts = sellPosts.OrderByDescending(x => x.CreateDate).ToList();
                        break;
                    case "LeastRecent":
                        sellPosts = sellPosts.OrderBy(x => x.CreateDate).ToList();
                        break;
                    case "AscendingPrice":
                        sellPosts = sellPosts.OrderBy(x => x.RequiredPrice).ToList();
                        break;
                    case "DescendingPrice":
                        sellPosts = sellPosts.OrderByDescending(x => x.RequiredPrice).ToList();
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
                sellPosts = sellPosts.OrderByDescending(x => x.CreateDate).ToList();

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

            return View(sellPosts);
        }

        // GET: /Sell/Post/id
        [InitializeSimpleMembership]
        public ActionResult Post(int id)
        {
            Post post = BusinessLogic.GetPostByPostId(id);

            Community comm = BusinessLogic.GetCommunityByCommunityId(post.CommunityId);
            ViewBag.Community = comm;

            return View(post);
        }

        /*
        public string[] GetKeywords(string text)
        {
            // BETTER STOP WORD LIST
            string[] stop = { "a", "able", "about", "above", "abroad", "according", "accordingly", "across", "actually", "adj", "after", "afterwards", "again", "against", "ago", "ahead", "ain't", "all", "allow", "allows", "almost", "alone", "along", "alongside", "already", "also", "although", "always", "am", "amid", "amidst", "among", "amongst", "an", "and", "another", "any", "anybody", "anyhow", "anyone", "anything", "anyway", "anyways", "anywhere", "apart", "appear", "appreciate", "appropriate", "are", "aren't", "around", "as", "a's", "aside", "ask", "asking", "associated", "at", "available", "away", "awfully", "b", "back", "backward", "backwards", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "begin", "behind", "being", "believe", "below", "beside", "besides", "best", "better", "between", "beyond", "both", "brief", "but", "by", "c", "came", "can", "cannot", "cant", "can't", "caption", "cause", "causes", "certain", "certainly", "changes", "clearly", "c'mon", "co", "co.", "com", "come", "comes", "concerning", "consequently", "consider", "considering", "contain", "containing", "contains", "corresponding", "could", "couldn't", "course", "c's", "currently", "d", "dare", "daren't", "definitely", "described", "despite", "did", "didn't", "different", "directly", "do", "does", "doesn't", "doing", "done", "don't", "down", "downwards", "during", "e", "each", "edu", "eg", "eight", "eighty", "either", "else", "elsewhere", "end", "ending", "enough", "entirely", "especially", "et", "etc", "even", "ever", "evermore", "every", "everybody", "everyone", "everything", "everywhere", "ex", "exactly", "example", "except", "f", "fairly", "far", "farther", "few", "fewer", "fifth", "first", "five", "followed", "following", "follows", "for", "forever", "former", "formerly", "forth", "forward", "found", "four", "from", "further", "furthermore", "g", "get", "gets", "getting", "given", "gives", "go", "goes", "going", "gone", "got", "gotten", "greetings", "h", "had", "hadn't", "half", "happens", "hardly", "has", "hasn't", "have", "haven't", "having", "he", "he'd", "he'll", "hello", "help", "hence", "her", "here", "hereafter", "hereby", "herein", "here's", "hereupon", "hers", "herself", "he's", "hi", "him", "himself", "his", "hither", "hopefully", "how", "howbeit", "however", "hundred", "i", "i'd", "ie", "if", "ignored", "i'll", "i'm", "immediate", "in", "inasmuch", "inc", "inc.", "indeed", "indicate", "indicated", "indicates", "inner", "inside", "insofar", "instead", "into", "inward", "is", "isn't", "it", "it'd", "it'll", "its", "it's", "itself", "i've", "j", "just", "k", "keep", "keeps", "kept", "know", "known", "knows", "l", "last", "lately", "later", "latter", "latterly", "least", "less", "lest", "let", "let's", "like", "liked", "likely", "likewise", "little", "look", "looking", "looks", "low", "lower", "ltd", "m", "made", "mainly", "make", "makes", "many", "may", "maybe", "mayn't", "me", "mean", "meantime", "meanwhile", "merely", "might", "mightn't", "mine", "minus", "miss", "more", "moreover", "most", "mostly", "mr", "mrs", "much", "must", "mustn't", "my", "myself", "n", "name", "namely", "nd", "near", "nearly", "necessary", "need", "needn't", "needs", "neither", "never", "neverf", "neverless", "nevertheless", "new", "next", "nine", "ninety", "no", "nobody", "non", "none", "nonetheless", "noone", "no-one", "nor", "normally", "not", "nothing", "notwithstanding", "novel", "now", "nowhere", "o", "obviously", "of", "off", "often", "oh", "ok", "okay", "old", "on", "once", "one", "ones", "one's", "only", "onto", "opposite", "or", "other", "others", "otherwise", "ought", "oughtn't", "our", "ours", "ourselves", "out", "outside", "over", "overall", "own", "p", "particular", "particularly", "past", "per", "perhaps", "placed", "please", "plus", "possible", "presumably", "probably", "provided", "provides", "q", "que", "quite", "qv", "r", "rather", "rd", "re", "really", "reasonably", "recent", "recently", "regarding", "regardless", "regards", "relatively", "respectively", "right", "round", "s", "said", "same", "saw", "say", "saying", "says", "second", "secondly", "see", "seeing", "seem", "seemed", "seeming", "seems", "seen", "self", "selves", "sensible", "sent", "serious", "seriously", "seven", "several", "shall", "shan't", "she", "she'd", "she'll", "she's", "should", "shouldn't", "since", "six", "so", "some", "somebody", "someday", "somehow", "someone", "something", "sometime", "sometimes", "somewhat", "somewhere", "soon", "sorry", "specified", "specify", "specifying", "still", "sub", "such", "sup", "sure", "t", "take", "taken", "taking", "tell", "tends", "th", "than", "thank", "thanks", "thanx", "that", "that'll", "thats", "that's", "that've", "the", "their", "theirs", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "there'd", "therefore", "therein", "there'll", "there're", "theres", "there's", "thereupon", "there've", "these", "they", "they'd", "they'll", "they're", "they've", "thing", "things", "think", "third", "thirty", "this", "thorough", "thoroughly", "those", "though", "three", "through", "throughout", "thru", "thus", "till", "to", "together", "too", "took", "toward", "towards", "tried", "tries", "truly", "try", "trying", "t's", "twice", "two", "u", "un", "under", "underneath", "undoing", "unfortunately", "unless", "unlike", "unlikely", "until", "unto", "up", "upon", "upwards", "us", "use", "used", "useful", "uses", "using", "usually", "v", "value", "various", "versus", "very", "via", "viz", "vs", "w", "want", "wants", "was", "wasn't", "way", "we", "we'd", "welcome", "well", "we'll", "went", "were", "we're", "weren't", "we've", "what", "whatever", "what'll", "what's", "what've", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "where's", "whereupon", "wherever", "whether", "which", "whichever", "while", "whilst", "whither", "who", "who'd", "whoever", "whole", "who'll", "whom", "whomever", "who's", "whose", "why", "will", "willing", "wish", "with", "within", "without", "wonder", "won't", "would", "wouldn't", "x", "y", "yes", "yet", "you", "you'd", "you'll", "your", "you're", "yours", "yourself", "yourselves", "you've", "z", "zero", "" };

            char[] splitChars = { ' ', '\'' };

            string[] words = text.Split(splitChars);

            var filteredWords = words.Except(stop).ToList();

            var keywordCount = (
                from keyword in filteredWords
                group keyword by keyword into g
                select new { Keyword = g.Key, Count = g.Count() }
             );

            return keywordCount.OrderByDescending(k => k.Count).Select(k => k.Keyword).Take(5).ToArray();
        }
         */

        // GET: /Sell/MakeSellPost
        public ActionResult MakeSellPost(CommunityAbbrev id)
        {
            Community comm = BusinessLogic.GetCommunityByCommunityId((int)id);
            ViewBag.Community = comm;

            return View();
        }

        // POST: /Sell/SubmitPost
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
                PostType = PostType.Sell,
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

        // POST: /Sell/MakeOffer
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

        // POST: /Sell/AcceptOffer
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
