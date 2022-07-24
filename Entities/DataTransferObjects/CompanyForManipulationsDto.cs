using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CompanyForManipulationsDto
    {
        [Required(ErrorMessage = "Name field qis required")]
        [MaxLength(60, ErrorMessage = "Maximum length of name is characters is 60")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address field is required")]
        [MaxLength(60, ErrorMessage = "Maximum length of address is characters is 60")]
        public string Address { get; set; }
        public string Country { get; set; }
        public IEnumerable<EmployeeCreateDto>? Employees { get; set; }
    }
}
