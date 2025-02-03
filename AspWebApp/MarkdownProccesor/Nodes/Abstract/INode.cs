

using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Nodes.Abstract;
public interface INode
{
    string? Represent();
    ITag Tag { get; }
}

