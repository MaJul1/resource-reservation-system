using System;

namespace resource_reservation_system_backend.DTO.Resource;

public class ResourceDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Type { get; set; } = null!;
  public string Description { get; set; } = null!;
}
