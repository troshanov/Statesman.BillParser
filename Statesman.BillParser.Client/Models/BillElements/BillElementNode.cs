namespace Statesman.BillParser.Client.Models.BillElements;

public class BillElementNode
{
    public BillElementNode(BillElement element)
    {
        Value = element;
    }

    public BillElement Value { get; set; } = null!;
    public BillElementNode? Parent { get; private set; }
    public List<BillElementNode> Children { get; set; } = new();

    public BillElementNode AddChild(BillElement element)
    {
        var child = new BillElementNode(element)
        {
            Parent = this
        };

        Children.Add(child);
        SortChildrenByMarker();

        return child;
    }

    public void RemoveChild(BillElementNode node)
    {
        if (Children.Contains(node))
        {
            Children.Remove(node);
            node.Parent = null;
        }

        RenumberChildMarkers();
    }

    private void RenumberChildMarkers()
    {
        var useLetters = false;
        if (Children.Count > 0)
            useLetters = Children[0].Value.Type == BillElementType.Letter;

        for (int i = 0; i < Children.Count; i++)
        {
            if (useLetters)
            {
                char letter = (char)('a' + i);
                Children[i].Value.Marker = letter.ToString();
            }
            else
            {
                Children[i].Value.Marker = (i + 1).ToString();
            }
        }
    }

    private void SortChildrenByMarker() 
        => Children.Sort((a, b) => string.Compare(a.Value.Marker, b.Value.Marker, StringComparison.Ordinal));
}
