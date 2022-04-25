using System.ComponentModel.DataAnnotations;

namespace Common {
    public class GetUserRequest : GetAllDatasRequest {
        public string? Search { get; set; }
    }
}