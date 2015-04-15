using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using QuickFlip.DataAccessLayer;
using QuickFlip.Models;

namespace QuickFlip.BusinessLayer
{
    public class BusinessLogic
    {
        #region UserProfile
            
        public static bool IsEmailVerified(string userName)
        {
            DataAccess da = new DataAccess();

            bool isEmailVerified = da.IsEmailVerified(userName);

            da.Dispose();

            return isEmailVerified;
        }

        public static void SetEmailAsVerified(string userName)
        {
            DataAccess da = new DataAccess();

            da.SetEmailAsVerified(userName);

            da.Dispose();
        }

        public static void SendConfirmationCode(RegisterModel newUser, string nonce)
        {
            string body = "Welcome to QuickFlip! \n \n" + "Your confirmation code: " + nonce;
            SendEmail(newUser.Email, "Confirmation Code", body);
        }

        public static bool CheckNonce(string nonce, string userName)
        {
            DataAccess da = new DataAccess();

            bool isNonceCorrect = da.CheckNonce(nonce, userName);

            da.Dispose();

            return isNonceCorrect;
        }

        public static bool DoesUserExist(string userName)
        {
            DataAccess da = new DataAccess();

            bool DoesUserExist = da.DoesUserExist(userName);

            da.Dispose();

            return DoesUserExist;
        }

        public static void PopulateUserProfile(RegisterModel newUser, string nonce)
        {
            DataAccess da = new DataAccess();

            da.PopulateUserProfile(newUser, nonce);

            da.Dispose();
        }

        public static User GetUserByUserId(int userId)
        {
            DataAccess da = new DataAccess();

            User user = da.GetUserByUserId(userId);

            da.Dispose();

            return user;
        }

        public static string GetUserNameFromUserId(int userId)
        {
            DataAccess da = new DataAccess();

            User user = da.GetUserByUserId(userId);

            da.Dispose();

            return user.UserName;
        }

        public static void ChangeProfilePicture(int userId, string b64EncodedImage)
        {
            DataAccess da = new DataAccess();

            da.ChangeProfilePicture(userId, b64EncodedImage);

            da.Dispose();
        }

        public static void ChangeHomeCommunity(int userId, CommunityAbbrev comm)
        {
            DataAccess da = new DataAccess();

            da.ChangeHomeCommunity(userId, comm);

            da.Dispose();
        }

        public static void ChangePhone(int userId, Int64 phoneNumber, Carrier carrier)
        {
            DataAccess da = new DataAccess();

            da.ChangePhone(userId, phoneNumber, carrier);

            da.Dispose();
        }

        public static void ChangeAlertMode(int userId, AlertMode mode)
        {
            DataAccess da = new DataAccess();

            da.ChangeAlertMode(userId, mode);

            da.Dispose();
        }

        #endregion

        #region Community

        public static Community GetCommunityByCommunityId(int id)
        {
            DataAccess da = new DataAccess();

            Community comm = da.GetCommmunityByCommunityId(id);

            da.Dispose();

            return comm;
        }

        #endregion

        #region Post

        public static Post CreatePost(Post newPost)
        {
            DataAccess da = new DataAccess();

            newPost = da.CreatePost(newPost);

            da.Dispose();

            return newPost;
        }

        public static Post EditPost(Post editedPost)
        {
            DataAccess da = new DataAccess();

            editedPost = da.EditPost(editedPost);

            da.Dispose();

            return editedPost;
        }

        public static Post GetPostByPostId(int postId)
        {
            DataAccess da = new DataAccess();

            Post post = da.GetPostByPostId(postId);

            da.Dispose();

            return post;
        }

        public static List<Post> GetPostsByUserId(int userId)
        {
            DataAccess da = new DataAccess();

            List<Post> posts = da.GetPostsByUserId(userId);

            da.Dispose();

            return posts;
        }

