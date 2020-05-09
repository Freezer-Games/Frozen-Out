using System.Collections.Generic;

using RestSharp;

using Scripts.Level.Dialogue.Instagram.Entity;
using Scripts.Level.Dialogue.Instagram.Model;
using Scripts.Level.Dialogue.Instagram.Utils;

namespace Scripts.Level.Dialogue.Instagram
{
    public class InstagramRepository
    {
        public InstagramRepository(IAPIAccess apiAccess)
        {
            this.ApiAccess = apiAccess;
        }

        private readonly IAPIAccess ApiAccess;

        public ICollection<Post> GetPosts(User user)
        {
            string query = $"{user.Id}";
            string request = "https://graph.facebook.com/v7.0/" + query;

            IRestResponse<List<InstagramMedia>> response = ApiAccess.DoRequest<List<InstagramMedia>>(request);

            ICollection<Post> posts = new List<Post>();
            foreach (InstagramMedia instaMedia in response.Data)
            {
                Post post = MapInstagramMedia(instaMedia);
                posts.Add(post);
            }

            return posts;
        }

        public ICollection<Comment> GetComments(Post post)
        {
            string query = $"{post.Id}";
            string request = "https://graph.facebook.com/v7.0/" + query;

            IRestResponse<List<InstagramComment>> response = ApiAccess.DoRequest<List<InstagramComment>>(request);

            ICollection<Comment> comments = new List<Comment>();
            foreach (InstagramComment instaComment in response.Data)
            {
                Comment comment = MapInstagramComment(instaComment);
                comments.Add(comment);
            }

            return comments;
        }

        public User GetUserFromFacebookPage(FacebookPage page)
        {
            string query = $"{page.Id}?fields=instagram_business_account";
            string request = "https://graph.facebook.com/v7.0/" + query;

            IRestResponse<InstagramUser> response = ApiAccess.DoRequest<InstagramUser>(request);

            User user = MapInstagramUser(response.Data);
            user.Name = page.Name;

            return user;
        }

        public Model.FacebookUser GetCurrentFacebookUser()
        {
            string query = "me?fields=id,name";
            string request = "https://graph.facebook.com/v7.0/" + query;

            IRestResponse<Entity.FacebookUser> response = ApiAccess.DoRequest<Entity.FacebookUser>(request);

            Model.FacebookUser user = MapFacebookUser(response.Data);

            return user;
        }

        public ICollection<FacebookPage> GetCurrentFacebookUserPages()
        {
            string query = "me/accounts";
            string request = "https://graph.facebook.com/v7.0/" + query;

            IRestResponse<List<FacebookAccount>> response = ApiAccess.DoRequest<List<FacebookAccount>>(request);

            ICollection<FacebookPage> pages = new List<FacebookPage>();
            foreach (FacebookAccount fbAccount in response.Data)
            {
                FacebookPage page = MapFacebookAccount(fbAccount);
                pages.Add(page);
            }

            return pages;
        }

        #region Mapping
        private User MapInstagramUser(InstagramUser reader)
        {
            User user = new User()
            {
                Id = reader.Id,
            };

            return user;
        }

        private Post MapInstagramMedia(InstagramMedia reader)
        {
            Post post = new Post()
            {
                Id = reader.Id,
                Username = reader.Username,
                Timestamp = DataReaderUtils.GetDateTime(reader.Timestamp),
                Likes = DataReaderUtils.GetInt(reader.Like_count)
            };

            return post;
        }

        private Comment MapInstagramComment(InstagramComment reader)
        {
            Comment comment = new Comment()
            {
                Id = reader.Id,
                Username = reader.Username,
                Text = reader.Text,
                Timestamp = DataReaderUtils.GetDateTime(reader.Timestamp),
                Likes = DataReaderUtils.GetInt(reader.Like_count)
            };

            return comment;
        }

        private Model.FacebookUser MapFacebookUser(Entity.FacebookUser reader)
        {
            Model.FacebookUser fbUser = new Model.FacebookUser()
            {
                Id = reader.Id,
                Name = reader.Name,
            };

            return fbUser;
        }

        private FacebookPage MapFacebookAccount(FacebookAccount reader)
        {
            FacebookPage fbPage = new FacebookPage()
            {
                Id = reader.Id,
                Name = reader.Name,
            };

            return fbPage;
        }
        #endregion
    }
}