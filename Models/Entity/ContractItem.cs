using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBLogicProject.Models.Entity
{
    [Table(nameof(ContractItem))]
    public class ContractItem
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string InstitutionName { get; set; }
        [Required]
        [RegularExpression(@"^(\D* \w+)$", ErrorMessage = "Characters are not allowed.")]
        public string ClientName { get; set; }
        public string ContractAdministratorName { get; set; }
        [Required]
        [RegularExpression(@"^(\D* \D*,|\D* \D*)$", ErrorMessage = "Characters are not allowed.")]
        public string Advisors { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime ClosingDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime EffectiveDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime EndDate { get; set; }

    }
}
