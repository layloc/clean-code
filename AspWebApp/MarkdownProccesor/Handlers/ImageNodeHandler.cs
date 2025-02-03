using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Tags;
using System.Text;

namespace MarkdownProccesor.Handlers;
public class ImageNodeHandler : IHandler
{
    private ImageTag _tag = new ImageTag();
    public IHandler Successor { get; set; }

    public CompositeNode HandleWord(ProcessedWord word, CompositeNode currentNode)
    {
        if (word.Current != _tag.MdTag) return Successor.HandleWord(word, currentNode);
        word.AddCurrentIndexValue();
        string alt = string.Empty;
        string src = string.Empty;
        if (word.Current == "[")
        {
            word.AddCurrentIndexValue();
            alt = GetAttribute(word, "]");
        }
        if (word.Current == "(")
        {
            word.AddCurrentIndexValue();
            src = GetAttribute(word, ")");
        }
        currentNode.Add(new ImageNode(src, alt));
        return Successor.HandleWord(word,currentNode);
        
    }
    private string GetAttribute(ProcessedWord word, string symbol)
    {
        var attribute = new StringBuilder();
        while (word.Current != symbol)
        {
            attribute.Append(word.Current);
            word.AddCurrentIndexValue();
            if (word.IsProcessed) return string.Empty;
        }
        word.AddCurrentIndexValue();
        return attribute.ToString();
    }
}
