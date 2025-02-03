

using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;
internal class ItalicNode : CompositeNode
{
    public override NodeType TypeOfNode => NodeType.Italic;
    public override ITag Tag => new ItalicTag();
    public ItalicNode(CompositeNode parent) : base(parent) { }
    
}
