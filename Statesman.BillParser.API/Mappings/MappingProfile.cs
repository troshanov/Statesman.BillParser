using AutoMapper;
using Statesman.BillParser.Shared.Models;
using Statesman.BillScraper.Data.Models;

namespace Statesman.BillParser.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bill, UnparsedBillDto>();
    }
}
