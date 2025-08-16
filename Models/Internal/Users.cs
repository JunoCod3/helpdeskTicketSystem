using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace FullstackDevTS.Models.Internal;

[Table("users")]
public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("Credential")]
    public required Guid CredentialId { get; set; }
    
    [Required]
    [ForeignKey("Information")]
    public required Guid InformationId { get; set; }
    
    public virtual UsersCredential Credential { get; set; }
    public virtual UsersInformation Information { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
}