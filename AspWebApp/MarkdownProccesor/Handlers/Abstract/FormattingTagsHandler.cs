using MarkdownProccesor.Handlers.Abstract;
using MarkdownProccesor.Handlers.Tools;
using MarkdownProccesor.ProcessedObjects;
using MarkdownProccesor.Nodes;
using MarkdownProccesor.Nodes.Abstract;
using MarkdownProccesor.Nodes.Types;
using MarkdownProccesor.Tags.Abstract;

public abstract class FormattingTagsHandler : IHandler
{ 
    protected ProcessedWord _word;
    protected CompositeNode _currentNode;
    public IHandler Successor { get ; set; }
    public abstract NodeType TagType { get; }
    public abstract CompositeNodeFactory Factory { get; }
    public abstract ITag Tag { get; }
    public CompositeNode HandleWord(ProcessedWord word, CompositeNode currentNode)
    {
        word.ContextNode = TagType;
        if (word.Current != Tag.MdTag) return Successor.HandleWord(word, currentNode);
        _word = word;
        _currentNode = currentNode;
        if (_word.IsBeginning)
        {
            _currentNode = HandleBeginWord();
        }
        else if (_word.IsEnd)
        {
            _currentNode = HandleEndWord();
        }
        else
        {
            _currentNode = HandleInsideWord();
        }
        return _currentNode;
    }
    public virtual CompositeNode HandleBeginWord()
    {
        if (char.IsDigit(_word.Next))
        {
            _currentNode.Add(new TextNode(Tag.MdTag));
            _word.AddCurrentIndexValue();
            return Successor.HandleWord(_word, _currentNode);
        }
        _word.AddCurrentIndexValue();          
        if (IsThereSameNodeParent())
        {
            _currentNode.Add(new TextNode(Tag.MdTag));
            return Successor.HandleWord(_word, _currentNode);
        }
        var node = Factory.CreateCompositeNode(_currentNode);
        return Successor.HandleWord(_word, node);
    }
    public abstract CompositeNode HandleEndWord();
    public abstract CompositeNode HandleInsideWord();   
    protected bool IsThereSameNodeParent()
    {
        var tempNode = _currentNode;
        while(tempNode != null)
        {
            if (tempNode.TypeOfNode == TagType) return true;
            tempNode = tempNode.Parent;
        }
        return false;
    }
    
    protected CompositeNode GetCreatedInWordNewNode()
    {
        var node = Factory.CreateCompositeNode(_currentNode);
        node.IsBeginInWord = true;
        return Successor.HandleWord(_word, node);
    }
}
