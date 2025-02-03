using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Handlers.Tools;
public static class HandleNodesHelper
{
    public static CompositeNode HandleEndWordIfNoClosingTag(CompositeNode node, string text)
    {
        node = HandleNoneClosingTag(node);
        node.Add(new TextNode(text+' '));
        return node;
    }
    public static CompositeNode? GetParentWithReplacedToTextCurrentNode(CompositeNode? node, ITag? leftTag, ITag? rightTag)
    {
        if (node == null) return null;
        var textNode = TextNode.DecorateCurrentNodeToTextNode(node, leftTag, rightTag);
        node.Parent.ReplaceNode(node, textNode);
        return node.Parent;
    }
    public static CompositeNode CompleteAllCreatedOpeningNodes(CompositeNode node, params NodeType[] types)
    {
        while (types.Any((type) => type == node.TypeOfNode))
        {
            node = GetParentWithReplacedToTextCurrentNode(node, node.Tag, null);
        }
        return node;
    }
    private static CompositeNode HandleNoneClosingTag(CompositeNode node)
    {
        if (!node.IsBeginInWord) return node;
        if (node.TypeOfNode == NodeType.Italic)
        {
            return HandleNoneClosingTag(GetParentWithReplacedToTextCurrentNode(node, new ItalicTag(), null));;
        }
        else if (node.TypeOfNode == NodeType.Bold)
        {
            return HandleNoneClosingTag(GetParentWithReplacedToTextCurrentNode(node, new BoldTag(), null));
        }
        return node;
    }
    
}
