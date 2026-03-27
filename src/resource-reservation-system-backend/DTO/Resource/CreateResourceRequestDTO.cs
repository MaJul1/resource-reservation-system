using System;

namespace resource_reservation_system_backend.DTO.Resource;

public class CreateResourceRequestDTO
{
  public string Name { get; set; } = null!;
  public string Type { get; set; } = null!;
  public string Description { get; set; } = null!;
}
