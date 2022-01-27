// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}


//请求令牌
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}
Console.WriteLine(tokenResponse.Json);


var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);
var response = await apiClient.GetAsync("https://localhost:5001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JArray.Parse(content));
}

