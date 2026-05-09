using System;
using resource_reservation_system_backend.DTO.Department;

namespace resource_reservation_system_backend.Interfaces;

public interface IDepartmentService
{
  Task<DetailedDepartmentDTO> GetDepartmentById(int id);
  Task<IEnumerable<SummaryDepartmentDTO>> GetAllDepartments();
  Task CreateDepartment(string name);
  Task UpdateDepartment(int id, string name);
  Task DeleteDepartment(int id);
}
