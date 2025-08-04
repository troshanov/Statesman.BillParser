using System.Net.Http.Json;
using Statesman.BillParser.Shared.Models;
using Statesman.BillParser.Client.Services.Interfaces;

namespace Statesman.BillParser.Client.Services;

public class BillsService : IBillsService
{
    private readonly HttpClient _httpClient;

    public BillsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UnparsedBillDto?> GetBillAsync(int id)
        => await _httpClient.GetFromJsonAsync<UnparsedBillDto>($"api/bills/{id}");

    public async Task<IEnumerable<UnparsedBillDto>?> GetUnparsedBillsAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<UnparsedBillDto>>("api/bills/unparsed");

    public async Task<bool> SaveLegislationAsync(ParsedBillDto legislation)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/bills", legislation);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
