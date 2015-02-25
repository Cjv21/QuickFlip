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
                        CommunityName = Convert.ToString(reader["CommunityName"]),
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

                // temporarily save categories
                List<Category> categories = newPost.Categories;

                // retrieve the newly created post so we can access the PostId
                newPost = GetPostByCreateDate(newPost.CreateDate);

                // restore categories
                newPost.Categories = categories;

                // add post to each category it belongs to
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
                    // post.Categories = GetCategoriesByPostId(post.PostId);
                    // post.PostMedia = GetPostMediaByPostId(post.PostId);

                    return post;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public void AddPostToCategory(Post post, Category category)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "INSERT INTO [" + category.ToString() + "] " +
                    "(PostId, Tags) " + 
                    "VALUES (@PostId, @Tags)",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostId", post.PostId);
                command.Parameters.AddWithValue("@Tags", String.Empty); // FIXME WHEN I ADD TAGS

                // execute
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<Post> GetPostsByPostType(PostType type)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Post] " +
                    "WHERE PostType = @PostType",
                    Connection);

                // add parameters
                command.Parameters.AddWithValue("@PostType", type);

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
                        TransactionType = (TransactionType)reader["TransactionType"]
                    };

                    posts.Add(post);
                }

                reader.Close();

                foreach (var post in posts)
                {
                    // post.Offers = GetOffersByPostId(post.PostId);
                    // if (post.AuctionType == AuctionType.Auction)
                    // {
                    //      post.BestOffer = post.Offers.OrderByDescending(x => x.Amount);
                    // }
                    // 
                    // post.Categories = GetCategoriesByPostId(post.PostId);
                    // post.PostMedia = GetPostMediaByPostId(post.PostId);
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

        #region PostMedia

        public PostMedia CreatePostMedia(PostMedia newPostMedia)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        #endregion
    }
}