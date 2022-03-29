using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Service {
    public class TestUserService : IUserService {
        public UserProfileDTO? GetUserProfile(string loginData) {
            throw new NotImplementedException();
        }

        public UserProfileDTO? GetUserProfile(IIdentity userIdentity) {
            throw new NotImplementedException();
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string loginData) {
            byte[] ba = new byte[32];
            new Random().NextBytes(ba);
            return new UserProfileDTO() {
                EffectiveEndDate = DateTime.Now,
                IsSysAdmin = false,
                LastUpdateBy = "Me",
                LastUpdatedDateTime = DateTime.Now,
                LoginIDDTO = new EntityCodeDTO() {
                    Code = loginData,
                    RecordVersion = ba
                },
                Name = loginData,
                AccessCodes = new List<string>() {
                    "Code1",
                    "Code2",
                    "Code3"
                },
                UserRoles = new List<UserRoleDTO>() {
                    new UserRoleDTO() {
                        Code = loginData,
                        Description = "Test User",
                        EffectiveStartDate = DateTime.Now,
                        EffectiveEndDate = DateTime.Now,
                        AccessCodes = "Code1|Code2|Code3"
                    }
                }
            };
        }

        public Task<UserProfileDTO?> GetUserProfileAsync(IIdentity userIdentity) {
            throw new NotImplementedException();
        }
    }
}
