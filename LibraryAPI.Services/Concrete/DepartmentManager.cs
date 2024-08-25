using AutoMapper;
using LibraryAPI.Entities.DTOs.DepartmentDTO;
using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public DepartmentManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(bool trackChanges)
        {
            var departments = await _repositoryManager.DepartmentRepository.GetAllDepartmentsAsync(trackChanges);
            return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
        }

        public async Task<DepartmentResponse> GetDepartmentByIdAsync(short id, bool trackChanges)
        {
            var department = await _repositoryManager.DepartmentRepository.GetDepartmentByIdAsync(id, trackChanges);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task AddDepartmentAsync(DepartmentRequest departmentRequest)
        {
            var existingDepartment = await _repositoryManager.DepartmentRepository
                .FindByCondition(d => d.Name == departmentRequest.Name, false)
                .FirstOrDefaultAsync();

            if (existingDepartment != null)
                throw new InvalidOperationException("The department name already exists!");

            var department = _mapper.Map<Department>(departmentRequest);

            await _repositoryManager.DepartmentRepository.CreateAsync(department);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateDepartmentAsync(short id, DepartmentRequest departmentRequest)
        {
            var department = await _repositoryManager.DepartmentRepository.GetDepartmentByIdAsync(id, true);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            var departmentWithSameName = await _repositoryManager.DepartmentRepository
                .FindByCondition(d => d.Name == departmentRequest.Name && d.DepartmentId != id, false)
                .FirstOrDefaultAsync();

            if (departmentWithSameName != null)
                throw new InvalidOperationException("The department name already exists!");

            department.Name = departmentRequest.Name;
            _repositoryManager.DepartmentRepository.Update(department);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeDepartmentResponse>> GetEmployeesByDepartmentIdAsync(short id, bool trackChanges)
        {
            var department = await _repositoryManager.DepartmentRepository
                .FindByCondition(d => d.DepartmentId == id, trackChanges)
                .Include(d => d.Employees)
                    .ThenInclude(e => e.ApplicationUser)
                .FirstOrDefaultAsync();

            if (department == null)
                throw new KeyNotFoundException("Department not found");

            var employeeResponses = department.Employees!.Select(e => new EmployeeDepartmentResponse
            {
                EmployeeId = e.EmployeeId,
                Name = e.ApplicationUser!.Name,
                LastName = e.ApplicationUser.LastName,
                Email = e.ApplicationUser.Email,
                PhoneNumber = e.ApplicationUser.PhoneNumber,
                Gender = e.ApplicationUser.Gender,
                EmployeeTitle = e.EmployeeTitle,
                Status = e.Status
            }).ToList();

            return employeeResponses;
        }

        public async Task DeleteDepartmentAsync(short id)
        {
            var department = await _repositoryManager.DepartmentRepository.GetDepartmentByIdAsync(id, true);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            _repositoryManager.DepartmentRepository.Delete(department);
            await _repositoryManager.SaveAsync();
        }
    }
}
