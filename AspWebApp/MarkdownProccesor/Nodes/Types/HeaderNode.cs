using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;

public sealed class HeaderNode : CompositeNode
{
    private uint _levelOfHeader;
    public override NodeType TypeOfNode => NodeType.Header;
    public override ITag Tag => new HeaderTag();
    public override string Represent() => $"<{Tag.HtmlTag}{_levelOfHeader}>{RepresentAllChildrenNodes()}</{Tag.HtmlTag}{_levelOfHeader}>";
    public HeaderNode(CompositeNode parent, uint level): base(parent)
    {
        _levelOfHeader = level;
    }
}
