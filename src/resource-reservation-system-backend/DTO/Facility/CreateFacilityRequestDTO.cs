using System;

namespace resource_reservation_system_backend.DTO.Facility;

public class CreateFacilityRequestDTO
{
  public string Name { get; set; } = null!;
  public string Type { get; set; } = null!;
  public string Location { get; set; } = null!;
  public int Capacity { get; set; }
  public string Description { get; set; } = null!;
  public IEnumerable<ItemRecordDTO> ItemsAllocated { get; set; } = [];
}

public record ItemRecordDTO (int InventoryId, int Quantity);
