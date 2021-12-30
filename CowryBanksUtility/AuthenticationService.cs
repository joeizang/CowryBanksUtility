using System.Net.Http.Headers;
using System.Text.Json;

namespace CowryBanksUtility
{
    public interface IAuthenticationService
    {
        Task<IAuthenticationService> GetApiToken();

        Task RefreshToken();

        ApiToken ApiToken { get; }
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationConfiguration _config;

        public AuthenticationService(HttpClient httpClient)
        {
            _http = httpClient;
            _config = new AuthenticationConfiguration(
                "CWRY-QMjMdkxOr1R5sXD2EvmJUPt03KhKURDsPMbM5i5k",
                "CWRY-SECRET-1UZJLwy726yjrw0b66kVudkAr7u5xOReWmBRMvdLivgt4xvBI2FmBlK7qPXApWImyqgS7gMXlyzqBfnzjnZWeqqnfMxoeduXKQHjY1KrDAf8DiOiYbJIf7cFe1OT9sHZ",
                "https://sandbox.embed.cowrywise.com",
                "/o/token/"
            );
        }

        public string GrantType => "client_credentials";

        public ApiToken ApiToken { get; private set; } = new();

        public static string ContentTypeFormUrlEnc { get; } = "application/x-www-form-urlencoded";

        public async Task<IAuthenticationService> GetApiToken()
        {
            try
            {
                _http.BaseAddress = new Uri(_config.EndPointBaseUrl);
                var dictionary = new Dictionary<string, string>
                {
                    {"grant_type", GrantType},
                    {"client_id", _config.ClientId},
                    {"client_secret", _config.ClientSecret}
                };
                HttpContent content = new FormUrlEncodedContent(dictionary);
                var request = new HttpRequestMessage(HttpMethod.Post, _config.TokenEndPoint);
                request.Content = content;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentTypeFormUrlEnc)
                {
                    CharSet = "UTF-8"
                };
                var response = await _http.SendAsync(request).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Token acquisition failure");
                }
                var payload = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                };
                ApiToken = JsonSerializer.Deserialize<ApiToken>(payload, options);
                return this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RefreshToken()
        {
            await GetApiToken().ConfigureAwait(false);
        }
    }
}
