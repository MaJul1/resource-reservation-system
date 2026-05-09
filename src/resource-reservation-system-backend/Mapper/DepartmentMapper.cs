using System;
using resource_reservation_system_backend.DTO.Department;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class DepartmentMapper
{
  public static SummaryDepartmentDTO ToDepartmentDTO(this Department department)
  {
    return new SummaryDepartmentDTO
    {
      Id = department.Id,
      Name = department.Name
    };
  }

  public static DetailedDepartmentDTO ToDetailedDepartmentDTO(this Department department)
  {
    return new DetailedDepartmentDTO
    {
      Id = department.Id,
      Name = department.Name,
      Facilities = department.Facilities.Select(f => f.ToSummarizedFacilityDTO())
    };
  }
}
