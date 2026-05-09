using System;

namespace resource_reservation_system_backend.Models;

public class AvailableItem
{
  public int Inventory_id {get; set;}
  public string Name {get; set;} = null!;
  public string Description {get; set;} = null!;
  public int Quantity {get; set;}
}
