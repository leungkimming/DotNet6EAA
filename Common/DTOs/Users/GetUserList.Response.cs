﻿
namespace Common {
    public class UserInfoDTO : DTObaseResponse {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public float CoefficientsSalary { get; set; }
        public PayslipDTO[]? PayslipDTOs {get; set;}
    }
}