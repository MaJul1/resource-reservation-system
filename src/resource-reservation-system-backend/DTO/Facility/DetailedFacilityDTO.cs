using System;
using resource_reservation_system_backend.DTO.Department;
using resource_reservation_system_backend.DTO.ItemAllocation;

namespace resource_reservation_system_backend.DTO.Facility;

public class DetailedFacilityDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Type { get; set; } = null!;
  public string Location { get; set; } = null!;
  public int Capacity { get; set; }
  public string Description { get; set; } = null!;
  public IEnumerable<ItemAllocationDTO> ItemsAllocated { get; set; } = [];
  public IEnumerable<SummaryDepartmentDTO> Departments { get; set; } = [];
}
