using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstBLogicProject.Models.Entity
{

    [Table(nameof(AdvisorItem))]
    public class AdvisorItem
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^([^0 - 9]*)$", ErrorMessage = "Characters are not allowed.")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^([^0 - 9]*)$", ErrorMessage = "Characters are not allowed.")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string IdentificationNumber { get; set; }
        [Required]
        [Range(18, 150)]
        public byte Age { get; set; }
    }
}
