using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeX.Models
{
    public class Business
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusinessId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string address { get; set; }= string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }=   string.Empty;
        [Required] 
        public string BankName { get; set; } = string.Empty;
        [Required] 
        public string Accountno { get; set; } = string.Empty;
        [Required]
        public string OwnerName{ get; set; }=string.Empty;
        [Required]
        public string OwnerNIC { get; set; }=string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public ICollection<Facility> Facilities { get; set; }
    }
}
