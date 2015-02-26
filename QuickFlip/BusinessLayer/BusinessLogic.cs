using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickFlip.DataAccessLayer;
using QuickFlip.Models;

namespace QuickFlip.BusinessLayer
{
    public class BusinessLogic
    {
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

        public static void AddPostToCategory(Post post, Category category)
        {
            DataAccess da = new DataAccess();

            da.AddPostToCategory(post, category);

            da.Dispose();
        }

        public static Post GetPostByPostId(int postId)
        {
            DataAccess da = new DataAccess();

            Post post = da.GetPostByPostId(postId);

            da.Dispose();

            return post;
        }

        public static List<Post> GetPostsByPostType(PostType type)
        {
            DataAccess da = new DataAccess();

            List<Post> buyPosts = da.GetPostsByPostType(type);

            da.Dispose();

            return buyPosts;
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

        #endregion PostMedia

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

        #endregion
    }
}