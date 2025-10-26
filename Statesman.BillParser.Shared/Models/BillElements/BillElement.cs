namespace Statesman.BillParser.Shared.Models.BillElements;

public class BillElement
{
    private string? _marker;

    public required BillElementType Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Marker 
    {
        get => _marker ??= Type switch
        {
            BillElementType.Letter => "a",
            _ => "1"
        };
        set => _marker = value;
    }
}
