namespace Common {
    public class UserProfileDto {
        public string? LoginId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? BusinessUnit { get; set; }
        public string? Department { get; set; }
        public string? Division { get; set; }
        public string? Section { get; set; }

        public Boolean IsSystemAdmin { get; set; }
        public RoleDto?[]? Roles { get; set; }
        public string[]? AccessCodes { get; set; }

        public string? UserType { get; set; }
        public string? VendorNo { get; set; }

        public string? PurchasingGroup { get; set; }
    }
    public class RoleDto {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    public class UserProfile {
        public string? LoginId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? BusinessUnit { get; set; }
        public string? Department { get; set; }
        public string? Division { get; set; }
        public string? Section { get; set; }

        public bool IsSystemAdmin { get; set; }
        public Role[]? Roles { get; set; }
        public string[]? AccessCodes { get; set; }

        public string? UserType { get; set; }
        public string? VendorNo { get; set; }

        public string? PurchasingGroup { get; set; }
    }
    public class Role {
        public string? SystemCode { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string[]? AccessCodes { get; set; }
        public bool IsPermanent { get; set; }
        public string? DelegateFrom { get; set; }
        public string? EffectiveStartDate { get; set; }
        public string? EffectiveEndDate { get; set; }
    }
}
