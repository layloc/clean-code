using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Nodes.Factories;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Handlers;

public class ItalicNodeHandler : ItalicWithBoldTagsHandler
{
    public override NodeType TagType => NodeType.Italic;
    public override CompositeNodeFactory Factory => new ItalicNodeFactory();
    public override ITag Tag => new ItalicTag();
    public override NodeType OppositeTagType => NodeType.Bold;
    public override ITag OppositeTag => new BoldTag();
}
