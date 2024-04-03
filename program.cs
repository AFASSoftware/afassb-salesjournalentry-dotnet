using AFASSB.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static string accessToken = string.Empty;
    private static string ClientUrl = "https://app-center-demo.afasfocus.nl/demo";
    private static string ClientId = "your client id";
    private static string ClientSecret = "your client secret";
    private static string RedirectUri = "https://try.afassb.nl/callback";
    private static string refreshToken = string.Empty;

    static async Task Main(string[] args)
    {
        AfasAuthClient afasAuthClient = new AfasAuthClient(ClientUrl, ClientId, ClientSecret, RedirectUri);

        string state = Guid.NewGuid().ToString();
        string codeVerifier = afasAuthClient.GenerateCodeVerifier();
        string codeChallenge = afasAuthClient.GenerateCodeChallenge(codeVerifier);
        string authorizationCode = await afasAuthClient.AuthorizeWithPKCE(state, codeChallenge);


        await afasAuthClient.GetAccessTokenAsync(authorizationCode, codeVerifier, state);

        // Get the access token from the AfasAuthClient
        string accessToken = afasAuthClient.AccessToken;

        // Create an instance of the "AfasApiClient" class
        AfasApiClient afasApiClient = new AfasApiClient(ClientUrl, accessToken);
        
        // Example usage
        List<Organisation> organizations = await afasApiClient.GetOrganisationsAsync(take: 5);
        List<Person> persons = await afasApiClient.GetPersonssAsync(take: 5);
        List<PaymentCondition> paymentCondition = await afasApiClient.GetPaymentConditionsAsync(take: 5);
        List<Administration> administrations = await afasApiClient.GetAdministrationsAsync(take: 5);
        List<LedgerAccount> ledgerAccounts = await afasApiClient.GetLedgerAccountsAsync(take: 5);


        // Send sample sales journal entry
        var salesJournal = new SalesJournalEntry
        {
            AdministrationId = administrations[0].Id,
            InvoiceDate = "2024-04-01",
            InvoiceNumber = "INV-001",
            RelationType = "organisation",
            RelationId = organizations[0].Id,
            InvoiceLine = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    LedgerAccountId = ledgerAccounts[0].Id,
                    AmountExcludingVat = 100,
                    VatType = "high"
                }
            }.ToArray()
        };

        await afasApiClient.SendSalesJournalAsync(salesJournal);

        Console.WriteLine("Sales journal entry sent successfully.");
    }

    

    private static async Task<List<Organisation>> GetOrganizations(int take = 100, int skip = 0, string filter = "")
    {
        var url = $"{ClientUrl}/api/organisations?take={take}&skip={skip}&filter={Uri.EscapeDataString(filter)}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());
        return result.GetProperty("Result").Deserialize<List<Organisation>>();
    }

    private static async Task<List<Person>> GetPersons(int take = 100, int skip = 0, string filter = "")
    {
        var url = $"{ClientUrl}/api/persons?take={take}&skip={skip}&filter={Uri.EscapeDataString(filter)}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());
        return result.GetProperty("Result").Deserialize<List<Person>>();
    }

    private static async Task<List<Administration>> GetAdministrations(int take = 100, int skip = 0, string filter = "")
    {
        var url = $"{ClientUrl}/api/administration?take={take}&skip={skip}&filter={Uri.EscapeDataString(filter)}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());
        return result.Deserialize<List<Administration>>();
    }

    private static async Task<List<LedgerAccount>> GetLedgerAccounts(int take = 100, int skip = 0, string filter = "")
    {
        var url = $"{ClientUrl}/api/ledgeraccount?take={take}&skip={skip}&filter={Uri.EscapeDataString(filter)}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());
        return result.Deserialize<List<LedgerAccount>>();
    }

    private static async Task SendSalesJournal(SalesJournalEntry salesJournal)
    {
        var url = $"{ClientUrl}/api/salesjournalentry";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.PostAsJsonAsync(url, salesJournal);
        response.EnsureSuccessStatusCode();
    }
}