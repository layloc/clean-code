using System.Text;
using MarkdownProcessor.Abstractions;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor;

public class MarkdownPc : IMarkdownProcessor
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

        for (int i = 0; i < markdownText.Length; i++)
        {
            char ch = markdownText[i];
            bool isLastChar = i == markdownText.Length - 1;

            // Правило: Если символ "#" перед пробелом, начинается заголовок.
            if (previousChar == '#' && ch == ' ')
            {
                outputString.Append(_markdownTags["#"].HtmlTag.Item1);
                previousChar = ch;
                currentTag.Clear();
                continue;
            }

            // Пропустить экранированные символы
            if (previousChar == '\\')
            {
                outputString.Append(ch);
                previousChar = '*';
                continue;
            }

            // Если не триггерный символ, сбрасываем флаг
            if (!_triggerSymbols.Contains(ch))
            {
                doubleFlag = false;

                // Если текущий тег валиден, добавляем HTML-тег
                if (_markdownTags.ContainsKey(currentTag.ToString()))
                {
                    AddTagToAnswer(currentTag, outputString);
                    currentTag.Clear();
                }

                outputString.Append(ch);
                previousChar = ch;
                continue;
            }

            // Обработка двойных подчерков "__" или одинарных "_"
            if (ch == '_')
            {
                bool isDoubleUnderscore = previousChar == '_';

                // Условие: Проверка на непробельные символы до и после
                if (isDoubleUnderscore && !isLastChar && !char.IsWhiteSpace(markdownText[i + 1]) && currentTag.Length == 1)
                {
                    currentTag.Append('_');
                    doubleFlag = false;
                }
                else if (!isDoubleUnderscore && !char.IsWhiteSpace(previousChar) && !isLastChar && !char.IsWhiteSpace(markdownText[i + 1]))
                {
                    currentTag.Append('_');
                    doubleFlag = true;
                }
                else
                {
                    // Если правила не выполнены, считаем символ обычным подчерком
                    outputString.Append('_');
                    currentTag.Clear();
                    doubleFlag = false;
                }

                previousChar = ch;
                continue;
            }

            // Завершаем текущий тег, если двойной флаг активирован и следующий символ не триггерный
            if (doubleFlag && !_triggerSymbols.Contains(ch))
            {
                AddTagToAnswer(currentTag, outputString);
                currentTag.Clear();
                outputString.Append(ch);
                previousChar = '*';
                continue;
            }

            // Если триггерный символ повторяется
            if (_triggerSymbols.Contains(ch) && previousChar == ch)
            {
                // Условие: Игнорируем пересечение одинарных и двойных подчерков
                if (currentTag.ToString() == "_")
                {
                    outputString.Append("__");
                    currentTag.Clear();
                    previousChar = '_';
                    continue;
                }

                currentTag.Append(ch);
                AddTagToAnswer(currentTag, outputString);
                currentTag.Clear();
                previousChar = ' ';
                doubleFlag = false;
                continue;
            }

            // Обычная обработка триггерного символа
            if (_triggerSymbols.Contains(ch))
            {
                currentTag.Append(ch);
                previousChar = ch;
                doubleFlag = true;
            }
        }

        // Завершаем оставшийся открытый тег
        if (currentTag.Length > 0 && _markdownTags.ContainsKey(currentTag.ToString()))
        {
            outputString.Append(_markdownTags[currentTag.ToString()].HtmlTag.Item2);
        }

        return outputString.ToString();
    }
}