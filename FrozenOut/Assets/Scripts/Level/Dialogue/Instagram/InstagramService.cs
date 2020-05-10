using System.Collections.Generic;
using System.Linq;

using Scripts.Level.Dialogue.Instagram.Model;

namespace Scripts.Level.Dialogue.Instagram
{
    public class InstagramService
    {
        public InstagramService(InstagramRepository repository)
        {
            this.Repository = repository;

            this.FrozenOutInstagramUser = GetFrozenOutInstagramUser();
        }

        public InstagramService() : this(new InstagramRepository(new InstagramGraphAPIAccess("https://graph.facebook.com/v7.0/", "EAAqLlZADTcb4BAEjhEmWoQwdviJ7zAYzmkBW6EOn3PxPZCwxD064G0HGZBcCLstdaAqs4oJl1NiJMfFsB7OYO7uwZBrtS55o9Tpv25VTtHAWQZBGpC1DbNPZBw6PO444UewfraidqMUQykasE6R3F2ZBzLqftexf7qOgHEUVujCHxLOrBkoyFaQ")))
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

            Post latestPost = posts.Last();
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