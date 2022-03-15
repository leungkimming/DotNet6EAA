using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Users
{
    public class GetUserRequest
    {
        [Required(ErrorMessage = "Requires at least 1 character")]
        [StringLength(20)]
        public string Search { get; set; }
    }
}