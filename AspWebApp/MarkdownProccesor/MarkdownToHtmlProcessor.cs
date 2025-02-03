using MarkdownProccesor.Handlers;
using MarkdownProccesor.Handlers.Abstract;

namespace MarkdownProccesor;

public sealed class MarkdownToHtmlProcessor
{
    private readonly IHandler _configuredHandlers;
    public string ConvertToHtml(string markdownText)
    {
        var textSplitByLines = markdownText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var mdTextHandler = new MarkdownTextHandler(_configuredHandlers);
        
        return mdTextHandler.HandleMdTextLines(textSplitByLines);
    }
    public MarkdownToHtmlProcessor()
    {
        _configuredHandlers = ConfigureHandlers();
    }
    private IHandler ConfigureHandlers()
    {
        var textHandler = new TextNodeHandler();
        var boldHandler = new BoldNodeHandler();
        var italicHandler = new ItalicNodeHandler();
        var headerHandler = new HeaderNodeHandler();
        var handwritingHandler = new AllHandwritingHandler();
        var imageHandler = new ImageNodeHandler();
        headerHandler.Successor = handwritingHandler;
        handwritingHandler.Successor = textHandler;
        textHandler.Successor = imageHandler;
        imageHandler.Successor = boldHandler;
        boldHandler.Successor = italicHandler;
        italicHandler.Successor = textHandler;

        return headerHandler;
    }
}
