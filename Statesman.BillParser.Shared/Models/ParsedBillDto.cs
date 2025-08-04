using Statesman.BillParser.Shared.Models.BillElements;

namespace Statesman.BillParser.Shared.Models;

public class ParsedBillDto
{
    public int Id { get; set; }
    public string LawTitle { get; set; } = null!;
    public string? LegalBasis { get; set; }
    public string? Promulgation { get; set; }
    public string? Motives { get; set; }
    public string? Assessments { get; set; }
    public List<BillElementNode> BillElements { get; set; } = new();
} 