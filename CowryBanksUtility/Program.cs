// See https://aka.ms/new-console-template for more information
using CowryBanksUtility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System.Text.Json.Serialization;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>

        services.AddScoped<IRestClient>(opt => new RestClient("https://sandbox.embed.cowrywise.com"))
        .AddTransient<IHttpService, HttpService>()
        .AddSingleton<IMongoDatabaseSettings, MongoDatabaseSettings>()
        .AddScoped<IMongodbService, MongodbPersistenceService>()
        .AddHttpClient()
        .AddHttpClient<IAuthenticationService, AuthenticationService>()
    ).Build();

List<Bank> banks = new();
int totalPages = 1;

while (totalPages <= 12)
{
    await DoItAsync(host.Services, banks, totalPages);
    totalPages++;
}

//save to db.
SaveToDb(banks, host.Services.CreateScope().ServiceProvider.GetRequiredService<IMongodbService>());

await host.RunAsync();

static async Task<List<Bank>> DoItAsync(IServiceProvider services, List<Bank> banks, int page = 1)
{
    using IServiceScope scope = services.CreateScope();
    var provider = scope.ServiceProvider;
    var auth = provider.GetRequiredService<IAuthenticationService>();
    var httpClient = provider.GetRequiredService<IHttpClientFactory>();
    var _service = provider.GetRequiredService<IHttpService>();

    var inputModel = new GetPaginatedResponseInputModel();
    inputModel.Page = page.ToString();

    var request = new RestRequest("/api/v1/misc/banks", Method.GET);
    request.AddParameter("page", inputModel.Page, ParameterType.GetOrPost);
    request.AddParameter("page_size", inputModel.PageSize, ParameterType.GetOrPost);
    var client = await _service.InitializeClient();
    var result = await client
        .ExecuteAsync<BankResponse>(request)
        .ConfigureAwait(false);
    banks.AddRange(result.Data.Data);
    request = null;
    client = null;
    return banks;
}

static void SaveToDb(List<Bank> banks, IMongodbService mongoService)
{
    if (banks == null || !banks.Any())
        return;
    banks.ForEach(async bank =>
    {
        await mongoService.CreateOneAsync(bank);
    });
}

public class AuthenticationConfiguration
{
    public AuthenticationConfiguration(string clientId, string clientSecret, string endPointBaseUrl, string tokenEndPoint, string grantType = "")
    {
        if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret) &&
            !string.IsNullOrEmpty(endPointBaseUrl) && !string.IsNullOrEmpty(tokenEndPoint))
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            EndPointBaseUrl = endPointBaseUrl;
            TokenEndPoint = tokenEndPoint;
            GrantType = string.IsNullOrEmpty(grantType) ? "client_credentials" : grantType;
        }
        else
        {
            throw new Exception(
                "One or more of the dependencies of this type are not in a valid state!");
        }
    }
    public string GrantType { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string EndPointBaseUrl { get; set; }

    public string TokenEndPoint { get; set; }

    public string ApiVersion { get; set; } = "v1";
}

public class ApiToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}