﻿using QuickFlip.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace QuickFlip.DataAccessLayer
{
    public class DataAccess : IDisposable
    {
        private SqlConnection Connection;

        #region Constructor/Dispose

        public DataAccess()
        {
            Connection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            try 
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } 
        }

        public void Dispose()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        #region UserProfile

        public bool IsEmailVerified(string userName)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT Verified FROM [UserProfile] " +
                    "WHERE UserName = @UserName",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserName", userName);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bool isVerified = Convert.ToBoolean(reader["Verified"]);

                    reader.Close();

                    return isVerified;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public void SetEmailAsVerified(string userName)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET Verified = 1 " +
                    "WHERE UserName = @UserName",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserName", userName);

                // execute
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public bool CheckNonce(string nonce, string userName)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT Nonce FROM [UserProfile] " +
                    "WHERE UserName = @UserName",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserName", userName);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string dbNonce = reader["Nonce"].ToString();

                    reader.Close();

                    return (dbNonce.ToLower() == nonce.ToLower());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public bool DoesUserExist(string userName)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT COUNT(*) FROM [UserProfile] " +
                    "WHERE UserName = @UserName",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserName", userName);

                // execute
                return (int)command.ExecuteScalar() != 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        public void PopulateUserProfile(RegisterModel newUser, string nonce)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET Email = @Email, Nonce = @Nonce " +
                    "WHERE UserName = @UserName",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@Nonce", nonce);
                command.Parameters.AddWithValue("@UserName", newUser.UserName);

                // convert null values to DBNull
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                }

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ChangeProfilePicture(int userId, string b64EncodedImage)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET B64EncodedImage = @B64EncodedImage " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@B64EncodedImage", b64EncodedImage);
                command.Parameters.AddWithValue("@UserId", userId);

                // convert null values to DBNull
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                }

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public User GetUserByUserId(int userId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [UserProfile] " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User()
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = Convert.ToString(reader["UserName"]),
                        //CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        Email = Convert.ToString(reader["Email"]),
                        B64EncodedImage = Convert.ToString(reader["B64EncodedImage"])
                    };

                    reader.Close();

                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        #endregion

        #region Community

        public Community GetCommmunityByCommunityId(int communityId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Community] " +
                    "WHERE CommunityId = @CommunityId", 
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@CommunityId", communityId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Community comm = new Community
                    {
                        CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        Name = Convert.ToString(reader["Name"]),
                        City = Convert.ToString(reader["City"]),
                        State = Convert.ToString(reader["State"]),
                        DefaultMeetingLocation = Convert.ToString(reader["DefaultMeetingLocation"]),
                        Logo = new HtmlImage 
                        {
                            Src = "/Images/" + (CommunityAbbrev)communityId + ".png"
                        }
                    };

                    reader.Close();

                    return comm;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        #endregion

        #region Post

        public Post CreatePost(Post newPost)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [Post] " +
                    "(CommunityId, UserId, CreateDate, ExpirationDate, Title, Description, RequiredPrice, PostType, AuctionType, TransactionType) " +
                    "VALUES (@CommunityId, @UserId, @CreateDate, @ExpirationDate, @Title, @Description, @RequiredPrice, @PostType, @AuctionType, @TransactionType)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@CommunityId", newPost.CommunityId);
                command.Parameters.AddWithValue("@UserId", newPost.UserId);
                command.Parameters.AddWithValue("@CreateDate", newPost.CreateDate);
                command.Parameters.AddWithValue("@ExpirationDate", newPost.ExpirationDate);
                command.Parameters.AddWithValue("@Title", newPost.Title);
                command.Parameters.AddWithValue("@Description", newPost.Description);
                command.Parameters.AddWithValue("@RequiredPrice", newPost.RequiredPrice);
                command.Parameters.AddWithValue("@PostType", newPost.PostType);
                command.Parameters.AddWithValue("@AuctionType", newPost.AuctionType);
                command.Parameters.AddWithValue("@TransactionType", newPost.TransactionType);

                // convert null values to DBNull
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                }

                // execute
                command.ExecuteNonQuery();

                // retrieve the PostId
                newPost.PostId = GetPostByCreateDate(newPost.CreateDate).PostId;

                // add post to categories
                foreach (var category in newPost.Categories)
                {
                    AddPostToCategory(newPost, category);
                }

                return newPost;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null; 
        }

        public Post GetPostByCreateDate(DateTime createDate)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " + 
                    "WHERE CreateDate = @CreateDate", 
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@CreateDate", createDate);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Post post = new Post()
                    {
                        PostId = Convert.ToInt32(reader["PostId"]),
                        CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                        Title = reader["Title"] == DBNull.Value 
                            ? String.Empty : reader["Title"].ToString(),
                        Description = reader["Description"] == DBNull.Value 
                            ? String.Empty : reader["Description"].ToString(),
                        RequiredPrice = reader["RequiredPrice"] == DBNull.Value 
                            ? (int?)null : Convert.ToInt32(reader["RequiredPrice"]), 
                        PostType = (PostType)reader["PostType"],
                        AuctionType = (AuctionType)reader["AuctionType"],
                        TransactionType = (TransactionType)reader["TransactionType"]
                    };

                    reader.Close();

                    // post.Offers = GetOffersByPostId(post.PostId);
                    // if (post.AuctionType == AuctionType.Auction)
                    // {
                    //      post.BestOffer = post.Offers.OrderByDescending(x => x.Amount);
                    // }
                    // 
                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    return post;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public Post GetPostByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Post post = new Post()
                    {
                        PostId = Convert.ToInt32(reader["PostId"]),
                        CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                        Title = reader["Title"] == DBNull.Value
                            ? String.Empty : reader["Title"].ToString(),
                        Description = reader["Description"] == DBNull.Value
                            ? String.Empty : reader["Description"].ToString(),
                        RequiredPrice = reader["RequiredPrice"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["RequiredPrice"]),
                        PostType = (PostType)reader["PostType"],
                        AuctionType = (AuctionType)reader["AuctionType"],
                        TransactionType = (TransactionType)reader["TransactionType"]
                    };

                    reader.Close();

                    post.Offers = GetOffersByPostId(post.PostId);
                    if (post.AuctionType == AuctionType.Auction)
                    {
                        post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).LastOrDefault();
                    }
                    
                    post.Categories = GetCategoriesByPostId(post.PostId);
                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    return post;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<Post> GetPostsByPostTypeAndCommunity(PostType type, CommunityAbbrev community)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "INNER JOIN [Category] ON Post.PostId = Category.PostId " + 
                    "WHERE PostType = @PostType " + 
                    "AND CommunityId = @CommunityId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostType", type);
                command.Parameters.AddWithValue("CommunityId", (int)community);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                List<Post> posts = new List<Post>();

                while (reader.Read())
                {
                    Post post = new Post()
                    {
                        PostId = Convert.ToInt32(reader["PostId"]),
                        CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                        Title = reader["Title"] == DBNull.Value
                            ? String.Empty : reader["Title"].ToString(),
                        Description = reader["Description"] == DBNull.Value
                            ? String.Empty : reader["Description"].ToString(),
                        RequiredPrice = reader["RequiredPrice"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["RequiredPrice"]),
                        PostType = (PostType)reader["PostType"],
                        AuctionType = (AuctionType)reader["AuctionType"],
                        TransactionType = (TransactionType)reader["TransactionType"],
                        Settled = Convert.ToBoolean(reader["Settled"]),
                        Categories = new List<Category>() { (Category)Enum.Parse(typeof(Category), reader["Category"].ToString()) }
                    };

                    posts.Add(post);
                }

                reader.Close();

                // combine categories
                var categoryCombined = posts.GroupBy(l => l.PostId)
                                            .Select(x => new Post { PostId = x.Key, Categories = x.SelectMany(y => y.Categories).ToList() })
                                            .ToList();

                foreach (var post in posts)
                {
                    post.Categories = categoryCombined.FirstOrDefault(x => x.PostId == post.PostId).Categories;
                }

                posts = posts.GroupBy(x => x.PostId).Select(group => group.First()).ToList();

                // get offers and post media
                foreach (var post in posts)
                {
                    post.Offers = GetOffersByPostId(post.PostId);
                    if (post.AuctionType == AuctionType.Auction)
                    {
                        post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).LastOrDefault();
                    }

                    post.PostMedia = GetPostMediaByPostId(post.PostId);
                }

                return posts;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public void SettlePost(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [Post] " +
                    "SET Settled = 1 " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Category

        public void AddPostToCategory(Post post, Category category)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [Category] " +
                    "(PostId, Category, Tags) " +
                    "VALUES (@PostId, @Category, @Tags)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", post.PostId);
                command.Parameters.AddWithValue("@Category", category.ToString());
                command.Parameters.AddWithValue("@Tags", String.Empty); // FIXME WHEN I ADD TAGS

                // execute
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<Category> GetCategoriesByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Category] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                List<Category> categories = new List<Category>();

                while (reader.Read())
                {
                    categories.Add((Category)Enum.Parse(typeof(Category), reader["Category"].ToString())); 
                }

                reader.Close();

                return categories;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        #endregion 

        #region PostMedia

        public PostMedia CreatePostMedia(PostMedia newPostMedia)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [PostMedia] " +
                    "(PostId, B64EncodedImage) " +
                    "VALUES (@PostId, @B64EncodedImage)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", newPostMedia.PostId);
                command.Parameters.AddWithValue("@B64EncodedImage", newPostMedia.B64EncodedImage); 

                // execute
                command.ExecuteNonQuery();

                return GetPostMediaByPostId(newPostMedia.PostId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public PostMedia GetPostMediaByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [PostMedia] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PostMedia postMedia = new PostMedia()
                    {
                        PostMediaId = Convert.ToInt32(reader["PostMediaId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        B64EncodedImage = reader["B64EncodedImage"].ToString()
                    };

                    reader.Close();

                    return postMedia;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        #endregion

        #region Offer

        public Offer CreateOffer(Offer newOffer)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [Offer] " +
                    "(PostId, UserId, Amount, Description, CreateDate, Accepted) " +
                    "VALUES (@PostId, @UserId, @Amount, @Description, @CreateDate, @Accepted)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", newOffer.PostId);
                command.Parameters.AddWithValue("@UserId", newOffer.UserId);
                command.Parameters.AddWithValue("@Amount", newOffer.Amount);
                command.Parameters.AddWithValue("@Description", newOffer.Description);
                command.Parameters.AddWithValue("@CreateDate", newOffer.CreateDate);
                command.Parameters.AddWithValue("@Accepted", newOffer.Accepted);

                // convert null values to DBNull
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                }

                // execute
                command.ExecuteNonQuery();

                newOffer = GetOfferByCreateDate(newOffer.CreateDate);

                // create offermedia here, or make separate????

                return newOffer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public Offer GetOfferByCreateDate(DateTime createDate)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Offer] " +
                    "WHERE CreateDate = @CreateDate",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@CreateDate", createDate);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Offer offer = new Offer()
                    {
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Amount = reader["RequiredPrice"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["RequiredPrice"]),
                        Description = Convert.ToString(reader["Description"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        Accepted = Convert.ToBoolean(reader["Accepted"])
                    };

                    reader.Close();

                    //offer.OfferMedia = GetOfferMediaByOfferId(offer.OfferId);

                    return offer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<Offer> GetOffersByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Offer] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                List<Offer> offers = new List<Offer>();

                while (reader.Read())
                {
                    Offer offer = new Offer()
                    {
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Amount = reader["Amount"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["Amount"]),
                        Description = Convert.ToString(reader["Description"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        Accepted = Convert.ToBoolean(reader["Accepted"])
                    };

                    //offer.OfferMedia = GetOfferMediaByOfferId(offer.OfferId);

                    offers.Add(offer);
                }

                reader.Close();

                return offers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public void AcceptOffer(int offerId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [Offer] " +
                    "SET Accepted = 1 " +
                    "WHERE OfferId = @OfferId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@OfferId", offerId);

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion
    }
}