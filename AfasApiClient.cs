using AFASSB.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

public class AfasApiClient
{
    private readonly string _clientUrl;
    private readonly string _accessToken;

    public AfasApiClient(string clientUrl, string accessToken)
    {
        _clientUrl = clientUrl;
        _accessToken = accessToken;
    }

    public async Task<List<Administration>> GetAdministrationsAsync(int take = 5)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "1.0");

        var response = await client.GetAsync($"{_clientUrl}/api/administration?take={take}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching administration: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var administrations = JsonSerializer.Deserialize<List<Administration>>(json);

        return administrations;
    }

    public async Task<List<Organisation>> GetOrganisationsAsync(int take = 5)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "2.0");

        var response = await client.GetAsync($"{_clientUrl}/api/organisations?take={take}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching organisations: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var organisationsObject = JsonSerializer.Deserialize<OrganisationRoot>(json);
        var organisationsTrackingToken = organisationsObject.TrackingToken;
        var organisations = organisationsObject.Result.ToList();

        return organisations;
    }

    public async Task<List<Person>> GetPersonssAsync(int take = 5)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "2.0");

        var response = await client.GetAsync($"{_clientUrl}/api/persons?take={take}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching persons: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        
        var personsObject = JsonSerializer.Deserialize<PersonRoot>(json);
        var personsTrackingToken = personsObject.TrackingToken;
        var persons = personsObject.Result.ToList();

        return persons;
    }

    public async Task<List<PaymentCondition>> GetPaymentConditionsAsync(int take = 5)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "1.0");

        var response = await client.GetAsync($"{_clientUrl}/api/paymentconditions?take={take}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching payment conditions: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var paymentConditions = JsonSerializer.Deserialize<List<PaymentCondition>>(json);

        return paymentConditions;
    }

    public async Task<List<LedgerAccount>> GetLedgerAccountsAsync(int take = 5)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "1.0");

        var response = await client.GetAsync($"{_clientUrl}/api/ledgeraccount?take={take}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching ledger accounts: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var ledgerAccounts = JsonSerializer.Deserialize<List<LedgerAccount>>(json);

        return ledgerAccounts;
    }

    public async Task<Guid> UploadAttachmentAsync(byte[] attachmentData)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "1.0");

        var content = new ByteArrayContent(attachmentData);
        var response = await client.PostAsync($"{_clientUrl}/api/blob", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error uploading attachment: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var attachmentId = JsonSerializer.Deserialize<Guid>(json);

        return attachmentId;
    }

    public async Task SendSalesJournalAsync(SalesJournalEntry salesJournalEntry)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Accept-Version", "1.0");

        var json = JsonSerializer.Serialize(salesJournalEntry);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{_clientUrl}/api/salesjournalentry", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error sending sales journal entry: {response.ReasonPhrase}");
        }
    }
}