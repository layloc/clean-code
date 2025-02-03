using MarkdownProcessor.Abstractions;

namespace MarkdownProcessor.Tags;

public class Emphasis : Tag
{
    public override string MarkdownEquivalent { get; private protected set; } = "_";
    public override (string, string) HtmlTag { get; private protected set; } = ("<em>", "</em>");
    public override char ClosingSymbol { get; private protected set; } = 'f';
}