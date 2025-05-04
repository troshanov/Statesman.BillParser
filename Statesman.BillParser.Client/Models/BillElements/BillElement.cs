namespace Statesman.BillParser.Client.Models.BillElements;

public class BillElement
{
    public required BillElementType Type { get; set; }
    public required string Content { get; set; } = string.Empty;
    public required string Marker { get; set; } = string.Empty;
}
