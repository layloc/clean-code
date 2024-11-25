namespace MarkdownProcessor.Abstractions;

public abstract class Tag
{
    public abstract string MarkdownEquivalent { get; private protected set; }
    public abstract (string, string) HtmlTag { get; private protected set; }
    public abstract char ClosingSymbol { get; private protected set; }
}