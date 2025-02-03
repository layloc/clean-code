using MarkdownProcessor.Abstractions;

namespace MarkdownProcessor.Tags;

public class Header : Tag
{
    public override string MarkdownEquivalent { get; private protected set; } = "# ";
    public override (string, string) HtmlTag { get; private protected set; } = ("<h1>", "</h1>");
    public override char ClosingSymbol { get; private protected set; } = 'n';
}