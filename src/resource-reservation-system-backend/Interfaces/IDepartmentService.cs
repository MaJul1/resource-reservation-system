using System;
using resource_reservation_system_backend.DTO.Department;

namespace resource_reservation_system_backend.Interfaces;

public interface IDepartmentService
{
  Task<DepartmentDTO> GetDepartmentById(int id);
  Task<IEnumerable<DepartmentDTO>> GetAllDepartments();
  Task CreateDepartment(string name);
  Task UpdateDepartment(int id, string name);
  Task DeleteDepartment(int id);
}
