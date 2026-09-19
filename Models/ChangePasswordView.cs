using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Models;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }
}