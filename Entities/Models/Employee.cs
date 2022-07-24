using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Name field is Required")]
        [MaxLength(30,ErrorMessage ="Maximum length for name is 30")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Age field is required")]
        [MaxLength(20,ErrorMessage ="Maximum length of Age field is 20")]
        public int Age { get; set; }
        [Required(ErrorMessage ="Position field is required")]
        public string Position { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
