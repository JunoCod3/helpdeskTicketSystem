using System.ComponentModel.DataAnnotations;

namespace FullstackDevTS.Models.Enums;

public enum Status
{
    [Display(Name = "Active")]
    ACTIVE,
    [Display(Name = "Inactive")]
    INACTIVE
}