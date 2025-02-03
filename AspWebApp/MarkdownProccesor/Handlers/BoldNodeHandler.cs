
using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Nodes.Factories;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Handlers;
public class BoldNodeHandler : ItalicWithBoldTagsHandler
{
    public override NodeType TagType => NodeType.Bold;
    public override CompositeNodeFactory Factory => new BoldNodeFactory();
    public override ITag Tag => new BoldTag();
    public override NodeType OppositeTagType => NodeType.Italic;
    public override ITag OppositeTag => new ItalicTag();
}
