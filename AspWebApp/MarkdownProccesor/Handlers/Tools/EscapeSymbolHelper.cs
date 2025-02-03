
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes;
using System.Text;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Handlers.Tools;
public static class EscapeSymbolHelper
{
    public static string HandleEscapeSymbols(ProcessedWord word)
    {
        bool isEscape = true;
        var text = new StringBuilder();
        string specialSymbol = string.Empty;
        while (word.Current == "\\")
        {
            if (isEscape)
            {
                text.Append("\\");
                isEscape = false;
            }
            else
            {
                isEscape = true;
            }
            word.AddCurrentIndexValue();
        }
        if (text.Length != 0 && IsEscapeSymbolStandsBeforeSpecial(ref specialSymbol, word,
            isEscape,
            (NodeType.Bold, new BoldTag()),
            (NodeType.Italic, new ItalicTag())
            ))
        {
            text.Remove(text.Length-1,1);
            text.Append(specialSymbol);
        }
        return text.ToString();
    }
    private static bool IsEscapeSymbolStandsBeforeSpecial(ref string special,
        ProcessedWord word, 
        bool isEscape,
        params (NodeType nodeType, ITag tagType)[] specialSymbols)
    {
        if (isEscape) return false; 
        bool result = false;
        foreach(var type in specialSymbols)
        {
            word.ContextNode = type.nodeType;
            result = word.Current == type.tagType.MdTag;
            if (result)
            {
                special = word.Current;
                word.AddCurrentIndexValue();
                break;
            }
        }
        word.ContextNode = NodeType.Text;
        return result;
    }
}