        public static List<Post> GetPostsByPostTypeAndCommunity(PostType type, CommunityAbbrev community)
        {
            DataAccess da = new DataAccess();

            List<Post> buyPosts = da.GetPostsByPostTypeAndCommunity(type, community);

            da.Dispose();

            return buyPosts;
        }

        public static List<Post> GetAllPosts()
        {
            DataAccess da = new DataAccess();

            List<Post> posts = da.GetAllPosts();

            da.Dispose();

            return posts;
        }

        public static void SettlePost(int postId)
        {
            DataAccess da = new DataAccess();

            da.SettlePost(postId);

            da.Dispose();
        }

        public static void DeletePost(int postId)
        {
            DataAccess da = new DataAccess();

            da.DeletePost(postId);

            da.Dispose();
        }

        public static List<Post> GetPostsByBidderId(int userId)
        {
            DataAccess da = new DataAccess();

            List<Post> posts = da.GetPostsByBidderId(userId);

            da.Dispose();

            return posts;
        }

        #endregion

        #region Category

        public static void AddPostToCategory(Post post, Category category)
        {
            DataAccess da = new DataAccess();

            da.AddPostToCategory(post, category);

            da.Dispose();
        }

        public static string GetFullCategoryName(Category category)
        {
            switch (category)
            {
                case Category.CameraPhoto:
                    return "Camera & Photo";
                case Category.CellPhones:
                    return "Cell Phones";
                case Category.ClothingShoe:
                    return "Clothing & Shoe";
                case Category.HealthBeauty:
                    return "Health & Beauty";
                case Category.MusicalInstruments:
                    return "Musical Instruments";
                case Category.RealEstate:
                    return "Real Estate";
                case Category.SportingGoods:
                    return "Sporting Goods";
                default:
                    return category.ToString();
            }
        }

        #endregion

        #region PostMedia

        public static PostMedia CreatePostMedia(PostMedia newPostMedia)
        {
            DataAccess da = new DataAccess();

            newPostMedia = da.CreatePostMedia(newPostMedia);

            da.Dispose();

            return newPostMedia;
        }

        public static void DeletePostMedia(int postId)
        {
            DataAccess da = new DataAccess();

            da.DeletePostMedia(postId);

            da.Dispose();
        }

        #endregion

        #region Offer

        public static Offer CreateOffer(Offer newOffer)
        {
            DataAccess da = new DataAccess();

            newOffer = da.CreateOffer(newOffer);

            da.Dispose();

            return newOffer;
        }

        public static List<Offer> GetOffersByPostId(int postId)
        {
            DataAccess da = new DataAccess();

            List<Offer> offers = da.GetOffersByPostId(postId);

            da.Dispose();

            return offers;
        }

        public static Offer GetAcceptedOfferIdFromPostId(int postId)
        {
            DataAccess da = new DataAccess();

            Post post = GetPostByPostId(postId);

            Offer acceptedOffer = new Offer();

            if  (post.Settled)
            {
                if (post.AuctionType == AuctionType.Auction)
                {
                    return post.BestOffer;
                }
                else // favorite offer
                {
                    foreach (var offer in post.Offers)
                    {
                        if (offer.Accepted) { return offer;  }
                    }
                }
            }

            return acceptedOffer;
        }

        public static Offer GetOfferByOfferId(int offerId)
        {
            DataAccess da = new DataAccess();

            Offer offers = da.GetOfferByOfferId(offerId);

            da.Dispose();

            return offers;
        }

        public static void AcceptOffer(int offerId)
        {
            DataAccess da = new DataAccess();

            da.AcceptOffer(offerId);

            da.Dispose();
        }

        public static void DeleteOffer(int offerId)
        {
            DataAccess da = new DataAccess();

            da.DeleteOffer(offerId);

            da.Dispose();
        }

        #endregion

        #region OfferMedia

        public static OfferMedia CreateOfferMedia(OfferMedia newOfferMedia)
        {
            DataAccess da = new DataAccess();

            newOfferMedia = da.CreateOfferMedia(newOfferMedia);

            da.Dispose();

            return newOfferMedia;
        }

