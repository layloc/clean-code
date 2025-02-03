using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetSemester.Models;

public class DocumentAccess
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int DocumentId { get; set; }

    [ForeignKey("DocumentId")]
    public Document Document { get; set; }
    
    public bool IsReadOnly { get; set; }
}