

using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Handlers.Tools;
namespace MarkdownProccesor.Handlers;

public class MarkdownTextHandler
{
    private readonly IHandler _wordHandler;
    private readonly CompositeNode _root;
    private CompositeNode _currentNode;

    public MarkdownTextHandler(IHandler handler)
    {
        _root = new DocumentNode();
        _currentNode = _root;
        _wordHandler = handler;
    }
    public string HandleMdTextLines(string[] mdText)
    {
        foreach(var line in mdText)
        {
            var textSplitByWords = line.Split(new char[] { '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            HandleLine(textSplitByWords);
        }
        if (mdText.Length == 1)
        {
            _currentNode = HandleNodesHelper.CompleteAllCreatedOpeningNodes(_currentNode, NodeType.Italic, NodeType.Bold);
        }
        return _root.Represent();
    }
    private void HandleLine(string[] line)
    {
        if (line.Length == 0)
        {
            _currentNode = HandleNodesHelper.CompleteAllCreatedOpeningNodes(_currentNode, NodeType.Italic, NodeType.Bold);
            _currentNode.Add(new TextNode("</br>"));
        }
        for (int i = 0; i < line.Length; i++)
        {
            var word = new ProcessedWord(line[i]);
            if (i == 0) word.IsFirst = true;
            _currentNode = _wordHandler.HandleWord(word, _currentNode);
        }
    }
    
}
