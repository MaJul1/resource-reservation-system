using System;

namespace resource_reservation_system_backend.DTO.ItemAllocation;

public class ItemAllocationDTO
{
  public int Id {get; set;}
  public int InventoryId {get; set;}
  public string Name {get; set;} = null!;
  public int Quantity {get; set;}
}
