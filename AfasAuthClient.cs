using AFASSB.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

public class AfasAuthClient
{
    private readonly string _clientUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;
    private readonly HttpClient httpClient = new HttpClient();

    public string AccessToken { get; private set; }


    public AfasAuthClient(string clientUrl, string clientId, string clientSecret, string redirectUri)
    {
        _clientUrl = clientUrl;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _redirectUri = redirectUri;
    }

    public async Task<string> AuthorizeWithPKCE(string state, string codeChallenge)
    {

        string authorizationRequest = $"{_clientUrl}/app/auth?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&code_challenge={codeChallenge}&code_challenge_method=S256&state={state}";

        // Simulate user interaction and obtain the authorization code
        Console.WriteLine($"Open the following URL in a web browser to authorize the application: {authorizationRequest}");
        Console.WriteLine("Enter the authorization code:");
        string authorizationCode = Console.ReadLine();

        return authorizationCode;
    }

    public string GenerateCodeVerifier()
    {
        byte[] randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public string GenerateCodeChallenge(string codeVerifier)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            return Convert.ToBase64String(challengeBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }

    public async Task GetAccessTokenAsync(string authorizationCode, string codeVerifier, string state)
    {
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("redirect_uri", _redirectUri),
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("code_verifier", codeVerifier),
            new KeyValuePair<string, string>("state", state),
        });

        var response = await httpClient.PostAsync($"{_clientUrl}/app/token", requestBody);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());
        AccessToken = tokenResponse.GetProperty("access_token").GetString();
    }
}