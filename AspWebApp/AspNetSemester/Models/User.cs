using System.ComponentModel.DataAnnotations;

namespace AspNetSemester.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public ICollection<DocumentAccess> DocumentAccesses { get; set; } = new List<DocumentAccess>();

}