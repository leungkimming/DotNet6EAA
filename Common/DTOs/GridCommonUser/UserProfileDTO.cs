using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.DTOs {
    public class UserProfileDTO : IOutputDTO {

        public UserProfileDTO() {
            UserRoles = new List<UserRoleDTO>();
            AccessCodes = new List<string>();
        }

        public EntityCodeDTO LoginIDDTO { get; set; }

        public string Name { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public string LastUpdateBy { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public bool IsSysAdmin { get; set; }

        public List<UserRoleDTO> UserRoles { get; init; }

        public List<string> AccessCodes {
            get => UserRoles
                    .Where(r => r.EffectiveEndDate == null || r.EffectiveEndDate >= DateTime.Today)
                    .SelectMany(o => o.AccessCodes.Split('|')).ToList();
            init {
                if (AccessCodes is null) {
                    AccessCodes = value;
                }
            }
        }
    }
}
