

using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Tags;
public class HeaderTag : ITag
{
    public string HtmlTag => "h";

    public string MdTag => "#";

    public int AtomicLength => 0;
}
