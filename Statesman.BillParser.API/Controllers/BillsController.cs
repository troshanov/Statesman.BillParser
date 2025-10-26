using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Statesman.BillParser.Shared.Models;
using Statesman.BillParser.Shared.Models.BillElements;
using Statesman.BillScraper.Data.Models;
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
        public async Task<IActionResult> UpdateBill([FromBody] ParsedBillDto parsedBill)
        {
            try
            {
                // Get the existing bill from the database
                var billEntity = await _billRepository.GetBillByIdAsync(parsedBill.Id);
                if (billEntity == null)
                {
                    return NotFound(new { success = false, message = "Bill not found" });
                }

                // Add new bill elements
                foreach (var elementNode in parsedBill.BillElements)
                {
                    var contextElement = AddBillElementsToDatabase(elementNode, billEntity);
                }

                // Update bill properties
                billEntity.IsParsed = true;
                billEntity.ParsedAt = DateTime.UtcNow;

                // TODO: Save the updated bill using the repository
                // await _billRepository.SaveAsync(existingBill);

                return Ok(new { success = true, message = "Legislation saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private BillElementEntity AddBillElementsToDatabase(
            BillElementNode elementNode,
            BillEntity? bill = null)
        {
            // Create the appropriate context element based on type
            BillElementEntity contextElement = elementNode.Value.Type switch
            {
                BillElementType.Chapter => new ChapterEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),
                BillElementType.Section => new SectionEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),
                BillElementType.Article => new ArticleEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),
                BillElementType.Paragraph => new ParagraphEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),
                BillElementType.Point => new PointEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),
                BillElementType.Letter => new LetterEntity(
                    elementNode.Value.Text,
                    elementNode.Value.Marker),

                _ => throw new ArgumentException($"Unknown BillElementType: {elementNode.Value.Type}")
            };

            // Set the bill reference
            if (bill != null)
            {
                contextElement.Bill = bill;
            }

            // Recursively map child elements
            foreach (var childNode in elementNode.Children)
            {
                var childElement = AddBillElementsToDatabase(childNode);
                childElement.ParentElement = contextElement;
                contextElement.ChildElements.Add(childElement);
            }

            return contextElement;
        }
    }
}
