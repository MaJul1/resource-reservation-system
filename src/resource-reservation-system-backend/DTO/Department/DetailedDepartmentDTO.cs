using System;
using resource_reservation_system_backend.DTO.Facility;

namespace resource_reservation_system_backend.DTO.Department;

public class DetailedDepartmentDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public IEnumerable<SummarizedFacilityDTO> Facilities { get; set; } = [];
}