        public static void DeleteOfferMedia(int offerMedia)
        {
            DataAccess da = new DataAccess();

            da.DeleteOfferMedia(offerMedia);

            da.Dispose();
        }

        #endregion

        #region Recommendations
        public static List<Post> GetRecommendationsByUserId(int userId)
        {
            List<Post> recommendations = new List<Post>();
            List<Post> posts = GetAllPosts().Where(x => x.Settled == false).ToList();

            // posts i have created
            List<Post> myPosts = posts.Where(x => x.UserId == userId).ToList();
            List<Post> myForSale = myPosts.Where(x => x.PostType == PostType.Sell).ToList();
            List<Post> myWantToBuy = myPosts.Where(x => x.PostType == PostType.Buy).ToList();

            // posts i have bid on
            List<Post> bidOn = GetPostsByBidderId(userId);
            List<Post> bidOnInSell = bidOn.Where(x => x.PostType == PostType.Sell).ToList();
            List<Post> bidOnInBuy = bidOn.Where(x => x.PostType == PostType.Buy).ToList();

            // get posts that indicate the user may want to buy my items
            List<Post> mayWantToBuyMyStuff = new List<Post>();
            if (myForSale.Count != 0 || bidOnInBuy.Count != 0)
            {
                mayWantToBuyMyStuff = GetMayWantToBuyMyStuff(userId, myForSale, bidOnInBuy, posts);
                recommendations.AddRange(mayWantToBuyMyStuff);
            }

            // get posts that I may want to buy
            List<Post> mayWantToBuyTheirStuff = new List<Post>();
            if (myWantToBuy.Count != 0 || bidOnInSell.Count != 0)
            {
                mayWantToBuyTheirStuff = GetMayWantToBuyTheirStuff(userId, myWantToBuy, bidOnInSell, posts);
                recommendations.AddRange(mayWantToBuyTheirStuff);
            } 

            return recommendations;
        }

        public static List<Post> GetMayWantToBuyTheirStuff(
            int myUserId,
            List<Post> myWantToBuy, 
            List<Post> bidOnInSell, 
            List<Post> posts)
        {
            int? myCommunityId = BusinessLogic.GetUserByUserId(myUserId).CommunityId;

            List<Post> sellPosts = posts.Where(x => 
                x.PostType == PostType.Sell &&
                !bidOnInSell.Any(e => e.PostId == x.PostId)
            ).ToList();

            // home community not set, don't restrict recommendations to communities
            if (myCommunityId == null)
            {
                sellPosts.RemoveAll(x => x.UserId == myUserId);
            }
            else
            {
                // remove posts that aren't in my community and are not willing to ship
                sellPosts.RemoveAll(x =>
                    x.UserId == myUserId ||
                    (x.CommunityId != myCommunityId &&
                     x.TransactionType == TransactionType.Local)
                );
            }

            List<Post> wantToBuyTheirStuff = new List<Post>();

            // get posts in want to buy that match my for sale
            foreach (var post in myWantToBuy)
            {
                wantToBuyTheirStuff.AddRange(GetSimilarPosts(post, PostType.Sell, sellPosts));
            }

            // get posts I bid on in want to buy (indicating I am selling)
            foreach (var post in bidOnInSell)
            {
                wantToBuyTheirStuff.AddRange(GetSimilarPosts(post, PostType.Sell, sellPosts));
            }

            // remove duplicates and return
            return wantToBuyTheirStuff.GroupBy(x => x.PostId).Select(g => g.First()).ToList();
        }

