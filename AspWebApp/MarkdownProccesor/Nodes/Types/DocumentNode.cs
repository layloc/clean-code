
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Types;
public class DocumentNode : CompositeNode
{
    public override NodeType TypeOfNode => NodeType.Document;
    public override ITag Tag => new DocumentTag();
}
