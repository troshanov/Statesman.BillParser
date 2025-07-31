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

    public async Task<BillDto?> GetBillAsync(int id)
        => await _httpClient.GetFromJsonAsync<BillDto>($"api/bills/{id}");

    public async Task<IEnumerable<BillDto>?> GetUnparsedBillsAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<BillDto>>("api/bills/unparsed");
}
