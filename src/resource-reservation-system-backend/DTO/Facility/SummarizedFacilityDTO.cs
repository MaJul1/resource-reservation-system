using System;

namespace resource_reservation_system_backend.DTO.Facility;

public class SummarizedFacilityDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Type { get; set; } = null!;
  public string Location { get; set; } = null!;
  public int Capacity { get; set; }
  public string Description { get; set; } = null!;
}
