using MarkdownProccesor.Tags.Abstract;


namespace MarkdownProccesor.Tags;
public class TextTag : ITag
{
    public string HtmlTag => "";

    public string MdTag => "";

    public int AtomicLength => 1;
}
