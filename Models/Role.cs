using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeX.Models
{
    public class Role: IdentityRole<int>
    {
        [Required]
        public ICollection<IdentityUser> Users { get; set; } = new List<IdentityUser>();
    }
}
