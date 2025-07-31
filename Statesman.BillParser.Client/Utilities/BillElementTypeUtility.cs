using Statesman.BillParser.Shared.Models.BillElements;
namespace Statesman.BillParser.Client.Utilities;
public static class BillElementTypeUtility
{
    public static Dictionary<BillElementType, List<BillElementType>> DropDownAllowedTypes => new Dictionary<BillElementType, List<BillElementType>>
    {
        { BillElementType.Chapter, new List<BillElementType> { BillElementType.Chapter, BillElementType.Section, BillElementType.Article, BillElementType.Paragraph, BillElementType.Point, BillElementType.Letter } },
        { BillElementType.Section, new List<BillElementType> { BillElementType.Section, BillElementType.Article, BillElementType.Paragraph, BillElementType.Point, BillElementType.Letter } },
        { BillElementType.Article, new List<BillElementType> { BillElementType.Article, BillElementType.Paragraph, BillElementType.Point, BillElementType.Letter } },
        { BillElementType.Paragraph, new List<BillElementType> { BillElementType.Paragraph, BillElementType.Point, BillElementType.Letter } },
        { BillElementType.Point, new List<BillElementType> { BillElementType.Point, BillElementType.Letter } },
    };

    public static Dictionary<BillElementType, BillElementType> DefaultTypes => new Dictionary<BillElementType, BillElementType>
    {
        { BillElementType.Chapter,  BillElementType.Section},
        { BillElementType.Section,  BillElementType.Article},
        { BillElementType.Article,  BillElementType.Paragraph},
        { BillElementType.Paragraph,  BillElementType.Point},
        { BillElementType.Point,  BillElementType.Letter},
    };
}