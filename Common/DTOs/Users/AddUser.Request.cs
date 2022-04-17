using System.ComponentModel.DataAnnotations;

namespace Common {
    public class AddUserRequest : DTObaseRequest {
        [Required(ErrorMessage = "UserName is mandatory")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is mandatory")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is mandatory")]
        [StringLength(125)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is mandatory")]
        [StringLength(255)]
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Department is mandatory")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Coefficients Salary is mandatory")]
        public float CoefficientsSalary { get; set; }
    }
}