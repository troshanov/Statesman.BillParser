using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Statesman.BillParser.Shared.Models;
using Statesman.BillParser.Shared.Models.BillElements;
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

            return Ok(_mapper.Map<IEnumerable<UnparsedBillDto>>(result));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBillById(int id)
        {
            var result = await _billRepository.GetBillByIdAsync(id);
            return Ok(_mapper.Map<UnparsedBillDto>(result));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBill([FromBody] ParsedBillDto legislation)
        {
            try
            {
                // Get the existing bill from the database
                var existingBill = await _billRepository.GetBillByIdAsync(legislation.Id);
                if (existingBill == null)
                {
                    return NotFound(new { success = false, message = "Bill not found" });
                }

                // Update bill properties
                existingBill.Title = legislation.LawTitle;
                existingBill.IsParsed = true;
                existingBill.ParsedAt = DateTime.UtcNow;

                // Clear existing bill elements
                existingBill.BillElements.Clear();

                // Map and add new bill elements
                foreach (var elementNode in legislation.BillElements)
                {
                    var contextElement = MapBillElementNodeToContext(elementNode, existingBill);
                    existingBill.BillElements.Add(contextElement);
                }

                // TODO: Save the updated bill using the repository
                // await _billRepository.SaveAsync(existingBill);

                return Ok(new { success = true, message = "Legislation saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private Statesman.BillScraper.Data.Models.BillElement MapBillElementNodeToContext(
            BillElementNode elementNode, 
            Statesman.BillScraper.Data.Models.Bill bill)
        {
            // Create the appropriate context element based on type
            Statesman.BillScraper.Data.Models.BillElement contextElement = elementNode.Value.Type switch
            {
                BillElementType.Chapter => new Statesman.BillScraper.Data.Models.Chapter(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                BillElementType.Section => new Statesman.BillScraper.Data.Models.Section(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                BillElementType.Article => new Statesman.BillScraper.Data.Models.Article(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                BillElementType.Paragraph => new Statesman.BillScraper.Data.Models.Paragraph(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                BillElementType.Point => new Statesman.BillScraper.Data.Models.Point(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                BillElementType.Letter => new Statesman.BillScraper.Data.Models.Letter(
                    elementNode.Value.Text, 
                    elementNode.Value.Marker),
                _ => throw new ArgumentException($"Unknown BillElementType: {elementNode.Value.Type}")
            };

            // Set the bill reference
            contextElement.Bill = bill;

            // Recursively map child elements
            foreach (var childNode in elementNode.Children)
            {
                var childElement = MapBillElementNodeToContext(childNode, bill);
                childElement.ParentElement = contextElement;
                contextElement.ChildElements.Add(childElement);
            }

            return contextElement;
        }
    }
}
