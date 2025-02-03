
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes.Abstract;

namespace MarkdownProccesor.Handlers.Abstract;
public interface IHandler
{
    IHandler Successor { get; set; }
    CompositeNode HandleWord(ProcessedWord word, CompositeNode currentNode);
}