        public static List<Post> GetMayWantToBuyMyStuff(
            int myUserId,
            List<Post> myForSale, 
            List<Post> bidOnInBuy, 
            List<Post> posts)
        {
            int? myCommunityId = BusinessLogic.GetUserByUserId(myUserId).CommunityId;

            List<Post> buyPosts = posts.Where(x =>
                x.PostType == PostType.Buy &&
                !bidOnInBuy.Any(e => e.PostId == x.PostId)
            ).ToList();

            // home community not set, don't restrict recommendations to communities
            if (myCommunityId == null)
            {
                buyPosts.RemoveAll(x => x.UserId == myUserId);
            }
            else
            {
                // remove posts that aren't in my community and are not willing to ship
                buyPosts.RemoveAll(x =>
                    x.UserId == myUserId ||
                    (x.CommunityId != myCommunityId &&
                     x.TransactionType == TransactionType.Local)
                );
            }

            List<Post> wantToBuyMyStuff = new List<Post>();

            // get posts in want to buy that match my for sale
            foreach (var post in myForSale)
            {
                wantToBuyMyStuff.AddRange(GetSimilarPosts(post, PostType.Buy, buyPosts));
            }

            // get posts I bid on in want to buy (indicating I am selling)
            foreach (var post in bidOnInBuy)
            {
                wantToBuyMyStuff.AddRange(GetSimilarPosts(post, PostType.Buy, buyPosts));
            }

            // remove duplicates and return
            return wantToBuyMyStuff.GroupBy(x => x.PostId).Select(g => g.First()).ToList();
        }
        
        public static List<Post> GetSimilarPosts(Post postToBeSimilarTo, PostType type, List<Post> posts)
        {
            List<Post> similarPosts = new List<Post>();
            List<string> keywords = new List<string>();

            // no tags, generate search words
            if (postToBeSimilarTo.Tags.Count == 0)
            {
                keywords = GetKeywords(postToBeSimilarTo);
            }
            else // use tags as search words
            {
                keywords = postToBeSimilarTo.Tags;
            }

            foreach (var post in posts)
            {
                foreach (var word in keywords)
                {
                    if (post.Title.ToLower().Contains(word.ToLower()) ||
                        post.Description.ToLower().Contains(word.ToLower()) ||
                        post.Tags.Contains(word.ToLower()))
                    {
                        similarPosts.Add(post);
                    }
                }
            }

            // remove duplicates and return
            return similarPosts.GroupBy(x => x.PostId).Select(g => g.First()).ToList();
        }

        #endregion

        #region Alert

        public static bool AnyAlerts(int userId)
        {
            List<Alert> alerts = GetAlertsByUserId(userId);

            return (alerts.Count != 0);
        }

        public static Alert CreateAlert(int postId, int userId, int offerId, AlertType type)
        {
            DataAccess da = new DataAccess();

            Alert alert = da.CreateAlert(postId, userId, offerId, type);

            da.Dispose();

            User user = GetUserByUserId(userId);

            // send e-mail alert
            if (user.AlertMode == AlertMode.Email || user.AlertMode == AlertMode.Both)
            {
                SendAlert(alert, AlertMode.Email);
            }

            // send text alert
            if (user.AlertMode == AlertMode.Text || user.AlertMode == AlertMode.Both)
            {
                SendAlert(alert, AlertMode.Text);
            }

            return alert;
        }

        public static void SendAlert(Alert alert, AlertMode mode)
        {
            User user = GetUserByUserId(alert.UserId);

            if (mode == AlertMode.Email)
            {
                SendEmail(
                    user.Email, 
                    GetAlertSubject(alert.Type),
                    GetAlertBody(alert)
               );
            }
            else if (mode == AlertMode.Text)
            {
                SendSMS(
                    user.Phone,
                    user.Carrier,
                    GetAlertBody(alert)
                );
            }
        }

        public static string GetAlertSubject(AlertType type)
        {
            switch (type)
            {
                case AlertType.Accepted:
                    return "Your offer was accepted!";
                case AlertType.Lost:
                    return "You lost the auction!";
                case AlertType.Outbid:
                    return "You were outbid!";
                case AlertType.NewOffer:
                    return "You have a new offer!";
                default:
                    return type.ToString();
            }
        }

