using System;
using resource_reservation_system_backend.DTO.Department;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class DepartmentMapper
{
  public static DepartmentDTO ToDepartmentDTO(this Department department)
  {
    return new DepartmentDTO
    {
      Id = department.Id,
      Name = department.Name
    };
  }
}
