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

        public static void SendSMS()
        {
           /* 
            * AT&T – cellnumber@txt.att.net
            * Verizon – cellnumber@vtext.com
            * T-Mobile – cellnumber@tmomail.net
            * Sprint PCS - cellnumber@messaging.sprintpcs.com
            * Virgin Mobile – cellnumber@vmobl.com
            * US Cellular – cellnumber@email.uscc.net
            * Nextel - cellnumber@messaging.nextel.com
            * Boost - cellnumber@myboostmobile.com
            * Alltel – cellnumber@message.alltel.com
            */
        }

        #endregion
    }
}