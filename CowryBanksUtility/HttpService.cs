using RestSharp;
using RestSharp.Serializers.SystemTextJson;
using System.Text.Json;

namespace CowryBanksUtility
{
    public interface IHttpService
    {
        Task<IRestClient> InitializeClient();
    }
    public class HttpService : IHttpService
    {
        public HttpService(IAuthenticationService service, IRestClient client)
        {
            _auth = service;
            _client = client;
        }

        private readonly IAuthenticationService _auth;

        private readonly IRestClient _client;

        public async Task<IRestClient> InitializeClient()
        {
            await _auth.GetApiToken()
                .ConfigureAwait(false);
            if (!string.IsNullOrEmpty(_auth.ApiToken.AccessToken))
            {
                _client.UseSystemTextJson(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                _client.AddDefaultHeaders(new Dictionary<string, string>
                {
                    {"Authorization", $"Bearer {_auth.ApiToken.AccessToken}"}
                });
                return _client;
            }

            await _auth.RefreshToken().ConfigureAwait(false);
            _client.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            _client.AddDefaultHeaders(new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {_auth.ApiToken.AccessToken}"}
            });

            return _client;
        }
    }
}
