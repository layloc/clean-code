using MarkdownProccesor.Tags.Abstract;


namespace MarkdownProccesor.Tags;
public class BoldTag : ITag
{
    public string HtmlTag => "strong";

    public string MdTag => "__";

    public int AtomicLength => 2;
}