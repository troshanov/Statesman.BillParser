using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Statesman.BillParser.Shared.Models;
using Statesman.BillScraper.Data.Repositories.Interfaces;

namespace Statesman.BillParser.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public BillsController(
            IBillRepository billRepository,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("unparsed")]
        public async Task<IActionResult> GetUnparsedBills()
        {
            var result = await _billRepository.GetUnparsedBillsAsync();

            return Ok(_mapper.Map<IEnumerable<BillDto>>(result));
        }
    }
}
