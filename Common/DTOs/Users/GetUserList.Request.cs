using System.ComponentModel.DataAnnotations;

namespace Common {
    public class GetUserRequest : DTObase {
        [Required(ErrorMessage = "Requires at least 1 character")]
        [StringLength(20)]
        public string Search { get; set; }
    }
}