using System;

namespace resource_reservation_system_backend.Models;

public class ItemAllocation
{
  public int Id {get; set;}
  public int InventoryId {get; set;}
  public string Name {get; set;} = null!;
  public int Quantity {get; set;}

  public int FacilityId {get; set;}
  public Facility Facility {get; set;} = null!;
}
