using System.ComponentModel.DataAnnotations;

namespace Common {
    public class GetUserRequest : GetAllDatasRequest {
        public string? Search { get; set; }
    }

    public class GetUserRequestV1 : DTObaseRequest {
        [Required(ErrorMessage = "Requires at least 1 character")]
        [StringLength(20)]
        public string Search { get; set; }
    }
}