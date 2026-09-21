using Microsoft.AspNetCore.Identity;

namespace ProductManagementSystem.Models
{
    public class User : IdentityUser
    {
        public string? ProfilePhotoUrl { get; set; }
    }
}