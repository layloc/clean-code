using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Tags;
public class ItalicTag : ITag
{
    public string HtmlTag => "em";

    public string MdTag => "_";

    public int AtomicLength => 1;
}
