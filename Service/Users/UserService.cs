using Common;
using Data;
using Business;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace Service {
    public class UserService : BaseService {
        private readonly GridCommon2Service _gridCommon2Service;
        public UserService(IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor, GridCommon2Service gridCommon2Service) : base(unitOfWork, logger, mapper, httpContextAccessor) {
            _gridCommon2Service = gridCommon2Service;
        }

        public async Task<AddUserResponse> AddNewAsync(AddUserRequest model) {
            _logger.LogWarning(2000, "Add " + model.UserName);
            var repository = UnitOfWork.AsyncRepository<User>();
            var checkuser = await repository.GetAsync(x => x.UserName == model.UserName);
            if (checkuser != null) {
                throw new UserAlreadyExistException(model.UserName);
            }

            var deptRepos = UnitOfWork.AsyncRepository<Department>();
            var dept = await deptRepos.GetAsync(_ => _.Name == model.DepartmentName);
            var user = _mapper.Map<User>(model);

            if (dept != null) {
                user.JoinDepartment(dept);
            }

            await repository.AddAsync(user);
            await UnitOfWork.SaveChangesAsync();

            var response = _mapper.Map<AddUserResponse>(user);

            return response;
        }

        public async Task<AddPayslipResponse> AddUserPayslipAsync(AddPayslipRequest model) {
            var repository = UnitOfWork.UserRepository();
            var user = await repository.GetAsyncWithPayslip(_ => _.Id == model.UserDTO.Id);
            if (user != null) {
                if (StructuralComparisons.StructuralComparer.Compare(user.RowVersion, model.UserDTO.RowVersion) != 0) {
                    throw new RecordVersionException();
                }
                var payslip = user.IssuePayslip(model.Date.Value
                    , model.WorkingDays.Value
                    , model.Bonus
                    , model.IsPaid.Value);

                await repository.UpdateAsync(user);
                await UnitOfWork.SaveChangesAsync();

                return new AddPayslipResponse() {
                    UserId = user.Id,
                    TotalSalary = payslip.TotalSalary,
                    LetterSentDate = payslip.LetterSentDate,
                    Letter = payslip.Letter
                };
            }

            throw new Exception("User not found.");
        }
        public async Task<GetAllDatasResponse<UserInfoDTO>> SearchAsync(GetUserRequest request) {
            var x = _httpContext.User;
            var repository = UnitOfWork.UserRepository();
            var users = await repository
                .ListAsyncwithDeptByPagging(_ => request.Search==null||_.UserName.Contains(request.Search),request.RecordsPerPage,request.PageNo);

            var totalCount=await repository.ListCountAsync(_ => request.Search==null||_.UserName.Contains(request.Search));

            var userDTOs = users.Select(_user => _mapper.Map<UserInfoDTO>(_user)).ToList();
            GetAllDatasResponse<UserInfoDTO> searchResult = new GetAllDatasResponse<UserInfoDTO>();
            searchResult.Datas = userDTOs;
            searchResult.TotalCount = totalCount;
            return searchResult;
        }
        public async Task<List<UserInfoDTO>> SearchAsyncV1(GetUserRequestV1 request) {
            var x = _httpContext.User;
            var repository = UnitOfWork.UserRepository();
            var users = await repository
                .ListAsyncwithDept(_ => _.UserName.Contains(request.Search));

            var userDTOs = users.Select(_user => _mapper.Map<UserInfoDTO>(_user)).ToList();
            return userDTOs;
        }

        public async Task<List<PayslipDTO>> SearchAsync(GetPayslipRequest request) {
            var repository = UnitOfWork.AsyncRepository<Payslip>();
            var payslips = await repository
                 .ListAsync(x => x.UserId == request.userId);

            var payslipDTOs = payslips.Select(x => _mapper.Map<PayslipDTO>(x)).ToList();
            return payslipDTOs;
        }
        public List<Claim> GetUserClaims(string userId) {
            var userClaim=new List<Claim>{
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.UserData, "{StaffId=41776"),
                new Claim(ClaimTypes.UserData, "{Section=DIA"),
                new Claim(ClaimTypes.UserData, "{DateJoin=20220406")
            };
            try {
                UserProfileDto userProfile = this._gridCommon2Service.GetUserProfile("ditasdasd").Result;
                if (userProfile.AccessCodes != null && userProfile.AccessCodes.Count() > 0) {
                    foreach (var accessCdoe in userProfile.AccessCodes) {
                        userClaim.Add(new Claim(ClaimTypes.Role, accessCdoe));
                    }
                    return userClaim;
                }
            } catch (Exception ex) {
               //Just for debug 
            }          
            var mockRoles=new List<Claim> {
                new Claim(ClaimTypes.Role, "AA01"),
                new Claim(ClaimTypes.Role, "AB01"),
                new Claim(ClaimTypes.Role, "AC01"),
                new Claim(ClaimTypes.Role, "SP01"),
                new Claim(ClaimTypes.Role, "SP02"),
                new Claim(ClaimTypes.Role, "SP03"),
                new Claim(ClaimTypes.Role, "SP04"),
                new Claim(ClaimTypes.Role, "RA01")
            };
            userClaim.AddRange(mockRoles);
            return userClaim;
        }
    }
}