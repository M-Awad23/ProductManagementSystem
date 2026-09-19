using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Models
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}