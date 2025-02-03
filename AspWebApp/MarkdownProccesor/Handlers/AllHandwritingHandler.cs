
using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;

namespace MarkdownProccesor.Handlers;
public class AllHandwritingHandler : IHandler
{
    public IHandler Successor { get ; set ; }

    public CompositeNode HandleWord(ProcessedWord word, CompositeNode currentNode)
    {
        if (word.Value.All((elem) => elem == '_'))
        {
            currentNode.Add(new TextNode(word.Value));
            return currentNode;
        }
        return Successor.HandleWord(word, currentNode);
    }
}