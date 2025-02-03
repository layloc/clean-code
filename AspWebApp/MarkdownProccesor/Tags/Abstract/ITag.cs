

namespace MarkdownProccesor.Tags.Abstract;

public interface ITag
{
    string HtmlTag { get; }
    string MdTag { get; }
    int AtomicLength { get; }
}