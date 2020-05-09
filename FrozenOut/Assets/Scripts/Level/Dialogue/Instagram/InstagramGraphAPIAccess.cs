using RestSharp;
using RestSharp.Authenticators;

namespace Scripts.Level.Dialogue.Instagram
{
    public class InstagramGraphAPIAccess : IAPIAccess
    {
        public InstagramGraphAPIAccess(string url, string username, string password, string token)
        {
            this.URL = url;
            this.Username = username;
            this.Password = password;
            this.Token = token;
        }

        private readonly string URL;
        private readonly string Username;
        private readonly string Password;
        private readonly string Token;

        public override IRestResponse<T> DoRequest<T>(string resource)
        {
            IRestRequest request = new RestRequest(resource, Method.GET);

            request.AddParameter(this.Token, "access_token");

            IRestClient client = new RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(this.Username, this.Password)
            };

            IRestResponse<T> response = null;//client.Get<T>(request);

            return response;
        }
    }
}