        public static string GetAlertBody(Alert alert)
        {
            string body = String.Empty;
            Post post = GetPostByPostId(alert.PostId);

            switch (alert.Type)
            {
                case AlertType.Accepted:
                    body = "Congratulations " + 
                        GetUserByUserId(alert.UserId).UserName +
                        "! Your offer on post: " +
                        post.Title +
                        " was accepted. Expect a follow up e-mail from " +
                        GetUserByUserId(post.UserId).UserName +
                        " shortly.";
                    break;

                case AlertType.Lost:
                    body = "We're sorry " +
                        GetUserByUserId(alert.UserId).UserName +
                        "! Your offer on post: " +
                        post.Title +
                        " was not accepted. Better luck next time";
                    break;

                case AlertType.Outbid:
                    body = "Hey " +
                        GetUserByUserId(alert.UserId).UserName +
                        "! You were outbid on post: " +
                        post.Title +
                        ". Place another bid if you want the item.";
                    break;

                case AlertType.NewOffer:
                    body = "Hey " +
                        GetUserByUserId(alert.UserId).UserName +
                        "! You have a new offer on post: " +
                        post.Title;
                    break;
            }

            return body;
        }

        public static Alert GetAlertByAlertId(int alertId)
        {
            DataAccess da = new DataAccess();

            Alert alert = da.GetAlertByAlertId(alertId);

            da.Dispose();

            return alert;
        }

        public static List<Alert> GetAlertsByUserId(int userId)
        {
            DataAccess da = new DataAccess();

            List<Alert> alertList = da.GetAlertsByUserId(userId);

            da.Dispose();

            return alertList;
        }

        public static List<Alert> GetAlertsByPostId(int postId)
        {
            DataAccess da = new DataAccess();

            List<Alert> alertList = da.GetAlertsByPostId(postId);

            da.Dispose();

            return alertList;
        }

        public static void DeleteAlert(int alertId)
        {
            DataAccess da = new DataAccess();

            da.DeleteAlert(alertId);

            da.Dispose();
        }

        public static void DeleteAlerts(int postId)
        {
            DataAccess da = new DataAccess();

            da.DeleteAlerts(postId);

            da.Dispose();
        }

        #endregion

        #region Utilities

        public static string Generate8CharNonce()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var nonce = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return nonce;
        }

        public static string PhoneNumberToString(Int64 number)
        {

            string numStr = number.ToString();

            return "(" + numStr.Substring(0, 3) + ") " 
                       + numStr.Substring(3, 3) + " " 
                       + numStr.Substring(6, 4);
        }

        public static string GetFullCarrierName(Carrier? carrier)
        {
            switch (carrier)
            {
                case Carrier.ATT:
                    return "AT&T";
                case Carrier.Verizon:
                    return "Verizon";
                case Carrier.TMobile:
                    return "T-Mobile";
                case Carrier.Sprint:
                    return "Sprint";
                case Carrier.VirginMobile:
                    return "Virgin Mobile";
                case Carrier.USCellular:
                    return "US Cellular";
                case Carrier.Nextel:
                    return "Nextel";
                case Carrier.Boost:
                    return "Boost";
                case Carrier.Alltell:
                    return "Alltell";
                default:
                    return carrier.ToString();
            }
        }

        public static string GetCarrierEmail(Carrier? carrier)
        {
            switch (carrier)
            {
                case Carrier.ATT:
                    return "txt.att.net";
                case Carrier.Verizon:
                    return "vtext.com";
                case Carrier.TMobile:
                    return "tmomail.net";
                case Carrier.Sprint:
                    return "messaging.sprintpcs.com";
                case Carrier.VirginMobile:
                    return "vmobl.com";
                case Carrier.USCellular:
                    return "email.uscc.net";
                case Carrier.Nextel:
                    return "messaging.nextel.com";
                case Carrier.Boost:
                    return "myboostmobile.com";
                case Carrier.Alltell:
                    return "message.alltel.com";
                default:
                    return carrier.ToString();
            }
        }

