


using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Tags;
public class DocumentTag : ITag
{
    public string HtmlTag => "html";

    public string MdTag => "";

    public int AtomicLength => 0;
}
