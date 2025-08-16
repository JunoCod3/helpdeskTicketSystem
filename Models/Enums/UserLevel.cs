using System.ComponentModel.DataAnnotations;

namespace FullstackDevTS.Models.Enums;

public enum UserLevel
{
    [Display(Name = "Admin")]
    ADMIN,
    
    [Display(Name = "User")]
    USER
}
