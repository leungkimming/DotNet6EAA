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
        public UserService(IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor) {
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
                user.AddDepartment(dept);
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
                var payslip = user.AddPayslip(model.Date.Value
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
        public async Task<List<UserInfoDTO>> SearchAsync(GetUserRequest request) {
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
            return new List<Claim> {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, "AA01"),
                new Claim(ClaimTypes.Role, "AB01"),
                new Claim(ClaimTypes.Role, "AC01"),
                new Claim(ClaimTypes.UserData, "{StaffId=41776"),
                new Claim(ClaimTypes.UserData, "{Section=DIA"),
                new Claim(ClaimTypes.UserData, "{DateJoin=20220406")
            };
        }
    }
}