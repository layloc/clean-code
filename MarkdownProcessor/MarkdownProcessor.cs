using System.Text;
using MarkdownProcessor.Abstractions;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor;

public class MarkdownProcessor : IMarkdownProcessor
{
    private Stack<string> _stack = new();
    private Stack<bool> _maslenitsya = new();
    private HashSet<char> _triggerSymbols = new HashSet<char> { '_', '#', '\n' };
    private void AddTagToAnswer(StringBuilder currentTag, StringBuilder outputString)
    {
        if (_stack.Count > 0 && (_stack.Peek() == _markdownTags[currentTag.ToString()].MarkdownEquivalent ||
                                 _stack.Peek() == _markdownTags[currentTag.ToString()].ClosingSymbol
                                     .ToString()))
        {
            _stack.Pop();
            outputString.Append(_markdownTags[currentTag.ToString()].HtmlTag.Item2);
        }
        else
        {
            _stack.Push(currentTag.ToString());
            outputString.Append(_markdownTags[currentTag.ToString()].HtmlTag.Item1);
        }
    }
    private Dictionary<string, Tag> _markdownTags = new Dictionary<string, Tag>()
    {
        { "_", new Emphasis() },
        { "#", new Header() },
        { "__", new Strong() },
        { "\\__", new Strong() },
        { "\\#", new Header() },
        { "\\_", new Emphasis() },
        { "\n", new Header() },
    };
    public void AddTag(Tuple<string, Tag> tagToAdd)
    {
        _markdownTags.Add(tagToAdd.Item1, tagToAdd.Item2);
    }
    public void AddSymbol(char symbolToAdd)
    {
        _triggerSymbols.Add(symbolToAdd);
    }
    public string ConvertToHtml(string markdownText)
    {
        StringBuilder outputString = new();
        StringBuilder currentTag = new();
        char previousChar = '*';
        bool doubleFlag = false;
        bool insideOfWord = false;
        foreach (var ch in markdownText)
        {
            if (previousChar == '#' && ch == ' ')
            {
                outputString.Append(_markdownTags["#"].HtmlTag.Item1);
                previousChar = ch;
                currentTag.Clear();
                continue;
            }
            if (previousChar == '\\' || char.IsDigit(previousChar))
            {
                outputString.Append(ch);
                previousChar = ch;
                continue;
            }
            if (ch == '\\')
            {
                previousChar = ch;
                continue;
            }
            if (!_triggerSymbols.Contains(ch))
            {
                doubleFlag = false;
                if (_markdownTags.ContainsKey(currentTag.ToString()))
                {
                    AddTagToAnswer(currentTag, outputString);
                    currentTag.Clear();
                }
                outputString.Append(ch);
            }
            if (doubleFlag && !(_triggerSymbols.Contains(ch)))
            {
                doubleFlag = false;
                AddTagToAnswer(currentTag, outputString);
                previousChar = '*';
                currentTag.Clear();
                outputString.Append(ch);
            }
            if (_triggerSymbols.Contains(ch) && previousChar == ch)
            {
                if (_stack.Contains("_") && currentTag.ToString() == "_")
                {
                    previousChar = '_';
                    currentTag.Clear();
                    outputString.Append("__");
                    continue;
                }
                currentTag.Append(ch);
                AddTagToAnswer(currentTag, outputString);
                previousChar = ' ';
                currentTag.Clear();
                doubleFlag = false;
                continue;
            }
            
            if (_triggerSymbols.Contains(ch)) 
            {
                currentTag.Append(ch);
                previousChar = ch;
                doubleFlag = true;
            }
            previousChar = ch;
        }

        if (currentTag.Length > 0)
        {
            outputString.Append(_markdownTags[currentTag.ToString()].HtmlTag.Item2);
        }
        return outputString.ToString();
    }
}