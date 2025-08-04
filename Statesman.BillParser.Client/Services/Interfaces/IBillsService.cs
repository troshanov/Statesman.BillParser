using Statesman.BillParser.Shared.Models;

namespace Statesman.BillParser.Client.Services.Interfaces;

public interface IBillsService
{
    Task<IEnumerable<UnparsedBillDto>?> GetUnparsedBillsAsync();
    Task<UnparsedBillDto?> GetBillAsync(int id);
    Task<bool> SaveLegislationAsync(ParsedBillDto legislation);
}
