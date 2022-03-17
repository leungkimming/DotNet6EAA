using Common.DTOs.Users;
using Data.EF.Interfaces;
using Business.Users;
using Business.Departments;
using Common.Shared;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Service.Users
{
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

        public async Task<AddUserResponse> AddNewAsync(AddUserRequest model)
        {
            _logger.LogWarning(2000, "Add " + model.UserName);
            var repository = UnitOfWork.AsyncRepository<User>();
            var checkuser = await repository.GetAsync(x => x.UserName == model.UserName);
            if (checkuser != null)
            {
                throw new UserAlreadyExistException(model.UserName);
            }

            var deptRepos = UnitOfWork.AsyncRepository<Department>();
            var dept = await deptRepos.GetAsync(_ => _.Id == model.DepartmentId);
            var user = _mapper.Map<User>(model);

            if (dept != null) {
                user.AddDepartment(dept);
            }

            await repository.AddAsync(user);
            await UnitOfWork.SaveChangesAsync();

            var response = _mapper.Map<AddUserResponse>(user);

            return response;
        }

        public async Task<AddPayslipResponse> AddUserPayslipAsync(AddPayslipRequest model)
        {
            var repository = UnitOfWork.UserRepository();
            var user = await repository.GetAsyncWithPayslip(_ => _.Id == model.UserId);
            if (user != null)
            {
                var payslip = user.AddPayslip(model.Date.Value
                    , model.WorkingDays.Value
                    , model.Bonus
                    , model.IsPaid.Value);

                await repository.UpdateAsync(user);
                await UnitOfWork.SaveChangesAsync();

                return new AddPayslipResponse()
                {
                    UserId = user.Id,
                    TotalSalary = payslip.TotalSalary,
                    LetterSentDate = payslip.LetterSentDate,
                    Letter = payslip.Letter
                };
            }

            throw new Exception("User not found.");
        }
        public async Task<List<UserInfoDTO>> SearchAsync(GetUserRequest request)
        {
            //          var repository = UnitOfWork.AsyncRepository<User>();
            var repository = UnitOfWork.UserRepository();
            var users = await repository
                .ListAsyncwithDept(_ => _.UserName.Contains(request.Search));
//              .ListAsync(_ => _.UserName.Contains(request.Search));
            
            var userDTOs = users.Select(_user => _mapper.Map<UserInfoDTO>(_user)).ToList();
            return userDTOs;
        }
        public async Task<List<PayslipDTO>> SearchAsync(GetPayslipRequest request)
        {
            var repository = UnitOfWork.AsyncRepository<Payslip>();
            var payslips = await repository
                 .ListAsync(x => x.UserId == request.userId);

            var payslipDTOs = payslips.Select(x => _mapper.Map<PayslipDTO>(x)).ToList();
            return payslipDTOs;
        }

    }
}