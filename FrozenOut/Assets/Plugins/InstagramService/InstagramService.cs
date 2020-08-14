using System.Collections.Generic;
using System.Linq;

using InstagramConnection.Model;

namespace InstagramConnection
{
    public class InstagramService
    {
        public InstagramService(InstagramRepository repository)
        {
            this.Repository = repository;

            this.FrozenOutInstagramUser = GetFrozenOutInstagramUser();
        }

        public InstagramService() : this(new InstagramRepository(new InstagramGraphAPIAccess("https://graph.facebook.com/v7.0/", "")))
        {
        }

        private readonly InstagramRepository Repository;
        private readonly User FrozenOutInstagramUser;

        private readonly string FROZENOUT_PAGE_NAME = "Frozen-Out";

        public ICollection<Comment> GetLatestPostComments()
        {
            Post post = GetLatestPost();

            ICollection<Comment> latestComments = post.Comments;

            return latestComments;
        }

        public Post GetLatestPost()
        {
            ICollection<Post> posts = Repository.GetPosts(this.FrozenOutInstagramUser);

            Post latestPost = posts.First();
            latestPost.Comments = Repository.GetComments(latestPost);

            return latestPost;
        }

        private User GetFrozenOutInstagramUser()
        {
            // Obtener las páginas del usuario de facebook
            ICollection<FacebookPage> fbPages = Repository.GetCurrentFacebookUserPages();
            // Coger solamente la página de Frozen-Out
            FacebookPage frozenOutPage = fbPages.Single<FacebookPage>(fbPage => fbPage.Name == FROZENOUT_PAGE_NAME);
            // Obtener el instagram vinculado a esa página
            User frozenOutInstagram = Repository.GetUserFromFacebookPage(frozenOutPage);

            return frozenOutInstagram;
        }
    }
}