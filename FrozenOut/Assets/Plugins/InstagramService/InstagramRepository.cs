using System.Collections.Generic;

using RestSharp;

using InstagramConnection.Entity;
using InstagramConnection.Model;
using InstagramConnection.Utils;

namespace InstagramConnection
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
            string resourceQuery = $"{user.Id}/media?fields=id,username,caption,timestamp,like_count,comments_count";

            IRestResponse<InstagramMediaData> response = ApiAccess.DoRequest<InstagramMediaData>(resourceQuery, Method.GET);

            ICollection<Post> posts = new List<Post>();
            foreach (InstagramMedia instaMedia in response.Data.Data)
            {
                Post post = MapInstagramMedia(instaMedia);
                posts.Add(post);
            }

            return posts;
        }

        public ICollection<Comment> GetComments(Post post)
        {
            string resourceQuery = $"{post.Id}?fields=comments" + "{id,username,text,timestamp,like_count}";

            IRestResponse<InstagramMedia> response = ApiAccess.DoRequest<InstagramMedia>(resourceQuery, Method.GET);

            ICollection<Comment> comments = new List<Comment>();
            foreach (InstagramComment instaComment in response.Data.Comments.Data)
            {
                Comment comment = MapInstagramComment(instaComment);
                comments.Add(comment);
            }

            return comments;
        }

        public User GetUserFromFacebookPage(FacebookPage page)
        {
            string resourceQuery = $"{page.Id}?fields=instagram_business_account" + "{id,name}";

            IRestResponse<FacebookAccount> response = ApiAccess.DoRequest<FacebookAccount>(resourceQuery, Method.GET);

            User user = MapInstagramAccount(response.Data.Instagram_business_account);

            return user;
        }

        public Model.FacebookUser GetCurrentFacebookUser()
        {
            string resourceQuery = "me?fields=id,name";

            IRestResponse<Entity.FacebookUser> response = ApiAccess.DoRequest<Entity.FacebookUser>(resourceQuery, Method.GET);

            Model.FacebookUser user = MapFacebookUser(response.Data);

            return user;
        }

        public ICollection<FacebookPage> GetCurrentFacebookUserPages()
        {
            string resourceQuery = "me/accounts";

            IRestResponse<FacebookAccountData> response = ApiAccess.DoRequest<FacebookAccountData>(resourceQuery, Method.GET);

            ICollection<FacebookPage> pages = new List<FacebookPage>();
            foreach (FacebookAccount fbAccount in response.Data.Data)
            {
                FacebookPage page = MapFacebookAccount(fbAccount);
                pages.Add(page);
            }

            return pages;
        }

        #region Mapping
        private User MapInstagramAccount(InstagramAccount reader)
        {
            User user = new User()
            {
                Id = reader.Id,
                Name = reader.Name
            };

            return user;
        }

        private Post MapInstagramMedia(InstagramMedia reader)
        {
            Post post = new Post()
            {
                Id = reader.Id,
                Username = reader.Username,
                Text = reader.Caption,
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