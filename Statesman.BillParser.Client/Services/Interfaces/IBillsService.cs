using Statesman.BillParser.Shared.Models;

namespace Statesman.BillParser.Client.Services.Interfaces;

public interface IBillsService
{
    Task<IEnumerable<BillDto>?> GetUnparsedBillsAsync();
    Task<BillDto?> GetBillAsync(int id);
}
