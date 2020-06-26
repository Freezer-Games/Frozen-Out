using System;
using System.Security.Authentication;

using RestSharp;

namespace InstagramConnection
{
    public class InstagramGraphAPIAccess : IAPIAccess
    {
        public InstagramGraphAPIAccess(string baseUrl, string token)
        {
            this.BaseUrl = baseUrl;
            this.Token = token;
        }

        private readonly string BaseUrl;
        private readonly string Token;

        public override IRestResponse<T> DoRequest<T>(string queryStringResource, Method method)
        {
            IRestRequest request = new RestRequest(queryStringResource, method)
            {
                RequestFormat = DataFormat.Json,
            };
            request.AddParameter("access_token", this.Token);

            IRestClient client = new RestClient(this.BaseUrl);

            IRestResponse<T> response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new InvalidOperationException();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new InvalidCredentialException();

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException();

            if (response == null || !(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent))
                throw new InvalidOperationException();

            return response;
        }
    }
}