using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FullstackDevTS.Models.Enums;

namespace FullstackDevTS.Models.Internal;

[Table("users_information")]
public class UsersInformation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Firstname { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Middlename { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Lastname { get; set; }
    
    [Required]
    public UserLevel UserLevel { get; set; } 
    
    [Required]
    public Status Status { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
}

