using MarkdownProccesor.Handlers.Tools;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.Tags;
using MarkdownProccesor.Tags.Abstract;

namespace MarkdownProccesor.Handlers.Abstract;
public abstract class ItalicWithBoldTagsHandler: FormattingTagsHandler
{
    public abstract NodeType OppositeTagType { get; }
    public abstract ITag OppositeTag { get; }
    public override CompositeNode HandleEndWord()
    {
        if (char.IsDigit(_word.Previous)) 
        {
            return HandleNodesHelper.HandleEndWordIfNoClosingTag(_currentNode, Tag.MdTag);
        }

        if (_currentNode.TypeOfNode == TagType)
        {
            _currentNode = HandleEndWordIfTagType();
        }
        else if (_currentNode.TypeOfNode == OppositeTagType)
        {
            _currentNode = HandleEndNodeIfOppositeTag();
        }
        else
        {
            HandleIntersectTags();
            _currentNode.Add(new TextNode(Tag.MdTag));
        }
        _currentNode.Add(new TextNode(" "));
        return _currentNode;
    }
    public override CompositeNode HandleInsideWord()
    {
        if (char.IsDigit(_word.Next) || char.IsDigit(_word.Previous))
        {
            _currentNode.Add(new TextNode(Tag.MdTag));
            _word.AddCurrentIndexValue();
            return Successor.HandleWord(_word, _currentNode);
        }
        _word.AddCurrentIndexValue();
        if (_currentNode.TypeOfNode == TagType)
        {
            _currentNode = HandleInsideWordIfTagType();
        }
        else if (_currentNode.TypeOfNode == OppositeTagType && _currentNode.IsBeginInWord)
        {
            _currentNode = Successor.HandleWord(_word, HandleEndNodeIfOppositeTag());
        }
        else
        {
            _currentNode = GetCreatedInWordNewNode();
        }
        return _currentNode;
    }
    public virtual CompositeNode HandleEndWordIfTagType()
    {
        return _currentNode.Parent!;
    }
    public virtual CompositeNode HandleEndNodeIfOppositeTag()
    {
        var sameNodeParent = _currentNode.Parent;
        if (sameNodeParent.TypeOfNode == TagType)
        {
            sameNodeParent.IsContainOppositeTag = true;
            return HandleNodesHelper.GetParentWithReplacedToTextCurrentNode(_currentNode, OppositeTag, null).Parent;
        }            
        return HandleNodesHelper.GetParentWithReplacedToTextCurrentNode(_currentNode, OppositeTag, Tag);
    }
    public virtual CompositeNode HandleInsideWordIfTagType()
    {
        return Successor.HandleWord(_word, _currentNode.Parent!);
    }
    protected void HandleIntersectTags()
    {
        var lastCompositeNode = _currentNode.GetLastCompositeNode();
        if (lastCompositeNode == null) return;
        if (lastCompositeNode.TypeOfNode == OppositeTagType && lastCompositeNode.IsContainOppositeTag)
        {
            _currentNode.ReplaceNode(lastCompositeNode,
            TextNode.DecorateCurrentNodeToTextNode(lastCompositeNode, OppositeTag, OppositeTag));
        }
    }

}
