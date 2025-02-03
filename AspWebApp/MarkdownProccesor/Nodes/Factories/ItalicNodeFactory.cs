

using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;

namespace MarkdownProccesor.Nodes.Factories;
public class ItalicNodeFactory : CompositeNodeFactory
{
    public override CompositeNode CreateCompositeNode(CompositeNode parent)
    {
        return new ItalicNode(parent);
    }
}
