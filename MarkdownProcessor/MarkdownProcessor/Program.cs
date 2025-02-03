namespace MarkdownProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var mdProcessor = new MarkdownProcessor();
        Console.WriteLine(mdProcessor.ConvertToHtml("Заголовок __с _разными_ символами__"));
    }
}