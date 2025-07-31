namespace Statesman.BillParser.Shared.Models.BillElements;

public class BillElement
{
    public required BillElementType Type { get; set; }
    public required string Text { get; set; } = string.Empty;
    public required string Marker { get; set; } = string.Empty;
}
