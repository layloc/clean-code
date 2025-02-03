

using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Tags;
public class ImageTag : ITag
{
    public string HtmlTag => "img";

    public string MdTag => "!";

    public int AtomicLength => 1;
}
