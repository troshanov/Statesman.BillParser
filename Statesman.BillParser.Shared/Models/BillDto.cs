namespace Statesman.BillParser.Shared.Models;

public class BillDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string RawText { get; set; } = null!;
    public DateTime Date { get; set; }
}
