using System;

namespace Common.DTOs {
    public class UserRoleDTO : IDTO {
        public string Code { get; set; }

        public string Description { get; set; }

        public string AccessCodes { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        public DateTime? EffectiveEndDate { get; set; }
    }
}
