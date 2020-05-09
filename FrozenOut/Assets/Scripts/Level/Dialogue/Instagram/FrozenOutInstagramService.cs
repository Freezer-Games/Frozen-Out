using System.Collections.Generic;
using System.Linq;

using Scripts.Level.Dialogue.Instagram.Model;

namespace Scripts.Level.Dialogue.Instagram
{
    public class FrozenOutInstagramService
    {
        public FrozenOutInstagramService(InstagramRepository repository)
        {
            this.Repository = repository;
        }

        public FrozenOutInstagramService()
        {
            this.Repository = new InstagramRepository(new InstagramGraphAPIAccess("https://graph.facebook.com/v7.0/", "", "", "EAAqLlZADTcb4BAOgIdXxl5PcsLnnzZBifsJRz9ZC2ZB3xYVPNZCq0sZCA1U6YAEeNZAWNEADMx6tbIgepWIhRpZAVget1NBRz93XGyNaoVK8ZAf3ZAuPTG2ZAF818TXBxUjRatENblSSeHHk9YF1BRjQYTsCzlZAF5eoN6ZAjFiojTeuZAViYxsKv55zPPYG0CMpWQSfMVY4gblF24lK4UUXgZBSLAZC"));
        }

        private InstagramRepository Repository;

        private readonly string FROZENOUT_PAGE_NAME = "Frozen-Out";

        /*public Comment GetLastComment()
        {
            Post post = GetLastPost();

            Comment lastComment = post.Comments.Last();

            return lastComment;
        }*/

        /*public Post GetLastPost()
        {
            ICollection<Post> posts = Repository.GetPosts();

            Post lastPost = posts.Last();
            lastPost.Comments = Repository.GetComments(lastPost.Id);

            return lastPost;
        }*/

        private User GetFrozenOutInstagram()
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