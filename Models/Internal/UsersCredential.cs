using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullstackDevTS.Models.Internal;

[Table("users_credential")]
public class UsersCredential
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public required string Username { get; set; }
    
    
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public required string Password { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
}