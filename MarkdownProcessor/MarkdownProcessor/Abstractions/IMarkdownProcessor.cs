namespace MarkdownProcessor.Abstractions;

public interface IMarkdownProcessor
{
    public string ConvertToHtml(string markdownText);
}