

using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;
public class ImageNode : INode
{
    public ITag Tag => new ImageTag();
    private string? _source;
    private string? _alternative;
    public string? Represent()
    {
        return $"<{Tag.HtmlTag} src=\"{_source}\" alt=\"{_alternative}\"/>";
    }
    public ImageNode(string? source, string? alternative)
    {
        _source = source;
        _alternative = alternative;
    }
}
