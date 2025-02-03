using MarkdownProcessor.Abstractions;

namespace MarkdownProcessor.Tags;

public class Strong : Tag
{
    public override string MarkdownEquivalent { get; private protected set; } = "__";
    public override (string, string) HtmlTag { get; private protected set; } = ("<strong>", "</strong>");
    public override char ClosingSymbol { get; private protected set; } = 'p';
}