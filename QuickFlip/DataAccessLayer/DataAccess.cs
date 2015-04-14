using QuickFlip.Models;
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

        public void ChangeHomeCommunity(int userId, CommunityAbbrev comm)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET CommunityId = @CommunityId " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@CommunityId", (int)comm);

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
                        CommunityId = reader["CommunityId"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["CommunityId"]),
                        Phone = reader["Phone"] == DBNull.Value
                            ? (Int64?)null : Convert.ToInt64(reader["Phone"]),
                        Carrier = reader["Carrier"] == DBNull.Value
                            ? (Carrier?)null : (Carrier)Int32.Parse(reader["Carrier"].ToString()),
                        AlertMode = (AlertMode)Int32.Parse(reader["AlertMode"].ToString()),
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

        public void ChangePhone(int userId, Int64 phoneNumber, Carrier carrier)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET Phone = @Phone, Carrier = @Carrier " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Phone", phoneNumber);
                command.Parameters.AddWithValue("@Carrier", carrier);

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ChangeAlertMode(int userId, AlertMode mode)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "UPDATE [UserProfile] " +
                    "SET AlertMode = @AlertMode " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@AlertMode", (int)mode);

                // execute
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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

                // add post to tags
                foreach (var tag in newPost.Tags)
                {
                    AddPostToTag(newPost, tag);
                }

                return newPost;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null; 
        }

        public Post EditPost(Post editedPost)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "Update [Post] " +
                    "SET CommunityId = @CommunityId, UserId = @UserId, CreateDate = @CreateDate, ExpirationDate = @ExpirationDate, Title = @Title, Description = @Description, RequiredPrice = @RequiredPrice, PostType = @PostType, AuctionType = @AuctionType, TransactionType = @TransactionType " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", editedPost.PostId);
                command.Parameters.AddWithValue("@CommunityId", editedPost.CommunityId);
                command.Parameters.AddWithValue("@UserId", editedPost.UserId);
                command.Parameters.AddWithValue("@CreateDate", editedPost.CreateDate);
                command.Parameters.AddWithValue("@ExpirationDate", editedPost.ExpirationDate);
                command.Parameters.AddWithValue("@Title", editedPost.Title);
                command.Parameters.AddWithValue("@Description", editedPost.Description);
                command.Parameters.AddWithValue("@RequiredPrice", editedPost.RequiredPrice);
                command.Parameters.AddWithValue("@PostType", editedPost.PostType);
                command.Parameters.AddWithValue("@AuctionType", editedPost.AuctionType);
                command.Parameters.AddWithValue("@TransactionType", editedPost.TransactionType);

                // convert null values to DBNull
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                }

                // execute
                command.ExecuteNonQuery();

                // delete old categories and tags
                DeleteFromCategories(editedPost.PostId);
                DeleteFromTags(editedPost.PostId);

                // add post to categories
                foreach (var category in editedPost.Categories)
                {
                    AddPostToCategory(editedPost, category);
                }

                // add post to tags
                foreach (var tag in editedPost.Tags)
                {
                    AddPostToTag(editedPost, tag);
                }

                return editedPost;
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
                        post.BestOffer = post.PostType == PostType.Buy
                            ? post.Offers.OrderByDescending(x => x.Amount).LastOrDefault()
                            : post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).FirstOrDefault();
                    }
                    
                    post.Categories = GetCategoriesByPostId(post.PostId);
                    post.PostMedia = GetPostMediaByPostId(post.PostId);
                    post.Tags = GetTagsByPostId(post.PostId);

                    return post;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "INNER JOIN [Category] ON Post.PostId = Category.PostId " +
                    "WHERE UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);

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
                        post.BestOffer = post.PostType == PostType.Buy
                            ? post.Offers.OrderByDescending(x => x.Amount).LastOrDefault()
                            : post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).FirstOrDefault();
                    }

                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    post.Tags = GetTagsByPostId(post.PostId);
                }

                return posts;

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
                        post.BestOffer = post.PostType == PostType.Buy
                            ? post.Offers.OrderByDescending(x => x.Amount).LastOrDefault()
                            : post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).FirstOrDefault();
                    }

                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    post.Tags = GetTagsByPostId(post.PostId);
                }

                return posts;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<Post> GetAllPosts()
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "INNER JOIN [Category] ON Post.PostId = Category.PostId",
                    Connection);

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
                        post.BestOffer = post.PostType == PostType.Buy
                            ? post.Offers.OrderByDescending(x => x.Amount).LastOrDefault()
                            : post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).FirstOrDefault();
                    }

                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    post.Tags = GetTagsByPostId(post.PostId);
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

        public void DeletePost(int postId)
        {
            // delete post media
            DeletePostMedia(postId);

            // remove from categories
            DeleteFromCategories(postId);

            // remove from tags
            DeleteFromTags(postId);

            // delete offers
            DeleteOffersByPostId(postId);

            // delete post
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Post] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<Post> GetPostsByBidderId(int userId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "INNER JOIN [Category] ON Post.PostId = Category.PostId " +
                    "INNER JOIN [Offer] ON Post.PostId = Offer.PostId " +
                    "WHERE Offer.UserId = @UserId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@UserId", userId);

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
               

                // get offers, post media, and tags
                foreach (var post in posts)
                {
                    post.Offers = GetOffersByPostId(post.PostId);
                    if (post.AuctionType == AuctionType.Auction)
                    {
                        post.BestOffer = post.PostType == PostType.Buy 
                            ? post.Offers.OrderByDescending(x => x.Amount).LastOrDefault() 
                            : post.BestOffer = post.Offers.OrderByDescending(x => x.Amount).FirstOrDefault();
                    }

                    post.PostMedia = GetPostMediaByPostId(post.PostId);

                    post.Tags = GetTagsByPostId(post.PostId);
                }

                return posts;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
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
                    "(PostId, Category) " +
                    "VALUES (@PostId, @Category)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", post.PostId);
                command.Parameters.AddWithValue("@Category", category.ToString());

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

        public void DeleteFromCategories(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Category] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion 

        #region Tag

        public void AddPostToTag(Post post, string tag)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [Tag] " +
                    "(PostId, Tag) " +
                    "VALUES (@PostId, @Tag)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", post.PostId);
                command.Parameters.AddWithValue("@Tag", tag);

                // execute
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<string> GetTagsByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Tag] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                List<string> tags = new List<string>();

                while (reader.Read())
                {
                    tags.Add(reader["Tag"].ToString());
                }

                reader.Close();

                return tags;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        public void DeleteFromTags(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Tag] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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

        public void DeletePostMedia(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [PostMedia] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
                        Amount = reader["Amount"] == DBNull.Value
                            ? (int?)null : Convert.ToInt32(reader["Amount"]),
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

        public void DeleteOffersByPostId(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Offer] " +
                    "WHERE PostId = @PostId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", postId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void DeleteOffer(int offerId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Offer] " +
                    "WHERE OfferId = @OfferId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@OfferId", offerId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        #region OfferMedia

        public OfferMedia CreateOfferMedia(OfferMedia newOfferMedia)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [OfferMedia] " +
                    "(OfferId, B64EncodedImage) " +
                    "VALUES (@OfferId, @B64EncodedImage)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@OfferId", newOfferMedia.OfferId);
                command.Parameters.AddWithValue("@B64EncodedImage", newOfferMedia.B64EncodedImage);

                // execute
                command.ExecuteNonQuery();

                return GetOfferMediaByOfferId(newOfferMedia.OfferId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public OfferMedia GetOfferMediaByOfferId(int offerId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [OfferMedia] " +
                    "WHERE OfferId = @OfferId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@OfferId", offerId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OfferMedia offerMedia = new OfferMedia()
                    {
                        OfferMediaId = Convert.ToInt32(reader["OfferMediaId"]),
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        B64EncodedImage = reader["B64EncodedImage"].ToString()
                    };

                    reader.Close();

                    return offerMedia;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public void DeleteOfferMedia(int offerId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [OfferMedia] " +
                    "WHERE OfferId = @OfferId",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@OfferId", offerId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Alert

        public Alert CreateAlert(
            int postId,
            int userId,
            int offerId,
            AlertType type)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [Alert] " +
                    "(PostId, UserId, OfferId, AlertType, CreateDate) " +
                    "VALUES (@PostId, @UserId, @OfferId, @AlertType, @CreateDate)", 
                    Connection);

                DateTime createDate = DateTime.Now;
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OfferId", offerId);
                command.Parameters.AddWithValue("@AlertType", (int)type);
                command.Parameters.AddWithValue("@CreateDate", createDate);

                command.ExecuteNonQuery();

                return GetAlertByCreateDate(createDate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        public Alert GetAlertByAlertId(int alertId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Alert] "+
                    "WHERE AlertId = @AlertId", 
                    Connection);

                command.Parameters.AddWithValue("@AlertId", alertId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = Convert.ToInt32(reader["AlertId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        Type = (AlertType)reader["AlertType"],
                        CreateDate = Convert.ToDateTime(reader["CreateDate"])
                    };

                    reader.Close();

                    return alert;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public Alert GetAlertByCreateDate(DateTime createDate)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Alert] " +
                    "WHERE CreateDate = @CreateDate", 
                    Connection);

                command.Parameters.AddWithValue("@CreateDate", createDate);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = Convert.ToInt32(reader["AlertId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        Type = (AlertType)reader["AlertType"],
                        CreateDate = Convert.ToDateTime(reader["CreateDate"])
                    };

                    reader.Close();

                    return alert;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }


        public List<Alert> GetAlertsByUserId(int userId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Alert] " +
                    "WHERE UserId = @UserId",
                    Connection);

                command.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = command.ExecuteReader();

                List<Alert> alertList = new List<Alert>();

                while (reader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = Convert.ToInt32(reader["AlertId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        Type = (AlertType)reader["AlertType"],
                        CreateDate = Convert.ToDateTime(reader["CreateDate"])
                    };

                    alertList.Add(alert);
                }

                reader.Close();

                return alertList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<Alert> GetAlertsByPostId(int postId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Alert] " +
                    "WHERE PostId = @PostId", 
                    Connection);

                command.Parameters.AddWithValue("@PostId", postId);

                SqlDataReader reader = command.ExecuteReader();

                List<Alert> alertList = new List<Alert>();

                while (reader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = Convert.ToInt32(reader["AlertId"]),
                        PostId = Convert.ToInt32(reader["PostId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        OfferId = Convert.ToInt32(reader["OfferId"]),
                        Type = (AlertType)reader["AlertType"],
                        CreateDate = Convert.ToDateTime(reader["CreateDate"])
                    };

                    alertList.Add(alert);
                }

                reader.Close();

                return alertList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public void DeleteAlert(int alertId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Alert] " +
                    "WHERE AlertId = @AlertId", 
                    Connection);

                command.Parameters.AddWithValue("@AlertId", alertId);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void DeleteAlerts(int postId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "DELETE FROM [Alert] " +
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
    }
}