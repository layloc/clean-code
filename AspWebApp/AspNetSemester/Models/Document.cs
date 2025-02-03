using System.ComponentModel.DataAnnotations;

namespace AspNetSemester.Models;

public class Document
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public string Url { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    public ICollection<DocumentAccess> DocumentAccesses { get; set; } = new List<DocumentAccess>();
}