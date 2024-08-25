using AutoMapper;
using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync(bool trackChanges)
        {
            var employees = await _repositoryManager.EmployeeRepository.GetAllEmployeesAsync(trackChanges);
            return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse> GetEmployeeByIdNumberAsync(string idNumber, bool trackChanges)
        {
            var employee = await _repositoryManager.EmployeeRepository
                .FindByCondition(e => e.ApplicationUser!.IdNumber == idNumber, trackChanges)
                .Include(e => e.ApplicationUser)
                .Include(e => e.Department)
                .Include(e => e.EmployeeAddresses)
                .FirstOrDefaultAsync();

            if (employee == null)
                throw new KeyNotFoundException("Employee not found");

            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task AddEmployeeAsync(EmployeeRequest employeeRequest)
        {
            var existingDepartment = await _repositoryManager.DepartmentRepository
                .FindByCondition(d => d.DepartmentId == employeeRequest.DepartmentId, false)
                .FirstOrDefaultAsync();

            if (existingDepartment == null)
                throw new InvalidOperationException("The department does not exist.");

            var existingUser = await _userManager.FindByEmailAsync(employeeRequest.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var existingEmployee = await _repositoryManager.EmployeeRepository
                .FindByCondition(e => e.ApplicationUser!.IdNumber == employeeRequest.IdNumber, false)
                .FirstOrDefaultAsync();

            if (existingEmployee != null)
                throw new InvalidOperationException("A user with this ID number already exists.");

            var user = new ApplicationUser
            {
                IdNumber = employeeRequest.IdNumber,
                Name = employeeRequest.Name,
                LastName = employeeRequest.LastName,
                Gender = employeeRequest.Gender,
                BirthDate = employeeRequest.BirthDate,
                UserName = employeeRequest.UserName,
                PhoneNumber = employeeRequest.PhoneNumber,
                Email = employeeRequest.Email,
            };

            var result = await _userManager.CreateAsync(user, employeeRequest.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, employeeRequest.EmployeeTitle!.ToString());
            if (!roleResult.Succeeded)
                throw new InvalidOperationException(string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            var employee = new Employee
            {
                EmployeeId = user.Id,
                Salary = employeeRequest.Salary,
                EmployeeShift = employeeRequest.EmployeeShift,
                EmployeeTitle = employeeRequest.EmployeeTitle,
                DepartmentId = employeeRequest.DepartmentId,
                Status = EmployeeStatus.Working.ToString()
            };

            await _repositoryManager.EmployeeRepository.CreateAsync(employee);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest)
        {
            var employee = await _repositoryManager.EmployeeRepository.GetEmployeeByIdAsync(id, true);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found");

            var existingUser = await _repositoryManager.EmployeeRepository
                .FindByCondition(e => e.ApplicationUser!.IdNumber == employeeRequest.IdNumber && e.EmployeeId != id, false)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                throw new InvalidOperationException("A user with this ID number already exists.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.IdNumber = employeeRequest.IdNumber;
            user.Name = employeeRequest.Name;
            user.LastName = employeeRequest.LastName;
            user.Gender = employeeRequest.Gender;
            user.BirthDate = employeeRequest.BirthDate;
            user.UserName = employeeRequest.UserName;
            user.PhoneNumber = employeeRequest.PhoneNumber;
            user.Email = employeeRequest.Email;

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded)
                throw new InvalidOperationException(string.Join(", ", userResult.Errors.Select(e => e.Description)));

            if (employee.EmployeeTitle != employeeRequest.EmployeeTitle)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    throw new InvalidOperationException(string.Join(", ", removeResult.Errors.Select(e => e.Description)));

                var addRoleResult = await _userManager.AddToRoleAsync(user, employeeRequest.EmployeeTitle!.ToString());
                if (!addRoleResult.Succeeded)
                    throw new InvalidOperationException(string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
            }

            employee.Salary = employeeRequest.Salary;
            employee.EmployeeShift = employeeRequest.EmployeeShift;
            employee.EmployeeTitle = employeeRequest.EmployeeTitle;
            employee.DepartmentId = employeeRequest.DepartmentId;

            _repositoryManager.EmployeeRepository.Update(employee);
            await _repositoryManager.SaveAsync();
        }

        public async Task SetEmployeeStatusAsync(string id, string status)
        {
            var employee = await _repositoryManager.EmployeeRepository.GetEmployeeByIdAsync(id, true);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found");

            employee.Status = status;

            _repositoryManager.EmployeeRepository.Update(employee);
            await _repositoryManager.SaveAsync();
        }

        public async Task AddEmployeeAddressAsync(EmployeeAddress employeeAddress)
        {
            var employee = await _repositoryManager.EmployeeRepository.GetEmployeeByIdAsync(employeeAddress.EmployeeId, false);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found");

            await _repositoryManager.EmployeeAddressRepository.CreateAsync(employeeAddress);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateEmployeePasswordAsync(string id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
