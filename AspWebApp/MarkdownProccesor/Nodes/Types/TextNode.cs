using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;

public sealed class TextNode : INode
{
    private readonly string _text;
    public string? Represent() => _text;
    public ITag Tag => new TextTag();
    public TextNode(string text)
    {
        _text = text;
    }
    public static TextNode DecorateCurrentNodeToTextNode(CompositeNode node, ITag? leftTag, ITag? rightTag)
    {
        var without = node.RepresentWithoutTags();
        return new TextNode((leftTag?.MdTag ?? "") + without + (rightTag?.MdTag ?? ""));
    }
    
}