        public static void SendEmail(string recipient, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                smtpClient.EnableSsl = true;

                smtpClient.Credentials = new System.Net.NetworkCredential("quickflipauctions", "Quickflip411");

                smtpClient.Port = 587;

                mail.From = new MailAddress("quickflipauctions@gmail.com");
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;

                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void SendSMS(Int64? phone, Carrier? carrier, string body)
        {
            string address = phone.ToString() + "@" + GetCarrierEmail(carrier);
            SendEmail(address, "", body);
        }

        public static List<string> GetKeywords(Post post)
        {
            string text = post.Title + " " + post.Description;
            text = text.Replace("\n", " ");
            text = text.Replace("\r", " ");
            text = text.Replace(".", "");
            text = text.Replace(",", "");
            text = text.Replace("?", "");
            text = text.ToLower();

            string[] stop = { "a", "able", "about", "above", "abroad", "according", "accordingly", "across", "actually", "adj", "after", "afterwards", "again", "against", "ago", "ahead", "ain't", "all", "allow", "allows", "almost", "alone", "along", "alongside", "already", "also", "although", "always", "am", "amid", "amidst", "among", "amongst", "an", "and", "another", "any", "anybody", "anyhow", "anyone", "anything", "anyway", "anyways", "anywhere", "apart", "appear", "appreciate", "appropriate", "are", "aren't", "around", "as", "a's", "aside", "ask", "asking", "associated", "at", "available", "away", "awfully", "b", "back", "backward", "backwards", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "begin", "behind", "being", "believe", "below", "beside", "besides", "best", "better", "between", "beyond", "both", "brief", "but", "by", "c", "came", "can", "cannot", "cant", "can't", "caption", "cause", "causes", "certain", "certainly", "changes", "clearly", "c'mon", "co", "co.", "com", "come", "comes", "concerning", "consequently", "consider", "considering", "contain", "containing", "contains", "corresponding", "could", "couldn't", "course", "c's", "currently", "d", "dare", "daren't", "definitely", "described", "despite", "did", "didn't", "different", "directly", "do", "does", "doesn't", "doing", "done", "don't", "down", "downwards", "during", "e", "each", "edu", "eg", "eight", "eighty", "either", "else", "elsewhere", "end", "ending", "enough", "entirely", "especially", "et", "etc", "even", "ever", "evermore", "every", "everybody", "everyone", "everything", "everywhere", "ex", "exactly", "example", "except", "f", "fairly", "far", "farther", "few", "fewer", "fifth", "first", "five", "followed", "following", "follows", "for", "forever", "former", "formerly", "forth", "forward", "found", "four", "from", "further", "furthermore", "g", "get", "gets", "getting", "given", "gives", "go", "goes", "going", "gone", "got", "gotten", "greetings", "h", "had", "hadn't", "half", "happens", "hardly", "has", "hasn't", "have", "haven't", "having", "he", "he'd", "he'll", "hello", "help", "hence", "her", "here", "hereafter", "hereby", "herein", "here's", "hereupon", "hers", "herself", "he's", "hi", "him", "himself", "his", "hither", "hopefully", "how", "howbeit", "however", "hundred", "i", "i'd", "ie", "if", "ignored", "i'll", "i'm", "immediate", "in", "inasmuch", "inc", "inc.", "indeed", "indicate", "indicated", "indicates", "inner", "inside", "insofar", "instead", "into", "inward", "is", "isn't", "it", "it'd", "it'll", "its", "it's", "itself", "i've", "j", "just", "k", "keep", "keeps", "kept", "know", "known", "knows", "l", "last", "lately", "later", "latter", "latterly", "least", "less", "lest", "let", "let's", "like", "liked", "likely", "likewise", "little", "look", "looking", "looks", "low", "lower", "ltd", "m", "made", "mainly", "make", "makes", "many", "may", "maybe", "mayn't", "me", "mean", "meantime", "meanwhile", "merely", "might", "mightn't", "mine", "minus", "miss", "more", "moreover", "most", "mostly", "mr", "mrs", "much", "must", "mustn't", "my", "myself", "n", "name", "namely", "nd", "near", "nearly", "necessary", "need", "needn't", "needs", "neither", "never", "neverf", "neverless", "nevertheless", "new", "next", "nine", "ninety", "no", "nobody", "non", "none", "nonetheless", "noone", "no-one", "nor", "normally", "not", "nothing", "notwithstanding", "novel", "now", "nowhere", "o", "obviously", "of", "off", "often", "oh", "ok", "okay", "old", "on", "once", "one", "ones", "one's", "only", "onto", "opposite", "or", "other", "others", "otherwise", "ought", "oughtn't", "our", "ours", "ourselves", "out", "outside", "over", "overall", "own", "p", "particular", "particularly", "past", "per", "perhaps", "placed", "please", "plus", "possible", "presumably", "probably", "provided", "provides", "q", "que", "quite", "qv", "r", "rather", "rd", "re", "really", "reasonably", "recent", "recently", "regarding", "regardless", "regards", "relatively", "respectively", "right", "round", "s", "said", "same", "saw", "say", "saying", "says", "second", "secondly", "see", "seeing", "seem", "seemed", "seeming", "seems", "seen", "self", "selves", "sensible", "sent", "serious", "seriously", "seven", "several", "shall", "shan't", "she", "she'd", "she'll", "she's", "should", "shouldn't", "since", "six", "so", "some", "somebody", "someday", "somehow", "someone", "something", "sometime", "sometimes", "somewhat", "somewhere", "soon", "sorry", "specified", "specify", "specifying", "still", "sub", "such", "sup", "sure", "t", "take", "taken", "taking", "tell", "tends", "th", "than", "thank", "thanks", "thanx", "that", "that'll", "thats", "that's", "that've", "the", "their", "theirs", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "there'd", "therefore", "therein", "there'll", "there're", "theres", "there's", "thereupon", "there've", "these", "they", "they'd", "they'll", "they're", "they've", "thing", "things", "think", "third", "thirty", "this", "thorough", "thoroughly", "those", "though", "three", "through", "throughout", "thru", "thus", "till", "to", "together", "too", "took", "toward", "towards", "tried", "tries", "truly", "try", "trying", "t's", "twice", "two", "u", "un", "under", "underneath", "undoing", "unfortunately", "unless", "unlike", "unlikely", "until", "unto", "up", "upon", "upwards", "us", "use", "used", "useful", "uses", "using", "usually", "v", "value", "various", "versus", "very", "via", "viz", "vs", "w", "want", "wants", "was", "wasn't", "way", "we", "we'd", "welcome", "well", "we'll", "went", "were", "we're", "weren't", "we've", "what", "whatever", "what'll", "what's", "what've", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "where's", "whereupon", "wherever", "whether", "which", "whichever", "while", "whilst", "whither", "who", "who'd", "whoever", "whole", "who'll", "whom", "whomever", "who's", "whose", "why", "will", "willing", "wish", "with", "within", "without", "wonder", "won't", "would", "wouldn't", "x", "y", "yes", "yet", "you", "you'd", "you'll", "your", "you're", "yours", "yourself", "yourselves", "you've", "z", "zero", "perfect", "condition", "great", "excellent", "black", "blue", "brown", "grey", "gray", "green", "orange", "pink", "purple", "red", "white", "yellow", "sale", "sell", "selling", "buy", "buying", "works", "working", "perfect", "condition", "good", "nice", "dusty", "1", "2", "3", "4", "5", "6", "7", "8", "9", "" };

            char[] splitChars = { ' ', '\'' };

            string[] words = text.Split(splitChars);

            var filteredWords = words.Except(stop).ToList();

            var keywordCount = (
                from keyword in filteredWords
                group keyword by keyword into g
                select new { Keyword = g.Key, Count = g.Count() }
             );

            return keywordCount.OrderByDescending(k => k.Count).ThenByDescending(k => k.ToString().Length)
                .Select(k => k.Keyword).Take(5).ToList();
        }


        #endregion
    }
}