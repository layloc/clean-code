
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;

public class BoldNode : CompositeNode
{
    public override NodeType TypeOfNode => NodeType.Bold;
    public override ITag Tag => new BoldTag();
    public BoldNode(CompositeNode parent) : base(parent) { }
    public override string? Represent()
    {
        if (Parent.TypeOfNode == NodeType.Italic && Parent.IsFinished) return RepresentWithMdTag();
        return base.Represent();
    }
    private string RepresentWithMdTag()
    {
        return Tag.MdTag + RepresentAllChildrenNodes() + Tag.MdTag;
    }

}