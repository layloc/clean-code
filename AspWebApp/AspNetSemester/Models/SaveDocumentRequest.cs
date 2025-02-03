namespace AspNetSemester.Models;

public class SaveDocumentRequest
{
    public string Text { get; set; }
    public string? FileName { get; set; }
    public int DocumentId { get; set; }
}