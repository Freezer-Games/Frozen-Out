using RestSharp;

namespace Scripts.Level.Dialogue.Instagram
{
    public abstract class IAPIAccess
    {
        public abstract IRestResponse<T> DoRequest<T>(string request)
            where T : class;
    }
}